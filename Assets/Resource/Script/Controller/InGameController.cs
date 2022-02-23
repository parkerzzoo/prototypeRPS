using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ParkerLibrary.EventSystem;
using Boomlagoon.JSON;
using DG.Tweening;

public class InGameController : Singletone<InGameController>
{
	public Image rockButtonBack;
	public Image paperButtonBack;
	public Image scissorsButtonBack;
	public Sprite activeButtonBack;
	public Sprite inactiveButtonBack;

	public GameObject[] objectsInPossibleSelect;
	public GameObject startGameButton;
	public GameObject startGamePanel;
	public TMPro.TextMeshProUGUI startGameText;
	public TMPro.TextMeshProUGUI userCountText;
	public TMPro.TextMeshProUGUI myUidText;

	bool isPossibleSelect = false;

	public void OnEvent(EVENT_TYPE eventType, Component sender, object param = null)
	{
		switch (eventType)
		{
			case EVENT_TYPE.WS_RECEIVE_DATA:
				OnReceiveData((string)param);
				break;
		}
	}

	void OnReceiveData(string data)
	{
		Debug.Log("OnReceiveData " + data);
		try
		{
			var _data = JSONObject.Parse(data);
			if (!_data.ContainsKey("command"))
				return;

			string _command = _data.GetString("command");

			switch (_command)
			{
				case "userListChange":
					{
						JSONArray _userDatas = _data.GetArray("data");
						UpdateUserList(_userDatas);
					}
					break;
				case "startGame":
					{
						float _startDelay = (float)_data.GetNumber("startDelay");
						StartGame(_startDelay);
					}
					break;
				case "returnResult":
					{
						JSONArray _userDatas = _data.GetArray("data");
						ResultUserList(_userDatas);
					}
					break;
				case "resetGame":
					{
						ResetGame();
					}
					break;
			}
		}
		catch
		{
			return;
		}
	}

	void StartGame(float selectionTime)
	{
		StartCoroutine(StartGameCor(selectionTime));
	}

	IEnumerator StartGameCor(float selectionTime)
	{
		InactiveAllButtons();
		isPossibleSelect = true;
		startGameButton.SetActive(false);
		ControlActiveObjectInPossibleSelect(true);
		startGamePanel.SetActive(true);
		startGamePanel.transform.DOScaleY(1f, 0.1f).SetEase(Ease.OutBack);

		startGameText.transform.DOPunchScale(Vector3.one * 0.2f, 0.1f, 1);
		startGameText.text = "가위!";
		yield return new WaitForSeconds(selectionTime / 2.5f);

		startGameText.transform.DOPunchScale(Vector3.one * 0.2f, 0.1f, 1);
		startGameText.text = "바위!!";
		yield return new WaitForSeconds(selectionTime / 2.5f);

		startGameText.transform.DOPunchScale(Vector3.one * 0.2f, 0.1f, 1);
		startGameText.text = "보!!!";
		yield return new WaitForSeconds(0.5f);

		isPossibleSelect = false;

		startGamePanel.transform.DOScaleY(0f, 0.05f).SetEase(Ease.InBack);

	}

	void ResetGame()
	{
		isPossibleSelect = true;
		startGameButton.SetActive(true);
		ControlActiveObjectInPossibleSelect(false);
		HandObjectController.Instance.AllReset();
	}

	void ResultUserList(JSONArray userList)
	{
		List<UserData> _userDatas = new List<UserData>();

		for (int i = 0; i < userList.Length; i++)
		{
			UserData _data = new UserData(userList[i].Obj);
			if (VarList.userId != _data.userId)
				continue;

			_userDatas.Add(_data);
			break;
		}

		for (int i = 0; i < userList.Length; i++)
		{
			var _data = new UserData(userList[i].Obj);
			if (VarList.userId == _data.userId)
				continue;

			_userDatas.Add(_data);
		}
		HandObjectController.Instance.SelectAllHand(_userDatas);
	}

	void UpdateUserList(JSONArray userList)
	{
		Debug.Log("uid: " + VarList.userId);
		List<UserData> _userDatas = new List<UserData>();

		for (int i = 0; i < userList.Length; i++)
		{
			UserData _data = new UserData(userList[i].Obj);
			if (VarList.userId != _data.userId)
				continue;

			Debug.Log(_data.userId + " is set in index" + i);
			_userDatas.Add(_data);
			break;
		}

		for (int i = 0; i < userList.Length; i++)
		{
			var _data = new UserData(userList[i].Obj);
			if (VarList.userId == _data.userId)
				continue;

			_userDatas.Add(_data);
		}
		HandObjectController.Instance.UpdateUserList(_userDatas);
		UpdateUserCountText(_userDatas.Count);
	}

	void UpdateUserCountText(int count)
	{
		userCountText.text = "참여 인원: " + count;
	}

	void UpdateMyUidText()
	{
		myUidText.text = "내 아이디: " + VarList.userId;
	}

	protected override void Awake()
	{
		base.Awake();
		EventManager.Instance.AddListener(EVENT_TYPE.WS_RECEIVE_DATA, OnEvent);
		ControlActiveObjectInPossibleSelect(false);
	}

	IEnumerator Start()
	{
		UpdateMyUidText();
		yield return new WaitForSeconds(0.5f);
		Enter();

	}


	void OnDestroy()
	{
		EventManager.Instance.RemoveListener(EVENT_TYPE.WS_RECEIVE_DATA, OnEvent);
	}

	public void StartGame()
	{
		Debug.Log("Click StartGame");
		JSONObject _data = new JSONObject();
		_data.Add("command", "startGame");
		//Connection.Instance.SendText(_data.ToString());
	}

	public void SendRock()
	{
		if (!isPossibleSelect)
			return;

		InactiveAllButtons();
		rockButtonBack.sprite = activeButtonBack;

		JSONObject _data = new JSONObject();
		_data.Add("command", "setHand");
		_data.Add("data", HandType.rock.ToString());
		//Connection.Instance.SendText(_data.ToString());
		HandObjectController.Instance.SelectMyHand(HandType.rock);
	}

	public void SendPaper()
	{
		if (!isPossibleSelect)
			return;

		InactiveAllButtons();
		paperButtonBack.sprite = activeButtonBack;

		JSONObject _data = new JSONObject();
		_data.Add("command", "setHand");
		_data.Add("data", HandType.paper.ToString());
		//Connection.Instance.SendText(_data.ToString());
		HandObjectController.Instance.SelectMyHand(HandType.paper);
	}

	public void SendScissors()
	{
		if (!isPossibleSelect)
			return;

		InactiveAllButtons();
		scissorsButtonBack.sprite = activeButtonBack;

		JSONObject _data = new JSONObject();
		_data.Add("command", "setHand");
		_data.Add("data", HandType.scissors.ToString());
		//Connection.Instance.SendText(_data.ToString());
		HandObjectController.Instance.SelectMyHand(HandType.scissors);
	}

	void ControlActiveObjectInPossibleSelect(bool isActive)
	{
		for (int i = 0; i < objectsInPossibleSelect.Length; i++)
			objectsInPossibleSelect[i].SetActive(isActive);
	}

	public void Enter()
	{
		JSONObject _data = new JSONObject();
		_data.Add("command", "userListChange");
		//Connection.Instance.SendText(_data.ToString());
	}

	void InactiveAllButtons()
	{
		rockButtonBack.sprite = inactiveButtonBack;
		paperButtonBack.sprite = inactiveButtonBack;
		scissorsButtonBack.sprite = inactiveButtonBack;
	}
}