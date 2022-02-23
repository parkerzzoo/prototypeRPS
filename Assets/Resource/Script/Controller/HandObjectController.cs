using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandObjectController : Singletone<HandObjectController>
{
    public GameObject handPrefab;
    public List<HandObject> handList = new List<HandObject>();
    HandObject myHand;

    public Transform centerPos;
    public Transform myPos;

    public void UpdateUserList(List<UserData> userList)
    {
        Debug.Log("user count is " + userList.Count);
        RemoveAllHand();
        Vector3 _targetVec = myPos.position - centerPos.position;
        int _userCount = userList.Count;
        float _gapAngle = 360f / _userCount;
        if (_userCount == 0)
            return;
        for (int i = 0; i < userList.Count; i++)
        {
            GameObject _handObject = Instantiate(handPrefab, transform);
            HandObject _hand = _handObject.GetComponent<HandObject>();
            _hand.Init(userList[i]);
            handList.Add(_hand);
            if (i == 0) myHand = _hand;

            _handObject.transform.position = centerPos.position + _targetVec;
            Quaternion _v3Rotation = Quaternion.Euler(0f, 0f, _gapAngle);  // 회전각
            _targetVec = _v3Rotation * _targetVec;
        }
    }

    void RemoveAllHand()
    {
        int _userCount = handList.Count;
        for (int i = 0; i < _userCount; i++)
            Destroy(handList[i].gameObject);
        handList.Clear();
    }

    public void SelectMyHand(HandType handType)
    {
        if (myHand == null)
            return;

        myHand.SetHand(handType);
    }

    public void AllReset()
    {
        for (int i = 0; i < handList.Count; i++)
        {
            handList[i].PlayRandom();
            handList[i].InitResult();
            handList[i].SetHand(HandType.empty);
        }
    }

    public void SelectAllHand(List<UserData> userList)
    {
        for(int i = 0; i < userList.Count; i++)
        {
            var _user = userList[i];
            if (_user.userId == VarList.userId)
                continue;

            foreach (var j in handList)
            {
                if(_user.userId == j.userData.userId)
                {
                    Debug.Log(j.userData.userId + " show " + _user.handType);
                    j.SetHand(_user.handType);
                    break;
                }
            }   
        }

        ShowResult();
    }

    public void ShowResult()
    {
        Dictionary<HandType, bool> _existHand = new Dictionary<HandType, bool>();
        _existHand.Add(HandType.rock, false);
        _existHand.Add(HandType.paper, false);
        _existHand.Add(HandType.scissors, false);

        foreach (var i in handList)
        {
            if (i.userData.handType != HandType.empty && !_existHand[i.userData.handType])
                _existHand[i.userData.handType] = true;
        }

        bool _allExist = true;
        foreach (var i in _existHand)
            _allExist = _allExist && i.Value;

        bool _allSame = false;
        if (_existHand[HandType.rock] && !_existHand[HandType.paper] && !_existHand[HandType.scissors])
            _allSame = true;
        else if (!_existHand[HandType.rock] && _existHand[HandType.paper] && !_existHand[HandType.scissors])
            _allSame = true;
        else if(!_existHand[HandType.rock] && !_existHand[HandType.paper] && _existHand[HandType.scissors])
            _allSame = true;
        else if(!_existHand[HandType.rock] && !_existHand[HandType.paper] && !_existHand[HandType.scissors])
            _allSame = true;

        Debug.Log(_allExist + " " + _allSame);

        if (_allExist || _allSame)
        {
            foreach (var i in handList)
            {
                if (i.userData.handType == HandType.empty)
                    i.SetResult(ResultType.lose);
                else
                    i.SetResult(ResultType.draw);
            }
        }
        else
        {
            HandType _winType = HandType.rock;
            HandType _loseType = HandType.rock;
            if(!_existHand[HandType.rock])
            {
                _winType = HandType.scissors;
                _loseType = HandType.paper;
            }
            else if (!_existHand[HandType.paper])
            {
                _winType = HandType.rock;
                _loseType = HandType.scissors;
            }
            else if(!_existHand[HandType.scissors])
            {
                _winType = HandType.paper;
                _loseType = HandType.rock;
            }
            foreach (var i in handList)
            {
                if (i.userData.handType == HandType.empty)
                    i.SetResult(ResultType.lose);
                else if(i.userData.handType == _winType)
                    i.SetResult(ResultType.win);
                else if (i.userData.handType == _loseType)
                    i.SetResult(ResultType.lose);
            }
        }
    }
}
