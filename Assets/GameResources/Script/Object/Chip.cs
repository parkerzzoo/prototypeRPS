using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Chip : MonoBehaviour
{
    private Transform handFollowTrans = null;
    private Transform trans;

    private void Awake()
    {
        trans = transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("HandBringChipCollider"))
        {
            handFollowTrans = collision.gameObject.GetComponentInParent<Hand_FewPeople>().bringChipPos;
        }
    }

    private void Update()
    {
        if (handFollowTrans != null)
            trans.position = Vector3.Lerp(trans.position, handFollowTrans.position, Time.deltaTime * 80f);
    }

    private void OnDisable()
    {
        handFollowTrans = null;
    }

    public void ShowChip(Vector3 putPos)
    {
        trans.DOKill();
        trans.DOMove(putPos, 0.3f).SetEase(Ease.OutExpo);
    }

    public void BringChip(float delayTime)
    {
        StopAllCoroutines();
        StartCoroutine(BringChipCor(delayTime));
    }

    IEnumerator BringChipCor(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        gameObject.SetActive(false);
    }
}
