using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EZObjectPools;

public class ChipCreateControl : Singletone<ChipCreateControl>
{
    [SerializeField] private EZObjectPool chipPool;

    [SerializeField] private Transform createRange_LeftBottom;
    [SerializeField] private Transform createRange_RightTop;

    private List<Transform> chipList = new List<Transform>();

    public void CreateChip(Vector3 createPos, Vector3 putPos)
    {
        GameObject _pooled;
        chipPool.TryGetNextObject(createPos, Quaternion.identity, out _pooled);
        _pooled.transform.localScale = Vector3.one;
        _pooled.GetComponentInChildren<SpriteRenderer>().sortingOrder = -1;
        chipList.Add(_pooled.transform);
        ShowChip(_pooled.transform, putPos);
    }

    public void GiveChip(Vector3 givePos)
    {
        StopAllCoroutines();
        StartCoroutine(GiveChipCor(givePos));
    }

    IEnumerator GiveChipCor(Vector3 givePos)
    {
        for (int i = 0; i < chipList.Count; i++)
        {
            Vector3 _bouncePos = transform.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
       
            chipList[i].DOKill();
            chipList[i].DOMove(_bouncePos, 0.5f);
            chipList[i].DOScale(Random.Range(1.4f, 1.5f), 0.7f).SetEase(Ease.OutCubic);
            chipList[i].DOShakeRotation(0.7f, 90, 50);
            chipList[i].GetComponentInChildren<SpriteRenderer>().sortingOrder = 1;
        }

        yield return new WaitForSeconds(0.7f);

        for (int i = 0; i < chipList.Count; i++)
        {
            chipList[i].DOKill();
            chipList[i].DOMove(givePos, 0.3f).SetEase(Ease.OutCubic);
            chipList[i].DOScale(1, 0.3f);
            yield return new WaitForSeconds(Random.Range(0f, 0.05f));
        }
        yield return new WaitForSeconds(0.4f);
        for (int i = 0; i < chipList.Count; i++)
        {
            chipList[i].gameObject.SetActive(false);
            chipList[i].transform.localScale = Vector3.one;
        }
            
    }

    void ShowChip(Transform chipTrans, Vector3 putPos)
    {
        Vector3 _aimPos = new Vector3(Random.Range(createRange_LeftBottom.position.x, createRange_RightTop.position.x),
            Random.Range(createRange_LeftBottom.position.y, createRange_RightTop.position.y), createRange_LeftBottom.position.z);

        chipTrans.DOKill();
        chipTrans.DOMove(putPos, 0.3f).SetEase(Ease.OutExpo);
    }
}
