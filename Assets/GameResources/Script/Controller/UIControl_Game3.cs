using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class UIControl_Game3 : UIControl
{
	public ChatPanel chatPanel;
	
	public Transform rockButtonTrans;
	public Transform paperButtonTrans;
	public Transform scissorsButtonTrans;

	public GameObject startGamePanel;
	public GameObject connectErrorPanel;

	public Show_CenterTextPanel centerTextPanel;
	public Show_CenterTimerPanel centerTimerPanel;

	public TMPro.TextMeshProUGUI chipCountText;

	public TMPro.TextMeshProUGUI frontHandSpeakText;
	public Transform frontHandSpeakTrans;
	public Image frontHandSpeakBackImage;

	public Image coverImage;

	private void Start()
	{
		InactiveFrontHandSpeak(true);
		connectErrorPanel.SetActive(false);
	}

	public void FadeOutCoverImage()
    {
		coverImage.DOFade(0f, 1f).OnComplete(() => coverImage.gameObject.SetActive(false)).SetEase(Ease.InQuart);
	}

	public void ActiveConnectErrorPanel()
    {
		connectErrorPanel.SetActive(true);
	}

	void InactiveFrontHandSpeak(bool isImmediate)
    {
		frontHandSpeakTrans.DOScale(0, isImmediate? 0f: 0.5f).SetEase(Ease.OutBack);
		frontHandSpeakBackImage.DOFade(0f, isImmediate ? 0f : 0.2f);
		frontHandSpeakText.DOFade(0f, isImmediate ? 0f : 0.2f);
	}

    public void UpdateChipCountText(int count)
	{
		//chipCountText.text = "내 칩: " + count;
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

	public void InvokeExcute(float delayTime, Action callback)
    {
		StartCoroutine(InvokeExcuteCor(delayTime, callback));
    }

	IEnumerator InvokeExcuteCor(float delayTime, Action callback)
    {
		yield return new WaitForSeconds(delayTime);
		callback?.Invoke();
	}

	public void ShowCenterTextPanel(string content, float delay, float duration)
	{
		centerTextPanel.Show(content, delay, duration);
	}

	public void ShowCenterTimerPanel(float time)
    {
		centerTimerPanel.Show(time);
	}


	Coroutine frontHandSpeakCor = null;

	public void CreatChatObject(string content)
	{
		chatPanel.AddChat(content);
	}

	public void ActiveFrontHandSpeak(string content)
	{
		if (frontHandSpeakCor != null)
			StopCoroutine(frontHandSpeakCor);

		frontHandSpeakCor = StartCoroutine(ActiveFrontHandSpeakCor(content));
	}

	IEnumerator ActiveFrontHandSpeakCor(string content)
	{
		frontHandSpeakText.text = content;

		frontHandSpeakTrans.DOKill();
		if (frontHandSpeakTrans.localScale.x == 1f)
			frontHandSpeakTrans.DOPunchScale(Vector3.one * 0.2f, 0.1f);
		else
			frontHandSpeakTrans.DOScale(1, 0.2f).SetEase(Ease.OutBack);

		frontHandSpeakBackImage.DOKill();
		frontHandSpeakBackImage.DOFade(1f, 0.2f);
		frontHandSpeakText.DOKill();
		frontHandSpeakText.DOFade(1f, 0.2f);

		yield return new WaitForSeconds(3f);

		InactiveFrontHandSpeak(false);
	}
}
