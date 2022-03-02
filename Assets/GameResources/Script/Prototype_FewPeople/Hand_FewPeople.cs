using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Hand_FewPeople : MonoBehaviour
{
    public enum HandManyPeopleState { WaitingPlayer, Win, Lose, LoseWaiting, Ready }
    public enum ResultType { Win, Lose, Death, Alive }

    private HandManyPeopleState curState = HandManyPeopleState.WaitingPlayer;
    private ResultType curResult = ResultType.Alive;

    public HandManyPeopleState CurState { get { return curState; } }
    public ResultType CurResult { get { return curResult; } }

    [SerializeField] private Transform chipCreatePos;
    [SerializeField] private Transform chipGivePos;
    [SerializeField] private Animator animator;
    [SerializeField] private HandUI_FewPeople handUI;

    [SerializeField] private GameObject content;
    [SerializeField] private Image handImage;

    private Coroutine randomCor = null;
    private HandType showHandType = HandType.rock;

    public UserData userData = null;
    public bool Dead { get { return curState == HandManyPeopleState.LoseWaiting; } }
    private bool ExistUser { get { return userData != null; } }

    public void OnUserListChange(UserData userData)
    {
        this.userData = userData;

        // 유저가 없다면, waiting 활성화.
        if (!ExistUser)
        {
            SetName(null);
            SetState(HandManyPeopleState.WaitingPlayer);
            return;
        }

        SetState(HandManyPeopleState.Ready);
        SetName(userData.nickname);
        SetHand(userData.handType, false);
    }

    public void OnStartGame(UserData userData)
    {
        if (userData != null && userData.isAlive)
            Invoke("PlayBettingAni", Random.Range(0f, 0.1f));
    }

    void PlayBettingAni()
    {
        animator.SetTrigger("bettingTrigger");
    }

    public void OnStartRound(UserData userData, float duration)
    {
        if (!ExistUser)
            return;

        if (userData.isAlive)
        {
            float _readyClipTime = GetAnimLength(animator, "HandObject_FewPeople_Ready");
            animator.speed = _readyClipTime / duration;
            animator.SetTrigger("readyTrigger");
        }

        SetState(userData.isAlive ? HandManyPeopleState.Ready : HandManyPeopleState.LoseWaiting);
    }

    public void OnEndRound(UserData userData)
    {
        if (userData == null)
            return;

        SetHand(userData.handType, userData.isAlive);
        SetState(userData.isAlive ? HandManyPeopleState.Win : HandManyPeopleState.Lose);
    }

    public void ShowLoser(UserData userData)
    {
        if (!userData.isAlive)
            SetState(HandManyPeopleState.Lose);
    }

    public void OnEndGame(UserData userData)
    {
        if (userData != null && userData.isAlive)
        {
            ChipCreateControl.Instance.GiveChip(chipGivePos.position);
        }

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
        //PlayRandom();
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

    public virtual void SetHand(HandType handType, bool playAni = true)
    {
        if (userData == null)
            return;

        userData.SetHandType(handType);

        // 패배후 대기중이라면, 랜덤 재생 하지 않기.
        /*if (handType == HandType.empty && CurState != HandManyPeopleState.LoseWaiting)
            PlayRandom();
        else if (CurState != HandManyPeopleState.LoseWaiting)
            StopRandom();*/

        if(playAni)
            PlaySetHandAni(handType);
    }

    /*public void PlayRandom()
    {
        if (randomCor != null)
            return;
        //randomCor = StartCoroutine(RandomCor());
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
    }*/

    protected void PlaySetHandAni(HandType handType)
    {
        switch (handType)
        {
            case HandType.rock: animator.SetTrigger("rockTrigger"); break;
            case HandType.paper: animator.SetTrigger("paperTrigger"); break;
            case HandType.scissors: animator.SetTrigger("scissorsTrigger"); break;
        }
    }

    /*protected void UpdateFingerObject(HandType handType)
    {
        switch (handType)
        {
            case HandType.rock: animator.SetTrigger("rockTrigger"); break;
            case HandType.paper: animator.SetTrigger("paperTrigger"); break;
            case HandType.scissors: animator.SetTrigger("scissorsTrigger"); break;
        }
    }*/

    void SetName(string name)
    {
        handUI.ControlActiveNameText(!string.IsNullOrEmpty(name));
        //nameText.text = name;
        handUI.SetName(name);
    }

    void SetState(HandManyPeopleState state)
    {
        if (curState == HandManyPeopleState.LoseWaiting)
            return;

        curState = state;

        bool _curActive = content.activeSelf;
        bool _nextActive = state != HandManyPeopleState.WaitingPlayer;
        if (!_curActive && _nextActive)
            animator.SetBool("appearTrigger", true);
        else if (_curActive && !_nextActive)
            animator.SetBool("disappearTrigger", false);

        content.SetActive(_nextActive);
        /////winObject.SetActive(false);
        /////loseObject.SetActive(false);
        if (state == HandManyPeopleState.Win && state != HandManyPeopleState.LoseWaiting)
            SetResult(ResultType.Win);
        else if (state == HandManyPeopleState.Lose && state != HandManyPeopleState.LoseWaiting)
            SetResult(ResultType.Lose);

        bool isDeath = state == HandManyPeopleState.LoseWaiting;
        if (isDeath)
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

        /////winObject.SetActive(resultType == ResultType.Win);
        /////loseObject.SetActive(resultType == ResultType.Lose);
        if (resultType == ResultType.Win)
            handUI.ShowWinPanel();

        if (resultType == ResultType.Lose)
        {
            content.transform.DOKill();
            content.transform.DOShakePosition(0.1f, 2f, 3, 200f);
        }

        yield return new WaitForSeconds(2f);

        yield break;
        if (curResult == ResultType.Alive)
            SetState(HandManyPeopleState.Ready);
        else if (curResult == ResultType.Death)
            SetState(HandManyPeopleState.LoseWaiting);
    }

    public void CreateChip()
    {
        ChipCreateControl.Instance.CreateChip(chipCreatePos.position);
    }

    private float GetAnimLength(Animator animator, string animName)
    {
        float time = 0;
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;

        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if (ac.animationClips[i].name == animName)
            {
                time = ac.animationClips[i].length;
            }
        }

        return time;
    }

    public void InitAnimatorSpeed()
    {
        animator.speed = 1f;
    }

    [ContextMenu("Betting")]
    public void PlayBetting()
    {
        animator.SetTrigger("bettingTrigger");
    }
}
