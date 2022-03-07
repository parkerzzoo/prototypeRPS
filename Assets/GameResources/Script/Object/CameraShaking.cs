using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraShaking : Singletone<CameraShaking>
{
    float firstCameraSize;
    Camera mainCamera;

    public float time = 0.4f;
    public float shakePower = 5;
    public int shakeCount = 60;

    [SerializeField] private float focuseCameraSize;

    protected override void Awake()
    {
        base.Awake();
        mainCamera = transform.GetComponent<Camera>();
        firstCameraSize = mainCamera.orthographicSize;
    }

    [ContextMenu("OnEndRound")]
    public void OnEndRound()
    {
        StartCoroutine(OnEndRoundCor());
    }

    IEnumerator OnEndRoundCor()
    {
        mainCamera.DOKill();
        mainCamera.DOOrthoSize(focuseCameraSize, 0.1f);

        yield return new WaitForSeconds(0.1f);

        mainCamera.DOKill();
        mainCamera.DOOrthoSize(firstCameraSize, 1f);
    }

    public void HitTable()
    {
        StartCoroutine(HitTableCor(time, shakePower, shakeCount));
    }

    IEnumerator HitTableCor(float time, float shakePower, int shakeCount)
    {
        mainCamera.transform.DOKill();
        mainCamera.transform.DOShakePosition(time, shakePower, shakeCount);

        mainCamera.DOKill();
        mainCamera.DOOrthoSize(focuseCameraSize, time);

        GameController.Instance.UIControl<UIControl_Game1>().ControlActiveFocusEffect(true);

        yield return new WaitForSeconds(time);

        mainCamera.DOKill();
        mainCamera.DOOrthoSize(firstCameraSize, 1f);

        GameController.Instance.UIControl<UIControl_Game1>().ControlActiveFocusEffect(false);
    }

    void ShakeCamera()
    {
        mainCamera.DOKill();
        mainCamera.DOOrthoSize(focuseCameraSize, 0.1f);
    }
}
