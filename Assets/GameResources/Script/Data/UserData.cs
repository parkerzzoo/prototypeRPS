using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boomlagoon.JSON;

public class UserData
{
    public string userId;
    public int index;
    public HandType handType;
    public bool possiblePlay;
    public bool IsMe { get { return VarList.userId == userId; } }
    public ResultType currentResult;
    public TeamInfo teamInfo;
    public int point = 0;

    public string nickname;
    public bool isAlive;

    public bool SetAlive(bool isAlive)
    {
        bool isChanged = isAlive != this.isAlive;
        this.isAlive = isAlive;
        return isChanged;
    }

    public UserData(string userId)
    {
        this.userId = userId;
        index = -1;
    }

    public UserData(JSONObject data)
    {
        userId = data.GetString("userId");
        string _hand = !data.ContainsKey("hand") ? null : data.GetString("hand");
        if (string.IsNullOrEmpty(_hand))
        {
            handType = HandType.empty;
        }
        else
        {
            switch(_hand)
            {
                case "rock": handType = HandType.rock; break;
                case "paper": handType = HandType.paper; break;
                case "scissors": handType = HandType.scissors; break;
            }
        }
        
        possiblePlay = data.ContainsKey("possiblePlayer")? data.GetBoolean("possiblePlayer"): true;

        point = data.ContainsKey("point")? (int)data.GetNumber("point"): 0;

        index = -1;

        nickname = data.ContainsKey("nickname") ? data.GetString("nickname") : "";
        isAlive = data.ContainsKey("isAlive") ? data.GetBoolean("isAlive") : false;

        if(data.ContainsKey("currentResult"))
        {
            string _result = data.GetString("currentResult");
            switch (_result)
            {
                case "win": currentResult = ResultType.win; break;
                case "lose": currentResult = ResultType.lose; break;
                case "draw": currentResult = ResultType.draw; break;
            }
        }

        if (data.ContainsKey("teamInfo"))
        {
            string _result = data.GetString("teamInfo");
            switch (_result)
            {
                case "A": teamInfo = TeamInfo.A; break;
                case "B": teamInfo = TeamInfo.B; break;
            }
        }
    }

    public void SetHandType(HandType handType)
    {
        this.handType = handType;
    }

    public void SetIndex(int index)
    {
        this.index = index;
    }

    // JSONArray ??????.
    public static List<UserData> ParseUserList(JSONArray data)
    {
        List<UserData> _userDatas = new List<UserData>();

        for (int i = 0; i < data.Length; i++)
        {
            UserData _data = new UserData(data[i].Obj);
            _userDatas.Add(_data);
        }

        return _userDatas;
    }

    public static bool ExistUser(List<UserData> userList, UserData user)
    {
        return IndexOf(userList, user) >= 0;
    }

    public static int IndexOf(List<UserData> userList, UserData user)
    {
        for (int i = 0; i < userList.Count; i++)
            if (userList[i].userId == user.userId)
                return i;
        return -1;
    }
}
