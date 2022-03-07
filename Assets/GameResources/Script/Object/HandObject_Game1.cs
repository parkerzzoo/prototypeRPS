using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HandObject_Game1 : HandObject
{
    public enum HandManyPeopleState { Empty, WaitingPlayer, LoseWaiting, Ready }

    private HandManyPeopleState curState = HandManyPeopleState.Empty;
    public HandManyPeopleState CurState { get { return curState; } }

    //public Transform bringChipPos;

    [SerializeField] private Transform chipCreatePos;
    [SerializeField] private Transform chipGivePos;
    [SerializeField] private Animator animator;
    [SerializeField] private HandUI_FewPeople handUI;

    [SerializeField] private GameObject content;
    [SerializeField] private Image handImage;

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
            float _readyClipTime = GetAnimLength(animator, "HandObject_FewPeople_Ready1");
            animator.speed = _readyClipTime / duration;
            animator.SetTrigger("readyTrigger");
        }

        SetState(userData.isAlive ? HandManyPeopleState.Ready : HandManyPeopleState.LoseWaiting);
    }

    public void OnEndRound(UserData userData)
    {
        if (userData == null)
            return;

        SetHand(userData.handType, curState != HandManyPeopleState.LoseWaiting);
        SetResult(userData.currentResult);
    }

    public void OnEndGame(UserData userData)
    {
        if (userData != null && userData.isAlive)
        {
            animator.SetTrigger("bringChipTrigger");//"hitTableTrigger");
            ChipCreateControl.Instance.GiveChip();
        }
    }

    public void OnResetGame(UserData userData)
    {
        if (userData == null)
        {
            SetState(HandManyPeopleState.WaitingPlayer);
            return;
        }

        animator.SetBool("loseWaiting", false);
        curState = HandManyPeopleState.Ready;
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

        if(playAni)
            PlaySetHandAni(handType);
    }

    protected void PlaySetHandAni(HandType handType)
    {
        switch (handType)
        {
            case HandType.rock: animator.SetTrigger("rockTrigger"); break;
            case HandType.paper: animator.SetTrigger("paperTrigger"); break;
            case HandType.scissors: animator.SetTrigger("scissorsTrigger"); break;
        }
    }

    void SetName(string name)
    {
        handUI.ControlActiveNameText(!string.IsNullOrEmpty(name));
        handUI.SetName(name);
    }

    void SetState(HandManyPeopleState state)
    {
        curState = state;

        bool _activeBool = animator.GetBool("activeBool");

        if (curState == HandManyPeopleState.Ready && !_activeBool)
        {
            animator.SetBool("activeBool", true);
            animator.SetTrigger("appearTrigger");
        }
        else if (curState == HandManyPeopleState.WaitingPlayer && _activeBool)
        {
            animator.SetBool("activeBool", false);
            animator.SetTrigger("disappearTrigger");
        }

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
        if(CurState == HandManyPeopleState.LoseWaiting)
            yield break;

        else if (resultType == ResultType.lose)
        {
            animator.SetBool("loseWaiting", true);
        }
        if (resultType == ResultType.win)
            handUI.ShowWinPanel();
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

    [ContextMenu("PlayHitTable")]
    public void PlayHitTable()
    {
        CameraShaking.Instance.HitTable();
        ChipCreateControl.Instance.GiveChip();
        //ChipCreateControl.Instance.GiveChip(chipGivePos.position);
    }

    [ContextMenu("HitTable")]
    public void HitTable()
    {
        animator.SetTrigger("hitTableTrigger");
    }

    public void InactvieContent()
    {
        //content.SetActive(false);
    }
}
