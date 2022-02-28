using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Show_CenterTimerPanel : MonoBehaviour
{
	public GameObject timerObject;
	public Image timerGaugeFrontImage;
	public TMPro.TextMeshProUGUI timerText;

	Coroutine timerCor = null;

	private void Start()
    {
		ControlActiveTimer(false);
	}

	public void Show(float time)
	{
		if (timerCor != null)
			StopCoroutine(timerCor);

		timerCor = StartCoroutine(StartTimerCor(time));
	}

	IEnumerator StartTimerCor(float time)
	{
		ControlActiveTimer(true);
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
		yield return new WaitForSeconds(0.5f);
		ControlActiveTimer(false);
	}

	public void ControlActiveTimer(bool isActive)
	{
		if (isActive)
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
}
