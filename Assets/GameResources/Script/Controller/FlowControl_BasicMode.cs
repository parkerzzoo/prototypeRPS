using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ParkerLibrary.EventSystem;
using Boomlagoon.JSON;
using DG.Tweening;

public class FlowControl_BasicMode : Singletone<FlowControl_BasicMode>
{
	public GameObject[] objectsInPossibleSelect;

	bool isPossibleSelect = false;
	bool IsPossibleSelect
    {
		get{ return isPossibleSelect; }
        set
        {
			isPossibleSelect = value;

			for (int i = 0; i < objectsInPossibleSelect.Length; i++)
				objectsInPossibleSelect[i].SetActive(isPossibleSelect);
		}
    }

	public void OnEvent(EVENT_TYPE eventType, Component sender, object param = null)
	{
		switch (eventType)
		{
			case EVENT_TYPE.WS_CONNECTED: OnConnected(); break;

			case EVENT_TYPE.BASIC_MODE_USER_LIST_UPDATE: OnUserListUpdate((JSONObject)param); break;
			case EVENT_TYPE.BASIC_MODE_START_GAME: OnStartGame((JSONObject)param); break;
			case EVENT_TYPE.BASIC_MODE_RETURN_RESULT: OnReturnResult((JSONObject)param); break;
			case EVENT_TYPE.BASIC_MODE_RESET_GAME: OnResetGame(); break;

			case EVENT_TYPE.BASIC_MODE_START_ROUND: OnStartRound((JSONObject)param); break;
			case EVENT_TYPE.BASIC_MODE_END_ROUND: OnEndRound((JSONObject)param); break;
			case EVENT_TYPE.BASIC_MODE_END_GAME: OnEndGame((JSONObject)param); break;
		}
	}

	void Start()
	{
		EventManager.Instance.AddListener(EVENT_TYPE.WS_CONNECTED, OnEvent);
		EventManager.Instance.AddListener(EVENT_TYPE.BASIC_MODE_USER_LIST_UPDATE, OnEvent);
		EventManager.Instance.AddListener(EVENT_TYPE.BASIC_MODE_START_GAME, OnEvent);
		EventManager.Instance.AddListener(EVENT_TYPE.BASIC_MODE_RETURN_RESULT, OnEvent);
		EventManager.Instance.AddListener(EVENT_TYPE.BASIC_MODE_RESET_GAME, OnEvent);

		EventManager.Instance.AddListener(EVENT_TYPE.BASIC_MODE_START_ROUND, OnEvent);
		EventManager.Instance.AddListener(EVENT_TYPE.BASIC_MODE_END_ROUND, OnEvent);
		EventManager.Instance.AddListener(EVENT_TYPE.BASIC_MODE_END_GAME, OnEvent);
		IsPossibleSelect = false;
	}

	void OnDestroy()
	{
		EventManager.Instance.RemoveListener(EVENT_TYPE.WS_CONNECTED, OnEvent);
		EventManager.Instance.RemoveListener(EVENT_TYPE.BASIC_MODE_USER_LIST_UPDATE, OnEvent);
		EventManager.Instance.RemoveListener(EVENT_TYPE.BASIC_MODE_START_GAME, OnEvent);
		EventManager.Instance.RemoveListener(EVENT_TYPE.BASIC_MODE_RETURN_RESULT, OnEvent);
		EventManager.Instance.RemoveListener(EVENT_TYPE.BASIC_MODE_RESET_GAME, OnEvent);

		EventManager.Instance.RemoveListener(EVENT_TYPE.BASIC_MODE_START_ROUND, OnEvent);
		EventManager.Instance.RemoveListener(EVENT_TYPE.BASIC_MODE_END_ROUND, OnEvent);
		EventManager.Instance.RemoveListener(EVENT_TYPE.BASIC_MODE_END_GAME, OnEvent);
	}

	void OnConnected()
	{
		JSONObject _data = new JSONObject();
		_data.Add("userId", VarList.userId);
		_data.Add("point", 100);
		UIControl_BasicMode.Instance.UpdateChipCountText(100);

		SocketIOManager.Instance.SendData("identity", _data);
		SocketIOManager.Instance.SendData("userListChange");
	}

	void OnUserListUpdate(JSONObject data)
	{
		JSONArray _userDatas = data.GetArray("users");
		UpdateUserList(_userDatas);
	}

