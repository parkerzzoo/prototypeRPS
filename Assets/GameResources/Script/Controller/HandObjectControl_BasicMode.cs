using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandObjectControl_BasicMode : HandObjectControl
{
    // index0 은 본인.
    [SerializeField] private Hand[] handList;
    private Hand MyHand { get { return handList.Length < 0 ? null : handList[0]; } }

    public void UpdateUserList(List<UserData> userList)
    {
        var _sortedList = SortUserList(userList);
        for (int i = 0; i < handList.Length; i++)
            handList[i].Init(_sortedList[i]);
    }

    public void UpdateResult(List<UserData> userList)
    {
        var _sortedList = SortUserList(userList);
        for (int i = 0; i < handList.Length; i++)
            handList[i].SetHand(_sortedList[i]);
    }

    public void StartGame(int normalRountCount)
    {
        for (int i = 0; i < handList.Length; i++)
            if (handList[i].userData != null)
                handList[i].StartNormalRound(normalRountCount);
    }

    public void EndNormalRound(List<UserData> userList)
    {
        var _sortedList = SortUserList(userList);
        for (int i = 0; i < handList.Length; i++)
            handList[i].EndNormalRound(_sortedList[i]);
    }

    public void EndDeathMatchRound(List<UserData> userList)
    {
        var _sortedList = SortUserList(userList);
        for (int i = 0; i < handList.Length; i++)
            handList[i].EndDeathMatchRound(_sortedList[i]);
    }

    public void EndGame()
    {
        for (int i = 0; i < handList.Length; i++)
            handList[i].EndGame();
    }

    public void ResetGame()
    {
        AllReset();
        for (int i = 0; i < handList.Length; i++)
            handList[i].ResetGame();
    }

    public void SelectMyHand(HandType handType)
    {
        if (MyHand == null)
            return;

        MyHand.SetHand(handType);
    }

    public void AllReset()
    {
        for (int i = 0; i < handList.Length; i++)
        {
            handList[i].PlayRandom();
            handList[i].SetHand(HandType.empty);
        }
    }

    // 유저리스트 소팅. 자기자신은 0, 이미 존재하는유저는 인덱스 유지.
    List<UserData> SortUserList(List<UserData> userDatas)
    {
        List<UserData> _sortDatas = new List<UserData>();

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
        for (int i = 1; i < handList.Length; i++)
        {
            if (handList[i].userData == null || handList[i].userData.IsMe)
                continue;

            int _index = UserData.IndexOf(userDatas, handList[i].userData);
            if (_index >= 0 && /*_index < handList.Length && */!_userIndex.ContainsKey(i))
            {
                _userIndex.Add(i, userDatas[_index]);
                userDatas.RemoveAt(_index);
            }
        }

        // 나머지는 차례대로 push.
        for (int i = 1; i < handList.Length; i++)
        {
            if (userDatas.Count <= 0)
                break;
            if (_userIndex.ContainsKey(i))
                continue;

            _userIndex.Add(i, userDatas[0]);
            userDatas.RemoveAt(0);
        }

        // 결과값 셋팅.
        for(int i = 0; i < handList.Length; i++)
        {
            bool _existIndex = _userIndex.ContainsKey(i);
            _sortDatas.Add(_existIndex? _userIndex[i]: null);
        }

        return _sortDatas;
    }
}
