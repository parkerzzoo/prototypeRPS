using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ParkerLibrary.EventSystem;
using Boomlagoon.JSON;
using DG.Tweening;

public class FlowControl_FewPeople : FlowControl
{
	bool handSelectable = false;
	[SerializeField] private TMPro.TMP_InputField nameInputField;

	public void OnEvent(EVENT_TYPE eventType, Component sender, object param = null)
	{
		switch (eventType)
		{
			case EVENT_TYPE.WS_CONNECTED: OnConnected(); break;

			case EVENT_TYPE.FEW_PEOPLE_MODE_USER_LIST_CHANGE: OnUserListChange((JSONObject)param); break;
			case EVENT_TYPE.FEW_PEOPLE_MODE_START_GAME: OnStartGame((JSONObject)param); break;
			case EVENT_TYPE.FEW_PEOPLE_MODE_START_ROUND: OnStartRound((JSONObject)param); break;
			case EVENT_TYPE.FEW_PEOPLE_MODE_END_ROUND: OnEndRound((JSONObject)param); break;
			case EVENT_TYPE.FEW_PEOPLE_MODE_END_GAME: OnEndGame((JSONObject)param); break;
			case EVENT_TYPE.FEW_PEOPLE_MODE_RESET_ROUND: OnResetRound((JSONObject)param); break;
			case EVENT_TYPE.FEW_PEOPLE_MODE_RESET_GAME: OnResetGame((JSONObject)param); break;
			case EVENT_TYPE.FEW_PEOPLE_MODE_USER_SPEAK: OnUserSpeak((JSONObject)param); break;

			case EVENT_TYPE.FEW_PEOPLE_MODE_USER_HAND_CHANGE: OnUserHandChange((JSONObject)param); break;
				
			case EVENT_TYPE.FEW_PEOPLE_MODE_CONNECT_ERROR: OnConnectError(); break;
		}
	}

	void Start()
	{
		EventManager.Instance.AddListener(EVENT_TYPE.WS_CONNECTED, OnEvent);

		EventManager.Instance.AddListener(EVENT_TYPE.FEW_PEOPLE_MODE_USER_LIST_CHANGE, OnEvent);
		EventManager.Instance.AddListener(EVENT_TYPE.FEW_PEOPLE_MODE_START_GAME, OnEvent);
		EventManager.Instance.AddListener(EVENT_TYPE.FEW_PEOPLE_MODE_START_ROUND, OnEvent);
		EventManager.Instance.AddListener(EVENT_TYPE.FEW_PEOPLE_MODE_END_ROUND, OnEvent);
		EventManager.Instance.AddListener(EVENT_TYPE.FEW_PEOPLE_MODE_END_GAME, OnEvent);
		EventManager.Instance.AddListener(EVENT_TYPE.FEW_PEOPLE_MODE_RESET_ROUND, OnEvent);
		EventManager.Instance.AddListener(EVENT_TYPE.FEW_PEOPLE_MODE_RESET_GAME, OnEvent);

		EventManager.Instance.AddListener(EVENT_TYPE.FEW_PEOPLE_MODE_USER_SPEAK, OnEvent);

		EventManager.Instance.AddListener(EVENT_TYPE.FEW_PEOPLE_MODE_USER_HAND_CHANGE, OnEvent);

		EventManager.Instance.AddListener(EVENT_TYPE.FEW_PEOPLE_MODE_CONNECT_ERROR, OnEvent);
	}

