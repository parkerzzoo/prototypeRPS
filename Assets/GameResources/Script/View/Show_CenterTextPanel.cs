using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Show_CenterTextPanel : MonoBehaviour
{
	public TMPro.TextMeshProUGUI startGameText;
	public GameObject startGamePanel;

	public Transform beforePos;
	public Transform afterPos;
	public Transform showPos;

	Coroutine centerTextCor = null;

    private void Start()
    {
		startGamePanel.transform.DOScaleY(0f, 0f);
		startGameText.DOFade(0f, 0f);
	}

	public void Show(string content, float delay, float duration)
    {
		if (centerTextCor != null)
			StopCoroutine(centerTextCor);

		centerTextCor = StartCoroutine(ShowCenterTextPanelCor(content, delay, duration));
	}

    IEnumerator ShowCenterTextPanelCor(string content, float delay, float duration)
	{
		startGameText.DOKill();
		startGameText.DOFade(0f, 0f);

		startGameText.transform.localPosition = beforePos.localPosition;

		startGameText.text = content;

		yield return new WaitForSeconds(delay);

		ControlActiveCenterText(true);

		yield return new WaitForSeconds(0.05f);

		startGameText.DOFade(1f, 0.1f);

		startGameText.transform.DOLocalMove(showPos.localPosition, 0.2f).SetEase(Ease.OutBack);

		yield return new WaitForSeconds(duration);

		startGameText.DOFade(0f, 0.1f);

		startGameText.transform.DOLocalMove(afterPos.localPosition, 0.2f).SetEase(Ease.InBack);

		yield return new WaitForSeconds(0.1f);

		ControlActiveCenterText(false);

		centerTextCor = null;
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
}
