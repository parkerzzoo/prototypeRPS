using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParkerLibrary.EventSystem;
using Boomlagoon.JSON;

public class WebSocketManager : Singletone<WebSocketManager>
{
	void Start()
	{
		EventManager.Instance.AddListener(EVENT_TYPE.WS_RECEIVE_DATA, OnEvent);
	}

	void OnDestroy()
	{
		EventManager.Instance.RemoveListener(EVENT_TYPE.WS_RECEIVE_DATA, OnEvent);
	}

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
		try
		{
			JSONObject _json = JSONObject.Parse(data);
			if (_json == null || !_json.ContainsKey("command"))
            {
				Debug.Log("not exist command");
				return;
			}

			string _command = _json.GetString("command");
			switch (_command)
			{
				case "userListChange": EventManager.Instance.PostNotification(EVENT_TYPE.WS_RECEIVE_DATA, this, _json); break;
				case "startGame": EventManager.Instance.PostNotification(EVENT_TYPE.WS_RECEIVE_DATA, this, _json); break;
				case "returnResult": EventManager.Instance.PostNotification(EVENT_TYPE.WS_RECEIVE_DATA, this, _json); break;
				case "resetGame": EventManager.Instance.PostNotification(EVENT_TYPE.WS_RECEIVE_DATA, this, _json); break;
			}
		}
		catch
		{
			Debug.Log("ParsingError: " + data);
		}
	}
}
