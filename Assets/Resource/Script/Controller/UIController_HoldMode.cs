using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class UIController_HoldMode : Singletone<UIController_HoldMode>
{
	public Image rockButtonBack;
	public Image paperButtonBack;
	public Image scissorsButtonBack;
	public Sprite activeButtonBack;
	public Sprite inactiveButtonBack;

	public GameObject startGameButton;
	public GameObject startGamePanel;

	public TMPro.TextMeshProUGUI startGameText;
	public TMPro.TextMeshProUGUI userCountText;
	public TMPro.TextMeshProUGUI myUidText;

	public void UpdateUserCountText(int count)
	{
		userCountText.text = "참여 인원: " + count;
	}

	public void UpdateMyUidText()
	{
		myUidText.text = "내 아이디: " + VarList.userId;
	}

	public void UpdateStartGameText(string content)
    {
		startGameText.transform.DOPunchScale(Vector3.one * 0.2f, 0.1f, 1);
		startGameText.text = content;
	}

	public void SendRock()
	{
		if(GameController_BasicMode.Instance.SelectHand(HandType.rock))
        {
			DeselectAllHand();
			rockButtonBack.sprite = activeButtonBack;
		}
	}

	public void SendPaper()
	{
		if (GameController_BasicMode.Instance.SelectHand(HandType.paper))
		{
			DeselectAllHand();
			paperButtonBack.sprite = activeButtonBack;
		}
	}

	public void SendScissors()
	{
		if (GameController_BasicMode.Instance.SelectHand(HandType.scissors))
		{
			DeselectAllHand();
			scissorsButtonBack.sprite = activeButtonBack;
		}
	}

	void DeselectAllHand()
    {
		rockButtonBack.sprite = inactiveButtonBack;
		paperButtonBack.sprite = inactiveButtonBack;
		scissorsButtonBack.sprite = inactiveButtonBack;
	}

	public void StartGame(float selectionTime, Action callback)
	{
		StartCoroutine(StartGameCor(selectionTime, callback));
	}

	IEnumerator StartGameCor(float selectionTime, Action callback)
	{
		startGameButton.SetActive(false);
		startGamePanel.SetActive(true);
		startGamePanel.transform.DOScaleY(1f, 0.1f).SetEase(Ease.OutBack);

		UpdateStartGameText("가위!");
		yield return new WaitForSeconds(selectionTime / 2.5f);

		UpdateStartGameText("바위!!");
		yield return new WaitForSeconds(selectionTime / 2.5f);

		UpdateStartGameText("보!!!");
		yield return new WaitForSeconds(0.5f);

		startGamePanel.transform.DOScaleY(0f, 0.05f).SetEase(Ease.InBack);
		callback?.Invoke();
	}
}
