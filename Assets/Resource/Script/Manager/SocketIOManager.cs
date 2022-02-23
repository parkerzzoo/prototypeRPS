using BestHTTP;
using BestHTTP.PlatformSupport;
using BestHTTP.SocketIO3;
using BestHTTP.SocketIO3.Events;
using System;
using UnityEngine;
using Boomlagoon.JSON;
using ParkerLibrary.EventSystem;

public class SocketIOManager : Singletone<SocketIOManager>
{
    [SerializeField] private string address = "";
    private SocketManager socketManager = null;

    private void Start()
    {
        // ConnectSocketIO();
    }

    public void ConnectWebSocket()
    {
        ConnectSocketIO();
    }

    private void ConnectSocketIO()
    {
        SocketOptions options = new SocketOptions();
        options.AutoConnect = false;

        socketManager = new SocketManager(new Uri(address));// "http://192.168.20.13:3000"));
        socketManager.Open();

        socketManager.Socket.On(SocketIOEventTypes.Connect, OnConnected);
        socketManager.Socket.On(SocketIOEventTypes.Disconnect, OnDisconnected);

        socketManager.Socket.On("connected", () => ReceiveData("connected", null));
        socketManager.Socket.On<string>("userListChange", (data) => ReceiveData("userListChange", data));
        socketManager.Socket.On<string>("startGame", (data) => ReceiveData("startGame", data));
        socketManager.Socket.On<string>("returnResult", (data) => ReceiveData("returnResult", data));
        socketManager.Socket.On("resetGame", () => ReceiveData("resetGame", null));

        socketManager.Socket.On<string>("startRound", (data) => ReceiveData("startRound", data));
        socketManager.Socket.On<string>("endRound", (data) => ReceiveData("endRound", data));
        socketManager.Socket.On<string>("endGame", (data) => ReceiveData("endGame", data));
    }

    private void OnConnected()
    {
        Debug.Log("[Socket.IO] Connected!");
    }

    private void OnDisconnected()
    {
        Debug.Log("[Socket.IO] Disconnected!");
    }

    void ReceiveData(string eventName, string data)
    {
        if (string.IsNullOrEmpty(eventName))
            return;

        Debug.Log("[Event] " + eventName + " " + data);

        try
        {
            JSONObject _json = (data == null)? null: JSONObject.Parse(data);
            switch (eventName)
            {
                case "connected": EventManager.Instance.PostNotification(EVENT_TYPE.WS_CONNECTED, this, _json); break;

                case "userListChange": EventManager.Instance.PostNotification(EVENT_TYPE.BASIC_MODE_USER_LIST_UPDATE, this, _json); break;
                case "startGame": EventManager.Instance.PostNotification(EVENT_TYPE.BASIC_MODE_START_GAME, this, _json); break;
                case "returnResult": EventManager.Instance.PostNotification(EVENT_TYPE.BASIC_MODE_RETURN_RESULT, this, _json); break;
                case "resetGame": EventManager.Instance.PostNotification(EVENT_TYPE.BASIC_MODE_RESET_GAME, this, _json); break;

                case "startRound": EventManager.Instance.PostNotification(EVENT_TYPE.BASIC_MODE_START_ROUND, this, _json); break;
                case "endRound": EventManager.Instance.PostNotification(EVENT_TYPE.BASIC_MODE_END_ROUND, this, _json); break;
                case "endGame": EventManager.Instance.PostNotification(EVENT_TYPE.BASIC_MODE_END_GAME, this, _json); break;
            }
        }
        catch(Exception e)
        {
            Debug.Log("[Error] " + e);
        }
    }

    public void SendData(string eventName)
    {
        socketManager.Socket.Emit(eventName);
    }

    public void SendData(string eventName, JSONObject data)
    {
        socketManager.Socket.Emit(eventName, data.ToString());
    }

    

    private void Destory()
    {
        if (socketManager != null)
        {
            socketManager.Close();
            socketManager = null;
        }
    }
}