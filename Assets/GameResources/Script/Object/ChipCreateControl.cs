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

    [SerializeField] private float inactiveChipDelayTime;

    private List<Chip> chipList = new List<Chip>();

    public void CreateChip(Vector3 createPos)
    {
        GameObject _pooled;
        chipPool.TryGetNextObject(createPos, Quaternion.identity, out _pooled);
        _pooled.transform.localScale = Vector3.one;

        Chip _chip = _pooled.GetComponentInChildren<Chip>();

        chipList.Add(_chip);

        Vector3 _aimPos = new Vector3(Random.Range(createRange_LeftBottom.position.x, createRange_RightTop.position.x),
            Random.Range(createRange_LeftBottom.position.y, createRange_RightTop.position.y), createRange_LeftBottom.position.z);
        _chip.ShowChip(_aimPos);
    }

    public void GiveChip()
    {
        for (int i = 0; i < chipList.Count; i++)
            chipList[i].BringChip(inactiveChipDelayTime);

        chipList.Clear();
        /*StopAllCoroutines();

        // 보상도중에 칩이 만들어질 경우를 대비해 스냅샷으로 넘김.
        List<Transform> _inactiveChips = new List<Transform>();
        for (int i = 0; i < chipList.Count; i++)
            _inactiveChips.Add(chipList[i]);

        StartCoroutine(GiveChipCor(givePos, _inactiveChips));
        chipList.Clear();*/
    }

    /*IEnumerator GiveChipCor(Vector3 givePos, List<Transform> inactiveChips)
    {
        for (int i = 0; i < inactiveChips.Count; i++)
        {
            Vector3 _bouncePos = transform.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
       
            inactiveChips[i].DOKill();
            inactiveChips[i].DOMove(_bouncePos, 0.5f);
            inactiveChips[i].DOScale(Random.Range(1.4f, 1.5f), 0.7f).SetEase(Ease.OutCubic);
            inactiveChips[i].DOShakeRotation(0.7f, 90, 50);
            inactiveChips[i].GetComponentInChildren<SpriteRenderer>().sortingOrder = 1;
        }

        yield return new WaitForSeconds(0.7f);

        for (int i = 0; i < inactiveChips.Count; i++)
        {
            inactiveChips[i].DOKill();
            inactiveChips[i].DOMove(givePos, 0.3f).SetEase(Ease.OutCubic);
            inactiveChips[i].DOScale(1, 0.3f);
            yield return new WaitForSeconds(Random.Range(0f, 0.05f));
        }
        yield return new WaitForSeconds(0.4f);
        for (int i = 0; i < inactiveChips.Count; i++)
        {
            inactiveChips[i].gameObject.SetActive(false);
            inactiveChips[i].transform.localScale = Vector3.one;
        }
    }*/
}
