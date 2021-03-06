using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HandObjectControl_Game2 : HandObjectControl
{
    [SerializeField] private int playerCount;
    [SerializeField] private int teamCount;

    [SerializeField] private HandObject_Game2[] handObjectList;
    [SerializeField] private HandObject_Game2 myHandObject;

    public HandObject_Game2 MyHandObject { get { return myHandObject; } }

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

    // ??????????????? ??????. ??????????????? 0, ?????? ????????????????????? ????????? ??????.
    List<UserData> SortUserList(List<UserData> userDatas)
    {
        List<UserData> _sortDatas = new List<UserData>();
        int _handObjectCount = handObjectList.Length;

        Dictionary<int, UserData> _userIndex = new Dictionary<int, UserData>();

        // ????????? ???????????? ?????? ?????????.
        for (int i = 0; i < userDatas.Count; i++)
        {
            if (!userDatas[i].IsMe)
                continue;

            _userIndex.Add(0, userDatas[i]);
            userDatas.RemoveAt(i);
            break;
        }

        // ?????? ?????? ????????? ????????? ????????? ??????.
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

        // ?????? ?????? ?????? ????????? ?????? push.
        TeamInfo _myTeam = _userIndex[0].teamInfo;
        for (int i = 1; i < _handObjectCount; i++)
        {
            if (userDatas.Count <= 0)
                break;
            bool _sameMyTeam = userDatas[0].teamInfo == _myTeam;
            if (_userIndex.ContainsKey(i) || !_sameMyTeam)
                continue;

            _userIndex.Add(i, userDatas[0]);
            userDatas.RemoveAt(0);
        }

        // ???????????? ???????????? push.
        for (int i = 1; i < _handObjectCount; i++)
        {
            if (userDatas.Count <= 0)
                break;
            if (_userIndex.ContainsKey(i))
                continue;

            bool _isSameTeam = userDatas[0].teamInfo == _myTeam;

            if(_isSameTeam)

            _userIndex.Add(i, userDatas[0]);
            userDatas.RemoveAt(0);
        }

        // ????????? ??????.
        for(int i = 0; i < _handObjectCount; i++)
        {
            bool _existIndex = _userIndex.ContainsKey(i);
            _sortDatas.Add(_existIndex? _userIndex[i]: null);
        }

        return _sortDatas;
    }
}
