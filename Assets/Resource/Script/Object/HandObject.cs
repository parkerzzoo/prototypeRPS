using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum HandType { rock, paper, scissors, empty }
public enum ResultType { win, lose, draw }
public class HandObject : MonoBehaviour
{
    public Color winColor;
    public Color loseColor;
    public Color drawColor;

    HandType showHandType = HandType.rock;
    public Image handImage;

    public Sprite rockSprite;
    public Sprite paperSprite;
    public Sprite scissorsSprite;

    public TMPro.TextMeshProUGUI nameText;

    public Coroutine randomCor = null;

    public UserData userData;

    private void Start()
    {
        PlayRandom();
    }

    public void Init(UserData userData)
    {
        this.userData = userData;
        nameText.text = userData.userId;
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
            switch(showHandType)
            {
                case HandType.rock: showHandType = HandType.paper; break;
                case HandType.paper: showHandType = HandType.scissors; break;
                case HandType.scissors: showHandType = HandType.rock; break;
            }
            UpdateSprite();

            yield return _wait;
        }
    }

    public void SetHand(HandType handType)
    {
        if (handType == HandType.empty)
        {
            userData.SetHandType(handType);
            showHandType = HandType.rock;
            return;
        }

        userData.SetHandType(handType);

        StopRandom();
        showHandType = handType;

        UpdateSprite();
    }

    void UpdateSprite()
    {
        switch(showHandType)
        {
            case HandType.rock: handImage.sprite = rockSprite; break;
            case HandType.paper: handImage.sprite = paperSprite; break;
            case HandType.scissors: handImage.sprite = scissorsSprite; break;
        }
    }

    public void SetResult(ResultType resultType)
    {
        switch(resultType)
        {
            case ResultType.win: handImage.color = winColor; break;
            case ResultType.lose:handImage.color = loseColor; break;
            case ResultType.draw: handImage.color = drawColor; break;
        }
    }

    public void InitResult()
    {
        handImage.color = Color.white;
    }
}