	void OnDestroy()
	{
		EventManager.Instance.RemoveListener(EVENT_TYPE.WS_CONNECTED, OnEvent);

		EventManager.Instance.RemoveListener(EVENT_TYPE.FEW_PEOPLE_MODE_USER_LIST_CHANGE, OnEvent);
		EventManager.Instance.RemoveListener(EVENT_TYPE.FEW_PEOPLE_MODE_START_GAME, OnEvent);
		EventManager.Instance.RemoveListener(EVENT_TYPE.FEW_PEOPLE_MODE_START_ROUND, OnEvent);
		EventManager.Instance.RemoveListener(EVENT_TYPE.FEW_PEOPLE_MODE_END_ROUND, OnEvent);
		EventManager.Instance.RemoveListener(EVENT_TYPE.FEW_PEOPLE_MODE_END_GAME, OnEvent);
		EventManager.Instance.RemoveListener(EVENT_TYPE.FEW_PEOPLE_MODE_RESET_ROUND, OnEvent);
		EventManager.Instance.RemoveListener(EVENT_TYPE.FEW_PEOPLE_MODE_RESET_GAME, OnEvent);

		EventManager.Instance.RemoveListener(EVENT_TYPE.FEW_PEOPLE_MODE_USER_SPEAK, OnEvent);

		EventManager.Instance.RemoveListener(EVENT_TYPE.FEW_PEOPLE_MODE_USER_HAND_CHANGE, OnEvent);

		EventManager.Instance.RemoveListener(EVENT_TYPE.FEW_PEOPLE_MODE_CONNECT_ERROR, OnEvent);
	}

	void OnConnected()
	{
		JSONObject _data = new JSONObject();
		_data.Add("userId", VarList.userId);
		_data.Add("point", 100);
		_data.Add("nickname", nameInputField.text);

		SocketControl_FewPeople.Instance.SendData("identity", _data);
		SocketControl_FewPeople.Instance.SendData("userListChange");

		UIControl_FewPeople.Instance.FadeOutCoverImage();
	}

	void OnUserListChange(JSONObject data)
	{
		JSONArray _userDatas = data.GetArray("users");
		List<UserData> _userList = UserData.ParseUserList(_userDatas);

		HandObjectControl_FewPeople.Instance.OnUserListChange(_userList);
	}

	void OnStartGame(JSONObject data)
	{
		float _duration = (float)data.GetNumber("delay");
		if (_duration > 10)
			_duration = 10f;

		UIControl_FewPeople.Instance.ShowCenterTextPanel("게임 시작", 0f, _duration);

		JSONArray _userDatas = data.GetArray("users");
		List<UserData> _userList = UserData.ParseUserList(_userDatas);

		HandObjectControl_FewPeople.Instance.OnStartGame(_userList);
	}

	void OnStartRound(JSONObject data)
	{
		float _duration = (float)data.GetNumber("delay");
		if (_duration > 10)
			_duration = 10f;

		JSONArray _userDatas = data.GetArray("users");
		List<UserData> _userList = UserData.ParseUserList(_userDatas);

		for (int i = 0; i < _userList.Count; i++)
		{
			if (_userList[i].IsMe)
			{
				handSelectable = _userList[i].possiblePlay;
				break;
			}
		}

		UIControl_FewPeople.Instance.ShowCenterTimerPanel(_duration);

		HandObjectControl_FewPeople.Instance.OnStartRound(_userList);
	}

	void OnEndRound(JSONObject data)
	{
		JSONArray _userDatas = data.GetArray("users");
		List<UserData> _userList = UserData.ParseUserList(_userDatas);

		float _duration = (float)data.GetNumber("delay");
		if (_duration > 10)
			_duration = 10f;

		// 내 유저정보를 찾아서, 결과를 출력해줌.
		for (int i = 0; i < _userList.Count; i++)
		{
			if (_userList[i].IsMe)
			{
				if (!HandObjectControl_FewPeople.Instance.MyHandObject.Dead)
				{
					string _result = _userList[i].currentResult.ToString();
					UIControl_FewPeople.Instance.ShowCenterTextPanel(_result, 0f, _duration);
				}
				break;
			}
		}

		HandType _frontHand = HandType.empty;
		string _hand = !data.ContainsKey("currentAdminHand") ? null : data.GetString("currentAdminHand");
		if (string.IsNullOrEmpty(_hand))
		{
			_frontHand = HandType.empty;
		}
		else
		{
			switch (_hand)
			{
				case "rock": _frontHand = HandType.rock; break;
				case "paper": _frontHand = HandType.paper; break;
				case "scissors": _frontHand = HandType.scissors; break;
			}
		}

		HandObjectControl_FewPeople.Instance.OnEndRound(_userList, _frontHand, _duration);
	}

