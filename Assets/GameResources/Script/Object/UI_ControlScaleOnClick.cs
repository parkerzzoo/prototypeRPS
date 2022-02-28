using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UI_ControlScaleOnClick : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerClickHandler
{
    public Transform targetTrans;
    public bool firstScaleIsOne;
    public float punchScale = 0.95f;
    Vector3 firstScale;
    bool isActivePunchScale;

    void Awake()
    {
        isActivePunchScale = true;
        if(targetTrans == null)
            targetTrans = transform;
        firstScale = firstScaleIsOne? Vector3.one: targetTrans.localScale;
    }

    public void ControlActivePunchScale(bool isActivePunchScale)
    {
        if(this.isActivePunchScale)
        {
            targetTrans.DOKill();
            targetTrans.DOScale(firstScale, 0.1f);
        }
        this.isActivePunchScale = isActivePunchScale;
    }

    public void OnPointerDown(PointerEventData e)
    {
        if(targetTrans == null || !isActivePunchScale)
            return;
        targetTrans.DOKill();
        targetTrans.DOScale(firstScale * punchScale, 0.1f);
    }

    public void OnPointerUp(PointerEventData e)
    {
        if(targetTrans == null || !isActivePunchScale)
            return;
        targetTrans.DOKill();
        targetTrans.DOScale(firstScale, 0.1f);
    }

    public void OnPointerExit(PointerEventData e)
    {
        if(targetTrans == null || !isActivePunchScale)
            return;
        targetTrans.DOKill();
        targetTrans.DOScale(firstScale, 0.1f);
    }

    public void OnPointerClick(PointerEventData e)
    {
        if(targetTrans == null || !isActivePunchScale)
            return;
        targetTrans.DOKill();
        targetTrans.DOScale(firstScale, 0.1f);
    }
}
