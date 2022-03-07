using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ParkerLibrary.EventSystem;
using Boomlagoon.JSON;
using DG.Tweening;

public class FlowControl_Game2 : FlowControl
{
	[SerializeField] private TMPro.TMP_InputField nameInputField;

	protected override void OnConnected()
	{
		JSONObject _data = new JSONObject();
		_data.Add("userId", VarList.userId);
		_data.Add("point", 100);
		_data.Add("nickname", nameInputField.text);

		SocketControl_Game2.Instance.SendData("identity", _data);
		SocketControl_Game2.Instance.SendData("userListChange");

		GameController.Instance.UIControl<UIControl_Game2>().FadeOutCoverImage();
	}

	protected override void OnUserListChange(JSONObject data)
	{
		JSONArray _userDatas = data.GetArray("users");
		List<UserData> _userList = UserData.ParseUserList(_userDatas);

		GameController.Instance.HandObjectControl<HandObjectControl_Game2>().OnUserListChange(_userList);
	}

	protected override void OnStartGame(JSONObject data)
	{
		float _duration = (float)data.GetNumber("delay");
		if (_duration > 10)
			_duration = 10f;

		//GameController.Instance.UIControl<UIControl_Game2>().ShowCenterTextPanel("게임 시작", 0f, _duration);

		JSONArray _userDatas = data.GetArray("users");
		List<UserData> _userList = UserData.ParseUserList(_userDatas);

		GameController.Instance.HandObjectControl<HandObjectControl_Game2>().OnStartGame(_userList);
	}

	protected override void OnStartRound(JSONObject data)
	{
		float _duration = (float)data.GetNumber("delay");
		if (_duration > 10)
			_duration = 10f;

		JSONArray _userDatas = data.GetArray("users");
		List<UserData> _userList = UserData.ParseUserList(_userDatas);

		for (int i = 0; i < _userList.Count; i++)
		{
			if (_userList[i].IsMe && _userList[i].isAlive)
			{
				GameController.Instance.UIControl<UIControl_Game2>().ShowSubTextPanel("손을 선택해주세요!", 0f, _duration);
				//handSelectable = _userList[i].possiblePlay;
				break;
			}
		}

		StartCoroutine(OnStartRoundCor(_userList, _duration));
		//GameController.Instance.UIControl<UIControl_Game2>().ShowCenterTimerPanel(_duration);
		//GameController.Instance.HandObjectControl<HandObjectControl_Game2>().OnStartRound(_userList, _duration);
	}

	IEnumerator OnStartRoundCor(List<UserData> userList, float duration)
    {
		float _readyPlayTime = 1.5f;
		if(duration > _readyPlayTime)
        {
			yield return new WaitForSeconds(duration - _readyPlayTime + 0.22f);
			GameController.Instance.HandObjectControl<HandObjectControl_Game2>().OnStartRound(userList, _readyPlayTime);
		}
		else
        {
			yield return new WaitForSeconds(0.18f);
			GameController.Instance.HandObjectControl<HandObjectControl_Game2>().OnStartRound(userList, duration);
		}
	}

	protected override void OnEndRound(JSONObject data)
	{
		CameraShaking.Instance.OnEndRound();
		JSONArray _userDatas = data.GetArray("users");
		List<UserData> _userList = UserData.ParseUserList(_userDatas);

		float _duration = (float)data.GetNumber("delay");
		if (_duration > 10)
			_duration = 10f;

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

		GameController.Instance.HandObjectControl<HandObjectControl_Game2>().OnEndRound(_userList, _frontHand, _duration);
	}

	protected override void OnEndGame(JSONObject data)
	{
		JSONArray _userDatas = data.GetArray("users");
		List<UserData> _userList = UserData.ParseUserList(_userDatas);
		for (int i = 0; i < _userList.Count; i++)
		{
			if (_userList[i].IsMe)
			{
				GameController.Instance.UIControl<UIControl_Game2>().UpdateChipCountText(_userList[i].point);
				break;
			}
		}

		TeamInfo _myTeam = GameController.Instance.HandObjectControl<HandObjectControl_Game2>().MyHandObject.userData.teamInfo;
		TeamInfo _winnerTeam = TeamInfo.A;
		for (int i = 0; i < _userList.Count; i++)
		{
			if (_userList[i].isAlive)
			{
				_winnerTeam = _userList[i].teamInfo;
				break;
			}
		}

		GameController.Instance.UIControl<UIControl_Game2>().ShowCenterTextPanel(_myTeam == _winnerTeam? "WIN": "LOSE", 0f, 3f);

		/*string _winnerName = null;
		for (int i = 0; i < _userList.Count; i++)
		{
			if (_userList[i].isAlive)
			{
				_winnerName = _userList[i].nickname;
				break;
			}
		}
		
		if (string.IsNullOrEmpty(_winnerName))
			_winnerName = "없음";*/
		//GameController.Instance.UIControl<UIControl_Game2>().ShowCenterTextPanel("우승자: " + _winnerName, 0f, 3f);

		GameController.Instance.HandObjectControl<HandObjectControl_Game2>().OnEndGame(_userList);
	}

	protected override void OnResetRound(JSONObject data)
	{
		JSONArray _userDatas = data.GetArray("users");
		List<UserData> _userList = UserData.ParseUserList(_userDatas);

		GameController.Instance.HandObjectControl<HandObjectControl_Game2>().OnResetRound(_userList);
	}

	protected override void OnResetGame(JSONObject data)
	{
		JSONArray _userDatas = data.GetArray("users");
		List<UserData> _userList = UserData.ParseUserList(_userDatas);

		GameController.Instance.HandObjectControl<HandObjectControl_Game2>().OnResetGame(_userList);
	}

	protected override void OnUserSpeak(JSONObject data)
	{
		string _msg = data.GetString("msg");

		GameController.Instance.UIControl<UIControl_Game2>().CreatChatObject(_msg);
	}

	protected override void OnUserHandChange(JSONObject data)
	{
		JSONArray _userDatas = data.GetArray("users");
		List<UserData> _userList = UserData.ParseUserList(_userDatas);

		GameController.Instance.HandObjectControl<HandObjectControl_Game2>().OnUserHandChange(_userList);
	}

	protected override void OnConnectError()
    {
		GameController.Instance.UIControl<UIControl_Game2>().ActiveConnectErrorPanel();
	}


    #region PublicMethod

    public void OnClickStart()
	{
		GameController.Instance.SocketControl().ConnectSocket();
	}

	public void OnSendChat(string content)
    {
		if (string.IsNullOrEmpty(content))
			return;

		JSONObject _data = new JSONObject();
		_data.Add("msg", content);

		SocketControl_Game2.Instance.SendData("msg", _data);
	}

	public void OnSelectRock()
    {
		SelectHand(HandType.rock);
	}

	public void OnSelectPaper()
	{
		SelectHand(HandType.paper);
	}

	public void OnSelectScissors()
	{
		SelectHand(HandType.scissors);
	}

	void SelectHand(HandType handType)
	{
		JSONObject _data = new JSONObject();
		_data.Add("hand", handType.ToString());

		SocketControl_Game2.Instance.SendData("setHand", _data);
	}

    #endregion
}
