using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZObjectPools;

public class ChatPanel : MonoBehaviour
{
    public TMPro.TMP_InputField chatInputField;
    public EZObjectPool chatObjectPool;

    public Transform left1;
    public Transform left2;
    public Transform right1;
    public Transform right2;

    public void AddChat(string content)
    {
        Transform _posLeftBottom;
        Transform _posRightTop;
        bool _isLeft = Random.Range(0, 2) == 0;

        _posLeftBottom = _isLeft ? left1 : right1;
        _posRightTop = _isLeft ? left2 : right2;

        Vector3 _randomPos = new Vector3(
            Random.Range(_posLeftBottom.localPosition.x, _posRightTop.localPosition.x),
            Random.Range(_posLeftBottom.localPosition.y, _posRightTop.localPosition.y), _posLeftBottom.localPosition.z);

        GameObject _pooled;
        chatObjectPool.TryGetNextObject(Vector3.one, Quaternion.identity, out _pooled);

        _pooled.transform.localPosition = _randomPos;
        _pooled.transform.localScale = Vector3.one;

        ChatObject _chat = _pooled.GetComponent<ChatObject>();
        _chat.Init(content);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            GameController.Instance.FlowControl<FlowControl_ManyPeople>().OnSendChat(chatInputField.text);
            chatInputField.ActivateInputField();
            chatInputField.text = "";
        }
    }
}
