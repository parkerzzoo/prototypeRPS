using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HandObject_Game3 : MonoBehaviour
{
    public enum HandManyPeopleState { WaitingPlayer, Win, Lose, LoseWaiting, Ready }
    public enum ResultType { Win, Lose, Death, Alive }

    private HandManyPeopleState curState = HandManyPeopleState.WaitingPlayer;
    private ResultType curResult = ResultType.Alive;

    public HandManyPeopleState CurState { get { return curState; } }
    public ResultType CurResult { get { return curResult; } }

    Vector3 contentFirstPos;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject winObject;
    [SerializeField] private GameObject loseObject;
    [SerializeField] private Image handImage;

    [SerializeField] private Sprite rockSprite;
    [SerializeField] private Sprite paperSprite;
    [SerializeField] private Sprite scissorsSprite;

    [SerializeField] private TMPro.TextMeshProUGUI nameText;

    private Coroutine randomCor = null;
    private HandType showHandType = HandType.rock;

    public UserData userData = null;
    public bool Dead { get { return curState == HandManyPeopleState.LoseWaiting; } }
    private bool ExistUser { get { return userData != null; } }

    private void Start()
    {
        if(content != null)
            contentFirstPos = content.transform.position;
    }

    public void OnUserListChange(UserData userData)
    {
        this.userData = userData;

        // 유저가 없다면, waiting 활성화.
        if (!ExistUser)
        {
            SetState(HandManyPeopleState.WaitingPlayer);
            return;
        }
        
        SetState(HandManyPeopleState.Ready);
        SetName(userData.nickname);
        SetHand(userData.handType);
    }

    public void OnStartGame(UserData userData)
    {

    }

    public void OnStartRound(UserData userData)
    {
        if (!ExistUser)
            return;

        SetState(userData.isAlive ? HandManyPeopleState.Ready : HandManyPeopleState.LoseWaiting);
    }

    public void OnEndRound(UserData userData)
    {
        if (userData == null)
            return;

        SetHand(userData.handType);
        //SetState(userData.isAlive ? HandManyPeopleState.Win : HandManyPeopleState.Lose);
    }

    public void ShowLoser(UserData userData)
    {
        if (!userData.isAlive)
            SetState(HandManyPeopleState.Lose);
    }

    public void OnEndGame(UserData userData)
    {
        SetState(HandManyPeopleState.Ready);
    }

    public void OnResetGame(UserData userData)
    {
        if (userData == null)
        {
            SetState(HandManyPeopleState.WaitingPlayer);
            return;
        }

        curResult = ResultType.Alive;
        curState = HandManyPeopleState.Ready;
        PlayRandom();
        SetState(HandManyPeopleState.Ready);
    }

    public void OnFrontHandSpeak(UserData userData)
    {

    }

    public void OnUserHandChange(UserData userData)
    {
        this.userData = userData;

        // 유저가 없다면, waiting 활성화.
        if (!ExistUser)
        {
            SetState(HandManyPeopleState.WaitingPlayer);
            return;
        }

        SetState(userData.isAlive ? HandManyPeopleState.Ready : HandManyPeopleState.LoseWaiting);
        SetHand(userData.handType);
    }

    public virtual void SetHand(HandType handType)
    {
        if (userData == null)
            return;

        userData.SetHandType(handType);

        // 패배후 대기중이라면, 랜덤 재생 하지 않기.
        if (handType == HandType.empty && CurState != HandManyPeopleState.LoseWaiting)
            PlayRandom();
        else if(CurState != HandManyPeopleState.LoseWaiting)
            StopRandom();
        UpdateFingerObject(handType);
    }

    public void PlayRandom()
    {
        if (randomCor != null)
            return;
        randomCor = StartCoroutine(RandomCor());
    }

    protected void StopRandom()
    {
        if (randomCor != null)
            StopCoroutine(randomCor);
        randomCor = null;
    }

    IEnumerator RandomCor()
    {
        var _wait = new WaitForSeconds(0.1f);
        while (true)
        {
            switch (showHandType)
            {
                case HandType.rock: showHandType = HandType.paper; break;
                case HandType.paper: showHandType = HandType.scissors; break;
                case HandType.scissors: showHandType = HandType.rock; break;
            }

            UpdateFingerObject(showHandType);
            yield return _wait;
        }
    }

    protected void UpdateFingerObject(HandType handType)
    {
        switch (handType)
        {
            case HandType.rock: handImage.sprite = rockSprite; break;
            case HandType.paper: handImage.sprite = paperSprite; break;
            case HandType.scissors: handImage.sprite = scissorsSprite; break;
        }
    }

    void SetName(string name)
    {
        nameText.text = name;
    }

    void SetState(HandManyPeopleState state)
    {
        if (curState == HandManyPeopleState.LoseWaiting)
            return;

        curState = state;

        content.SetActive(state != HandManyPeopleState.WaitingPlayer);
        winObject.SetActive(false);
        loseObject.SetActive(false);
        if (state == HandManyPeopleState.Win && state != HandManyPeopleState.LoseWaiting)
            SetResult(ResultType.Win);
        else if (state == HandManyPeopleState.Lose && state != HandManyPeopleState.LoseWaiting)
            SetResult(ResultType.Lose);

        bool isDeath = state == HandManyPeopleState.LoseWaiting;
        if(isDeath)
        {
            handImage.DOFade(0.2f, 0f);
        }
        else
        {
            handImage.DOFade(1f, 0f);
        }
    }

    void SetResult(ResultType resultType)
    {
        StartCoroutine(SetResultCor(resultType));
    }

    IEnumerator SetResultCor(ResultType resultType)
    {
        if (curResult == ResultType.Death)
            yield break;

        if (resultType == ResultType.Win)
            curResult = ResultType.Alive;
        else if (resultType == ResultType.Lose)
            curResult = ResultType.Death;

        winObject.SetActive(resultType == ResultType.Win);
        loseObject.SetActive(resultType == ResultType.Lose);

        if(resultType == ResultType.Lose)
        {
            content.transform.DOKill();
            content.transform.DOShakePosition(0.4f, 15f, 30, 200f);
        }

        yield return new WaitForSeconds(2f);

        yield break;
        if (curResult == ResultType.Alive)
            SetState(HandManyPeopleState.Ready);
        else if (curResult == ResultType.Death)
            SetState(HandManyPeopleState.LoseWaiting);
    }
}
