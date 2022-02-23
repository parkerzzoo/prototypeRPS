using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZObjectPools;

public class UI_Hand : MonoBehaviour
{
    public enum HandUIState { WaitingPlayer, NormalRound, AdditionalRound, LoseWaiting, Ready }

    [SerializeField] EZObjectPool resultUIPool;
    List<UI_ResultUI> resultUIs = new List<UI_ResultUI>();
    
    public TMPro.TextMeshProUGUI nameText;
    public GameObject waitingPanel;
    public GameObject normalRoundPanel;
    public GameObject additionalRoundPanel;
    public GameObject loseWaitingPanel;
    public GameObject readyPanel;

    public GameObject resultUIParent;

    public void Init(UserData userData)
    {
        nameText.text = userData.userId;
    }

    public void InitResultUI(int roundCount)
    {
        for (int i = 0; i < resultUIs.Count; i++)
        {
            resultUIs[i].Init();
            resultUIs[i].gameObject.SetActive(false);
        }
        resultUIs.Clear();

        GameObject _pooled;

        for (int i = 0; i < roundCount; i++)
        {
            resultUIPool.TryGetNextObject(Vector3.zero, Quaternion.identity, out _pooled);
            UI_ResultUI _result = _pooled.GetComponent<UI_ResultUI>();
            _result.Init();
            _result.transform.SetAsLastSibling();
            _result.transform.localScale = Vector3.one;
            resultUIs.Add(_result);
        }
    }

    public void SetResult(ResultType[] resultTypes)
    {
        for (int i = 0; i < resultUIs.Count; i++)
            resultUIs[i].Init();

        for (int i = 0; i < resultUIs.Count; i++)
        {
            if (i >= resultTypes.Length)
                break;
            resultUIs[i].SetResult(resultTypes[i]);
        }
    }

    public void SetState(HandUIState handUIState)
    {
        waitingPanel.SetActive(handUIState == HandUIState.WaitingPlayer);

        normalRoundPanel.SetActive(handUIState != HandUIState.WaitingPlayer);
        additionalRoundPanel.SetActive(handUIState == HandUIState.AdditionalRound);
        loseWaitingPanel.SetActive(handUIState == HandUIState.LoseWaiting);
        readyPanel.SetActive(handUIState == HandUIState.Ready);

        resultUIParent.SetActive(handUIState == HandUIState.NormalRound);
    }
}
