using BestHTTP;
using BestHTTP.PlatformSupport;
using BestHTTP.SocketIO3;
using BestHTTP.SocketIO3.Events;
using System;
using UnityEngine;
using Boomlagoon.JSON;
using ParkerLibrary.EventSystem;

public class SocketControl_ManyPeople : SocketControl
{
    protected override void InitEvent()
    {
        targetSocket.On(SocketIOEventTypes.Connect, OnConnected);
        targetSocket.On(SocketIOEventTypes.Disconnect, OnDisconnected);

        targetSocket.On("connected", () => ReceiveData("connected", null));

        targetSocket.On<string>("userListChange", (data) => ReceiveData("userListChange", data));
        targetSocket.On<string>("userHandChange", (data) => ReceiveData("userHandChange", data));

        targetSocket.On<string>("startGame", (data) => ReceiveData("startGame", data));
        targetSocket.On<string>("endGame", (data) => ReceiveData("endGame", data));
        targetSocket.On<string>("startRound", (data) => ReceiveData("startRound", data));
        targetSocket.On<string>("endRound", (data) => ReceiveData("endRound", data));
        targetSocket.On<string>("resetGame", (data) => ReceiveData("resetGame", data));
        targetSocket.On<string>("resetRound", (data) => ReceiveData("resetRound", data));

        targetSocket.On<string>("msg", (data) => ReceiveData("msg", data));
        targetSocket.On<string>("adminMsg", (data) => ReceiveData("adminMsg", data));

        targetSocket.On("error", () => ReceiveData("error"));
    }

    protected override void ReceiveData(string eventName)
    {
        if (string.IsNullOrEmpty(eventName))
            return;

        Debug.Log("[Event] " + eventName);

        try
        {
            switch (eventName)
            {
                case "error": EventManager.Instance.PostNotification(EVENT_TYPE.MANY_PEOPLE_MODE_CONNECT_ERROR, this, null); break;
            }
        }
        catch (Exception e)
        {
            Debug.Log("[Error] " + e);
        }
    }

    protected override void ReceiveData(string eventName, string data)
    {
        if (string.IsNullOrEmpty(eventName))
            return;

        Debug.Log("[Event] " + eventName + " " + data);

        try
        {
            JSONObject _json = (data == null) ? null : JSONObject.Parse(data);
            switch (eventName)
            {
                case "connected": EventManager.Instance.PostNotification(EVENT_TYPE.WS_CONNECTED, this, _json); break;

                case "userListChange": EventManager.Instance.PostNotification(EVENT_TYPE.MANY_PEOPLE_MODE_USER_LIST_CHANGE, this, _json); break;
                case "userHandChange": EventManager.Instance.PostNotification(EVENT_TYPE.MANY_PEOPLE_MODE_USER_HAND_CHANGE, this, _json); break;

                case "startGame": EventManager.Instance.PostNotification(EVENT_TYPE.MANY_PEOPLE_MODE_START_GAME, this, _json); break;
                case "startRound": EventManager.Instance.PostNotification(EVENT_TYPE.MANY_PEOPLE_MODE_START_ROUND, this, _json); break;
                case "endGame": EventManager.Instance.PostNotification(EVENT_TYPE.MANY_PEOPLE_MODE_END_GAME, this, _json); break;
                case "endRound": EventManager.Instance.PostNotification(EVENT_TYPE.MANY_PEOPLE_MODE_END_ROUND, this, _json); break;
                case "resetGame": EventManager.Instance.PostNotification(EVENT_TYPE.MANY_PEOPLE_MODE_RESET_GAME, this, _json); break;
                case "resetRound": EventManager.Instance.PostNotification(EVENT_TYPE.MANY_PEOPLE_MODE_RESET_ROUND, this, _json); break;
                case "msg": EventManager.Instance.PostNotification(EVENT_TYPE.MANY_PEOPLE_MODE_USER_SPEAK, this, _json); break;
                case "adminMsg": EventManager.Instance.PostNotification(EVENT_TYPE.MANY_PEOPLE_MODE_FRONT_HAND_SPEAK, this, _json); break;
            }
        }
        catch (Exception e)
        {
            Debug.Log("[Error] " + e);
        }
    }
}