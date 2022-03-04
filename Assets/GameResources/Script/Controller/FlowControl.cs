using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boomlagoon.JSON;
using ParkerLibrary.EventSystem;

public class FlowControl : Singletone<FlowControl>
{
	protected virtual void OnEvent(EVENT_TYPE eventType, Component sender, object param = null)
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

	protected virtual void Start()
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

	protected virtual void OnDestroy()
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

	protected virtual void OnConnected(){}

	protected virtual void OnUserListChange(JSONObject data) { }

	protected virtual void OnStartGame(JSONObject data) { }

	protected virtual void OnStartRound(JSONObject data) { }

	protected virtual void OnEndRound(JSONObject data) { }

	protected virtual void OnEndGame(JSONObject data) { }

	protected virtual void OnResetRound(JSONObject data) { }

	protected virtual void OnResetGame(JSONObject data) { }

	protected virtual void OnUserSpeak(JSONObject data) { }

	protected virtual void OnUserHandChange(JSONObject data) { }

	protected virtual void OnConnectError() { }
}
