using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class UIController_BasicMode : Singletone<UIController_BasicMode>
{
	public Image timerGaugeFrontImage;
	public TMPro.TextMeshProUGUI timerText;
	public GameObject timerObject;

	public Transform rockButtonTrans;
	public Transform paperButtonTrans;
	public Transform scissorsButtonTrans;

	public GameObject startGameButton;
	public GameObject startGamePanel;

	public TMPro.TextMeshProUGUI startGameText;
	public TMPro.TextMeshProUGUI chipCountText;

    private void Start()
    {
		startGamePanel.transform.DOScaleY(0f, 0f);
		ControlActiveTimer(false);
	}

    public void UpdateChipCountText(int count)
	{
		chipCountText.text = "내 칩: " + count;
	}

	public void UpdateStartGameText(string content)
    {
		startGameText.text = content;
		startGameText.transform.DOPunchScale(Vector3.one * 0.2f, 0.1f, 1);
	}

	public void SendRock()
	{
		if(GameController_BasicMode.Instance.SelectHand(HandType.rock))
        {
			DeselectAllHand();
			rockButtonTrans.DOKill();
			rockButtonTrans.DOScale(1.1f, 0.1f).SetEase(Ease.OutBack);
		}
	}

	public void SendPaper()
	{
		if (GameController_BasicMode.Instance.SelectHand(HandType.paper))
		{
			DeselectAllHand();
			paperButtonTrans.DOKill();
			paperButtonTrans.DOScale(1.1f, 0.1f).SetEase(Ease.OutBack);
		}
	}

	public void SendScissors()
	{
		if (GameController_BasicMode.Instance.SelectHand(HandType.scissors))
		{
			DeselectAllHand();
			scissorsButtonTrans.DOKill();
			scissorsButtonTrans.DOScale(1.1f, 0.1f).SetEase(Ease.OutBack);
		}
	}

	void DeselectAllHand(bool immediate = false)
    {
		if(immediate)
        {
			rockButtonTrans.transform.localScale = Vector3.one;
			paperButtonTrans.transform.localScale = Vector3.one;
			scissorsButtonTrans.transform.localScale = Vector3.one;
		}
		else
        {
			rockButtonTrans.DOKill();
			rockButtonTrans.DOScale(1f, 0.1f).SetEase(Ease.OutBack);

			paperButtonTrans.DOKill();
			paperButtonTrans.DOScale(1f, 0.1f).SetEase(Ease.OutBack);

			scissorsButtonTrans.DOKill();
			scissorsButtonTrans.DOScale(1f, 0.1f).SetEase(Ease.OutBack);
		}
	}

	public void StartRound(float selectionTime, Action callback)
	{
		DeselectAllHand();
		ControlActiveTimer(true);
		StartTimer(selectionTime, callback);
	}

	IEnumerator StartGameCor(float selectionTime, Action callback)
	{
		DeselectAllHand(true);
		startGameButton.SetActive(false);
		ControlActiveCenterText(true);

		UpdateStartGameText("가위!");
		yield return new WaitForSeconds(selectionTime / 2.5f);

		UpdateStartGameText("바위!!");
		yield return new WaitForSeconds(selectionTime / 2.5f);

		UpdateStartGameText("보!!!");
		yield return new WaitForSeconds(0.5f);

		ControlActiveCenterText(false);
		callback?.Invoke();
	}

	public void InvokeExcute(float delayTime, Action callback)
    {
		StartCoroutine(InvokeExcuteCor(delayTime, callback));
    }

	IEnumerator InvokeExcuteCor(float delayTime, Action callback)
    {
		yield return new WaitForSeconds(delayTime);
		callback?.Invoke();
	}

	public void ControlActiveCenterText(bool isActive)
	{
		if (isActive)
		{
			startGamePanel.transform.DOKill();
			startGamePanel.transform.DOScaleY(1f, 0.1f).SetEase(Ease.OutBack);
		}
		else
		{
			startGamePanel.transform.DOKill();
			startGamePanel.transform.DOScaleY(0f, 0.1f).SetEase(Ease.InBack);
		}
	}

	public void ControlActiveTimer(bool isActive)
    {
		if(isActive)
        {
			timerObject.transform.DOKill();
			timerObject.transform.DOScale(1f, 0.1f).SetEase(Ease.OutBack);
		}
		else
        {
			timerObject.transform.DOKill();
			timerObject.transform.DOScale(0f, 0.1f).SetEase(Ease.InBack);
		}
    }

	Coroutine timerCor = null;

	public void StartTimer(float time, Action callback)
    {
		if (timerCor != null)
			StopCoroutine(timerCor);
		timerCor = StartCoroutine(StartTimerCor(time, callback));
	}

	IEnumerator StartTimerCor(float time, Action callback)
    {
		timerGaugeFrontImage.fillAmount = 1f;
		timerGaugeFrontImage.DOFillAmount(0f, time).SetEase(Ease.Linear);

		int _time = (int)time;
		for (int i = 0; i < _time; i++)
        {
			timerText.text = (_time - i).ToString();
			yield return new WaitForSeconds(1f);
			timerObject.transform.DOPunchScale(Vector3.one * 0.2f, 0.1f, 1);
		}
		timerText.text = "0";
		callback?.Invoke();
		yield return new WaitForSeconds(0.5f);
		ControlActiveTimer(false);
	}
}
