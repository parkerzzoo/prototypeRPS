using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HandUI_FewPeople : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI nameText;

    [SerializeField] private Image winImage;
    [SerializeField] private Transform winBeforePos;
    [SerializeField] private Transform winCenterPos;
    [SerializeField] private Transform winAfterPos;

    private Coroutine winShowCor = null;

    private void Start()
    {
        winImage.gameObject.SetActive(false);
        winImage.transform.DOKill();
        winImage.transform.localPosition = winBeforePos.localPosition;
        winImage.DOKill();
        winImage.DOFade(0f, 0f);
    }

    public void ControlActiveNameText(bool isAcitve)
    {
        nameText.gameObject.SetActive(isAcitve);
    }

    public void SetName(string name)
    {
        nameText.text = name;
    }

    public void ShowWinPanel()
    {
        if (winShowCor != null)
            StopCoroutine(winShowCor);

        winShowCor = StartCoroutine(ShowWinPanelCor());
    }

    IEnumerator ShowWinPanelCor()
    {
        yield return new WaitForSeconds(0.5f);

        winImage.gameObject.SetActive(true);
        winImage.transform.DOKill();
        winImage.transform.localPosition = winBeforePos.localPosition;
        winImage.DOKill();
        winImage.DOFade(0f, 0f);

        yield return null;

        winImage.transform.DOLocalMove(winCenterPos.localPosition, 0.1f);
        winImage.DOFade(1f, 0.1f);

        yield return new WaitForSeconds(1f);

        winImage.transform.DOLocalMove(winAfterPos.localPosition, 0.1f);
        winImage.DOFade(0f, 0.1f);

        yield return new WaitForSeconds(0.1f);

        winImage.gameObject.SetActive(false);
        winShowCor = null;
    }
}
