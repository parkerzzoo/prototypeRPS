using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private GameObject meshObject;
    [SerializeField] private Animator handAnimator;
    [SerializeField] private UI_Hand handUi;

    public Coroutine randomCor = null;
    HandType showHandType = HandType.rock;

    public UserData userData = null;

    public bool ExistUser{ get { return userData != null; } }

    public void Init(UserData userData)
    {
        // 유저가 없다면, waiting 활성화.
        if(userData == null)
        {
            this.userData = null;
            SetHandUI(UI_Hand.HandUIState.WaitingPlayer);
            //IsWaiting = true;
            return;
        }
        //IsWaiting = false;
        SetHandUI(UI_Hand.HandUIState.Ready);

        this.userData = userData;
        handUi.Init(userData);
        PlayRandom();
    }

    public void SetHand(UserData userData)
    {
        // 유저가 없다면, waiting 활성화.
        if (userData == null)
        {
            //SetHandUI(UI_Hand.HandUIState.Ready);
            //IsWaiting = true;
            return;
        }
        //IsWaiting = false;

        this.userData = userData;
        handUi.Init(userData);
        if (userData.handType != HandType.empty)
            StopRandom();
        UpdateFingerObject(userData.handType);
    }

    public void SetHand(HandType handType)
    {
        if (userData == null)
        {
            //SetHandUI(UI_Hand.HandUIState.Ready);
            //IsWaiting = true;
            return;
        }
        //IsWaiting = false;

        userData.SetHandType(handType);
        if (userData.handType != HandType.empty)
            StopRandom();
        UpdateFingerObject(handType);
    }

    public void StartNormalRound(int normalRoundCount)
    {
        if (!ExistUser)
            return;

        SetHandUI(UI_Hand.HandUIState.NormalRound);
        handUi.InitResultUI(normalRoundCount);
    }

    public void EndNormalRound(UserData userData)
    {
        if (userData == null)
            return;

        SetHand(userData);
        handUi.SetResult(userData.resultTypes);

        SetHandUI(userData.possiblePlay? UI_Hand.HandUIState.NormalRound: UI_Hand.HandUIState.LoseWaiting);
    }

    public void EndDeathMatchRound(UserData userData)
    {
        if (userData == null)
            return;

        SetHand(userData);
        handUi.SetResult(userData.resultTypes);
        SetHandUI(userData.possiblePlay ? UI_Hand.HandUIState.AdditionalRound : UI_Hand.HandUIState.LoseWaiting);
    }

    public void EndGame()
    {
        SetHandUI(UI_Hand.HandUIState.Ready);
    }

    public void ResetGame()
    {
        if (userData == null)
            return;

        SetHandUI(UI_Hand.HandUIState.Ready);
    }

    public void SetHandUI(UI_Hand.HandUIState handUIState)
    {
        handUi.SetState(handUIState);
        meshObject.SetActive(handUIState == UI_Hand.HandUIState.NormalRound
            || handUIState == UI_Hand.HandUIState.AdditionalRound
            || handUIState == UI_Hand.HandUIState.Ready);
    }

    public void PlayRandom()
    {
        StopRandom();

        randomCor = StartCoroutine(RandomCor());
    }

    void StopRandom()
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

    void UpdateFingerObject(HandType handType)
    {
        switch (handType)
        {
            case HandType.rock: handAnimator.SetTrigger("rockTrigger"); break;
            case HandType.paper: handAnimator.SetTrigger("paperTrigger"); break;
            case HandType.scissors: handAnimator.SetTrigger("scissorsTrigger"); break;
        }
    }
}
