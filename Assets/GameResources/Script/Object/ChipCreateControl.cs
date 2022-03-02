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

    public void CreateChip(Vector3 createPos)
    {
        GameObject _pooled;
        chipPool.TryGetNextObject(createPos, Quaternion.identity, out _pooled);
        chipList.Add(_pooled.transform);
        ShowChip(_pooled.transform);
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
            chipList[i].DOKill();
            chipList[i].DOMove(givePos, 0.3f).SetEase(Ease.OutCubic);
            yield return new WaitForSeconds(Random.Range(0f, 0.05f));
        }
        yield return new WaitForSeconds(0.4f);
        for (int i = 0; i < chipList.Count; i++)
            chipList[i].gameObject.SetActive(false);
    }

    void ShowChip(Transform chipTrans)
    {
        Vector3 _aimPos = new Vector3(Random.Range(createRange_LeftBottom.position.x, createRange_RightTop.position.x),
            Random.Range(createRange_LeftBottom.position.y, createRange_RightTop.position.y), createRange_LeftBottom.position.z);

        chipTrans.DOKill();
        chipTrans.DOMove(_aimPos, 0.3f).SetEase(Ease.OutExpo);
    }
}
