using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ResultUI : MonoBehaviour
{
    [SerializeField] private Color winColor;
    [SerializeField] private Color loseColor;
    [SerializeField] private Color drawColor;
    [SerializeField] private Color normalColor;

    [SerializeField] private Image resultCircle;

    public void SetResult(ResultType resultType)
    {
        switch(resultType)
        {
            case ResultType.win: resultCircle.color = winColor; break;
            case ResultType.lose: resultCircle.color = loseColor; break;
            case ResultType.draw: resultCircle.color = drawColor; break;
        }
    }

    public void Init()
    {
        resultCircle.color = normalColor;
    }
}
