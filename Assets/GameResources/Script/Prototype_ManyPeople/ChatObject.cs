using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ChatObject : MonoBehaviour
{
    public TextMeshProUGUI chatText;

    public void Init(string content)
    {
        chatText.text = content;
    }

    private IEnumerator Start()
    {
        chatText.DOKill();
        chatText.DOFade(1f, 0.1f);

        yield return new WaitForSeconds(2f);

        chatText.DOKill();
        chatText.DOFade(0f, 0.1f);

        yield return new WaitForSeconds(0.1f);

        chatText.gameObject.SetActive(false);
    }
}