	void OnEndGame(JSONObject data)
	{
		JSONArray _userDatas = data.GetArray("users");
		List<UserData> _userList = UserData.ParseUserList(_userDatas);
		for (int i = 0; i < _userList.Count; i++)
		{
			if (_userList[i].IsMe)
			{
				UIControl_FewPeople.Instance.UpdateChipCountText(_userList[i].point);
				break;
			}
		}

		string _winnerName = null;
		for (int i = 0; i < _userList.Count; i++)
		{
			if (_userList[i].isAlive)
			{
				_winnerName = _userList[i].nickname;
				break;
			}
		}

		if (string.IsNullOrEmpty(_winnerName))
			_winnerName = "없음";
		UIControl_FewPeople.Instance.ShowCenterTextPanel("우승자: " + _winnerName, 0f, 3f);

		HandObjectControl_FewPeople.Instance.OnEndGame(_userList);
	}

	void OnResetRound(JSONObject data)
	{
		handSelectable = false;

		JSONArray _userDatas = data.GetArray("users");
		List<UserData> _userList = UserData.ParseUserList(_userDatas);

		HandObjectControl_FewPeople.Instance.OnResetRound(_userList);
	}

	void OnResetGame(JSONObject data)
	{
		handSelectable = false;

		JSONArray _userDatas = data.GetArray("users");
		List<UserData> _userList = UserData.ParseUserList(_userDatas);

		HandObjectControl_FewPeople.Instance.OnResetGame(_userList);
	}

	void OnFrontHandSpeak(JSONObject data)
	{
		string _msg = data.GetString("adminMsg");

		UIControl_FewPeople.Instance.ActiveFrontHandSpeak(_msg);
	}

	void OnUserSpeak(JSONObject data)
	{
		string _msg = data.GetString("msg");

		UIControl_FewPeople.Instance.CreatChatObject(_msg);
	}

	void OnUserHandChange(JSONObject data)
	{
		JSONArray _userDatas = data.GetArray("users");
		List<UserData> _userList = UserData.ParseUserList(_userDatas);

		HandObjectControl_FewPeople.Instance.OnUserHandChange(_userList);
	}

	void OnConnectError()
    {
		UIControl_FewPeople.Instance.ActiveConnectErrorPanel();
	}


	// Public Method.

	public void OnClickStart()
	{
		SocketControl_FewPeople.Instance.ConnectWebSocket();
	}

	public void OnSendChat(string content)
    {
		if (string.IsNullOrEmpty(content))
			return;

		JSONObject _data = new JSONObject();
		_data.Add("msg", content);

		SocketControl_FewPeople.Instance.SendData("msg", _data);
	}

	public void OnSelectRock()
    {
		SelectHand(HandType.rock);

		HandObjectControl_FewPeople.Instance.SelectMyHand(HandType.rock);
	}

	public void OnSelectPaper()
	{
		SelectHand(HandType.paper);

		HandObjectControl_FewPeople.Instance.SelectMyHand(HandType.paper);
	}

	public void OnSelectScissors()
	{
		SelectHand(HandType.scissors);

		HandObjectControl_FewPeople.Instance.SelectMyHand(HandType.scissors);
	}

	void SelectHand(HandType handType)
	{
		JSONObject _data = new JSONObject();
		_data.Add("hand", handType.ToString());

		SocketControl_FewPeople.Instance.SendData("setHand", _data);
		HandObjectControl_FewPeople.Instance.SelectMyHand(handType);
	}
}
