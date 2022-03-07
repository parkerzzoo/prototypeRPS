using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HandObjectControl_Game1 : HandObjectControl
{
    [SerializeField] private HandObject_Game1[] handObjectList;
    [SerializeField] private HandObject_Game1 myHandObject;

    public HandObject_Game1 MyHandObject { get { return myHandObject; } }

    public void OnUserListChange(List<UserData> userList)
    {
        var _sortedList = SortUserList(userList);
        for (int i = 0; i < handObjectList.Length; i++)
            handObjectList[i].OnUserListChange(_sortedList[i]);
    }

    public void OnStartGame(List<UserData> userList)
    {
        var _sortedList = SortUserList(userList);
        for (int i = 0; i < handObjectList.Length; i++)
            handObjectList[i].OnStartGame(_sortedList[i]);
    }

    public void OnStartRound(List<UserData> userList, float duration)
    {
        var _sortedList = SortUserList(userList);
        for (int i = 0; i < handObjectList.Length; i++)
            handObjectList[i].OnStartRound(_sortedList[i], duration);
    }

    public void OnEndRound(List<UserData> userList, HandType frontHandType, float duration)
    {
        var _sortedList = SortUserList(userList);
        for (int i = 0; i < handObjectList.Length; i++)
            handObjectList[i].OnEndRound(_sortedList[i]);
    }

    public void OnEndGame(List<UserData> userList)
    {
        var _sortedList = SortUserList(userList);
        for (int i = 0; i < handObjectList.Length; i++)
            handObjectList[i].OnEndGame(_sortedList[i]);
    }

    public void OnResetRound(List<UserData> userList)
    {
    }

    public void OnResetGame(List<UserData> userList)
    {
        var _sortedList = SortUserList(userList);
        for (int i = 0; i < handObjectList.Length; i++)
            handObjectList[i].OnResetGame(_sortedList[i]);
    }

    public void OnFrontHandSpeak(List<UserData> userList)
    {
        var _sortedList = SortUserList(userList);
        for (int i = 0; i < handObjectList.Length; i++)
            handObjectList[i].OnFrontHandSpeak(_sortedList[i]);
    }

    public void OnUserHandChange(List<UserData> userList)
    {
        var _sortedList = SortUserList(userList);
        for (int i = 0; i < handObjectList.Length; i++)
            handObjectList[i].OnUserHandChange(_sortedList[i]);
    }

    // 유저리스트 소팅. 자기자신은 0, 이미 존재하는유저는 인덱스 유지.
    List<UserData> SortUserList(List<UserData> userDatas)
    {
        List<UserData> _sortDatas = new List<UserData>();
        int _handObjectCount = handObjectList.Length;

        Dictionary<int, UserData> _userIndex = new Dictionary<int, UserData>();

        // 자신의 데이터는 제일 앞으로.
        for (int i = 0; i < userDatas.Count; i++)
        {
            if (!userDatas[i].IsMe)
                continue;

            _userIndex.Add(0, userDatas[i]);
            userDatas.RemoveAt(i);
            break;
        }

        // 이미 있는 손들은 인덱스 그대로 셋팅.
        for (int i = 1; i < _handObjectCount; i++)
        {
            if (handObjectList[i].userData == null || handObjectList[i].userData.IsMe)
                continue;

            int _index = UserData.IndexOf(userDatas, handObjectList[i].userData);
            if (_index >= 0 && /*_index < handList.Length && */!_userIndex.ContainsKey(i))
            {
                _userIndex.Add(i, userDatas[_index]);
                userDatas.RemoveAt(_index);
            }
        }

        // 나머지는 차례대로 push.
        for (int i = 1; i < _handObjectCount; i++)
        {
            if (userDatas.Count <= 0)
                break;
            if (_userIndex.ContainsKey(i))
                continue;

            _userIndex.Add(i, userDatas[0]);
            userDatas.RemoveAt(0);
        }

        // 결과값 셋팅.
        for(int i = 0; i < _handObjectCount; i++)
        {
            bool _existIndex = _userIndex.ContainsKey(i);
            _sortDatas.Add(_existIndex? _userIndex[i]: null);
        }

        return _sortDatas;
    }
}