	void OnStartGame(JSONObject data)
	{
		// 나중에 지워주기.
		UIControl_BasicMode.Instance.startGameButton.SetActive(false);

		float _startDelay = (float)data.GetNumber("delay");
		int _normalRoundCount = (int)data.GetNumber("minimumRoundCount");
		//StartGame(_startDelay);
		//////////HandObjectControl_BasicMode.Instance.StartGame(_normalRoundCount);
		UIControl_BasicMode.Instance.ControlActiveCenterText(true);
		UIControl_BasicMode.Instance.UpdateStartGameText("게임 시작");
	}

	void OnReturnResult(JSONObject data)
	{
		JSONArray _userDatas = data.GetArray("users");
		ResultUserList(_userDatas);
	}

	void OnResetGame()
	{
		ResetGame();
	}

	void OnStartRound(JSONObject data)
    {
		//////////HandObjectControl_BasicMode.Instance.AllReset();
		UIControl_BasicMode.Instance.ControlActiveCenterText(false);
		float _startDelay = (float)data.GetNumber("delay");

		JSONArray _userDatas = data.GetArray("users");
		List<UserData> _userList = UserData.ParseUserList(_userDatas);
		bool _possiblePlayMe = true;
		for (int i = 0; i < _userList.Count; i++)
		{
			if (_userList[i].IsMe)
			{
				_possiblePlayMe = _userList[i].possiblePlay;
				break;
			}
		}


		StartRound(_startDelay, _possiblePlayMe);
	}

	void OnEndRound(JSONObject data)
	{
		JSONArray _userDatas = data.GetArray("users");
		List<UserData> _userList = UserData.ParseUserList(_userDatas);
		// 내 유저정보를 찾아서, 가장 최근 결과를 출력해줌.
		for (int i = 0; i < _userList.Count; i++)
		{
			if (_userList[i].IsMe && _userList[i].possiblePlay)
			{
				int _myResultCount = _userList[i].resultTypes.Length;
				string _result = _myResultCount > 0 ? _userList[i].resultTypes[_myResultCount - 1].ToString() : null;
				if (!string.IsNullOrEmpty(_result))
				{
					UIControl_BasicMode.Instance.InvokeExcute(0.5f, ()=>UIControl_BasicMode.Instance.ControlActiveCenterText(true));
					UIControl_BasicMode.Instance.UpdateStartGameText(_result);
				}
				break;
			}
		}
		string _mode = data.GetString("roundMode");
		bool _isNomalMode = _mode == "normal";
		//////////if (_isNomalMode)
		//////////HandObjectControl_BasicMode.Instance.EndNormalRound(_userList);
		//////////else
		//////////HandObjectControl_BasicMode.Instance.EndDeathMatchRound(_userList);

		//for (int i = 0; i < _userList.Count; i++)
		//	if()
	}

	void OnEndGame(JSONObject data)
	{
		// 나중에 지워주기.
		UIControl_BasicMode.Instance.startGameButton.SetActive(true);

		UIControl_BasicMode.Instance.ControlActiveCenterText(false);
		JSONArray _userDatas = data.GetArray("users");
		List<UserData> _userList = UserData.ParseUserList(_userDatas);
		for (int i = 0; i < _userList.Count; i++)
		{
			if (_userList[i].IsMe)
			{
				UIControl_BasicMode.Instance.UpdateChipCountText(_userList[i].point);
				break;
			}
		}
	}

	void StartRound(float selectionTime, bool possiblePlayerMe)
	{
		IsPossibleSelect = possiblePlayerMe;
		
		UIControl_BasicMode.Instance.StartRound(selectionTime, ()=> IsPossibleSelect = false);
	}

	void ResetGame()
	{
		IsPossibleSelect = false;
		UIControl_BasicMode.Instance.startGameButton.SetActive(true);

		//////////HandObjectControl_BasicMode.Instance.ResetGame();
	}

	void ResultUserList(JSONArray userList)
	{
		//////////HandObjectControl_BasicMode.Instance.UpdateResult(UserData.ParseUserList(userList));
	}

	void UpdateUserList(JSONArray userList)
	{
		//////////HandObjectControl_BasicMode.Instance.UpdateUserList(UserData.ParseUserList(userList));
	}

	public void StartGame()
	{
		JSONObject _data = new JSONObject();
		_data.Add("command", "startGame");
		SocketIOManager.Instance.SendData("startGame", _data);
	}

	public bool SelectHand(HandType handType)
    {
		if (!IsPossibleSelect)
			return false;

		JSONObject _data = new JSONObject();
		_data.Add("hand", handType.ToString());

		SocketIOManager.Instance.SendData("setHand", _data);
		//////////HandObjectControl_BasicMode.Instance.SelectMyHand(handType);
		return true;
	}
}
