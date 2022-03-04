using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HandObjectControl_FewPeople : HandObjectControl
{
    [SerializeField] private Hand_FewPeople[] handObjectList;
    [SerializeField] private Hand_FewPeople myHandObject;
    //[SerializeField] private HandObject_ManyPeople frontHandObject;

    private Dictionary<int, Hand_FewPeople> indexHandPair = new Dictionary<int, Hand_FewPeople>();

    public Hand_FewPeople MyHandObject { get { return myHandObject; } }

    public void OnUserListChange(List<UserData> userList)
    {
        var _sortedList = SortUserList(userList);
        for (int i = 0; i < handObjectList.Length; i++)
            handObjectList[i].OnUserListChange(_sortedList[i]);

        //frontHandObject.PlayRandom();
    }

    public void OnStartGame(List<UserData> userList)
    {
        var _sortedList = SortUserList(userList);
        for (int i = 0; i < handObjectList.Length; i++)
            handObjectList[i].OnStartGame(_sortedList[i]);

        //frontHandObject.PlayRandom();
    }

    public void OnStartRound(List<UserData> userList, float duration)
    {
        var _sortedList = SortUserList(userList);
        for (int i = 0; i < handObjectList.Length; i++)
            handObjectList[i].OnStartRound(_sortedList[i], duration);

        //frontHandObject.PlayRandom();
    }

    public void OnEndRound(List<UserData> userList, HandType frontHandType, float duration)
    {
        //frontHandObject.SetHand(frontHandType);

        var _sortedList = SortUserList(userList);
        for (int i = 0; i < handObjectList.Length; i++)
            handObjectList[i].OnEndRound(_sortedList[i]);

        int _loserCount = 0;
        for (int i = 0; i < _sortedList.Count; i++)
            if (_sortedList[i] != null && !_sortedList[i].isAlive && handObjectList[i].CurState != Hand_FewPeople.HandManyPeopleState.LoseWaiting)
                _loserCount++;

        StartCoroutine(ShowLoserCor(_sortedList, duration, _loserCount));
    }

    IEnumerator ShowLoserCor(List<UserData> userList, float duration, int loserCount)
    {
        float _delay = (duration - 2.5f) / (float)loserCount;

        int _aliver = 0;

        for (int i = 0; i < handObjectList.Length; i++)
        {
            if(userList[i] != null && !userList[i].isAlive && handObjectList[i].CurState != Hand_FewPeople.HandManyPeopleState.LoseWaiting)
            {
                handObjectList[i].ShowLoser(userList[i]);
                yield return new WaitForSeconds(0f);
            }
            else if(userList[i] != null && userList[i].isAlive)
            {
                _aliver++;
            }
        }
        //UIControl_FewPeople.Instance.ActiveFrontHandSpeak("생존자 " + _aliver + "명 입니다.");
    }

    public void OnEndGame(List<UserData> userList)
    {
        var _sortedList = SortUserList(userList);
        for (int i = 0; i < handObjectList.Length; i++)
            handObjectList[i].OnEndGame(_sortedList[i]);

        //frontHandObject.PlayRandom();
    }

    public void OnResetRound(List<UserData> userList)
    {
    }


    public void OnResetGame(List<UserData> userList)
    {
        var _sortedList = SortUserList(userList);
        for (int i = 0; i < handObjectList.Length; i++)
            handObjectList[i].OnResetGame(_sortedList[i]);

        //frontHandObject.PlayRandom();
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

    public void SelectMyHand(HandType handType)
    {
        if (myHandObject == null)
            return;

        //myHandObject.SetHand(handType);
    }

    public void AllReset()
    {
        for (int i = 0; i < handObjectList.Length; i++)
        {
            //handObjectList[i].PlayRandom();
            //handObjectList[i].SetHand(HandType.empty);
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
        for (int i = 1; i < handObjectList.Length; i++)
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
        for (int i = 1; i < handObjectList.Length; i++)
        {
            if (userDatas.Count <= 0)
                break;
            if (_userIndex.ContainsKey(i))
                continue;

            _userIndex.Add(i, userDatas[0]);
            userDatas.RemoveAt(0);
        }

        // 결과값 셋팅.
        for(int i = 0; i < handObjectList.Length; i++)
        {
            bool _existIndex = _userIndex.ContainsKey(i);
            _sortDatas.Add(_existIndex? _userIndex[i]: null);
        }

        return _sortDatas;
    }
}
