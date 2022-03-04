using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : Singletone<GameController>
{
    [SerializeField] protected UIControl uiControl;
    [SerializeField] protected FlowControl flowControl;
    [SerializeField] protected HandObjectControl handObjectControl;

    public UIControl UIControl()
    {
        return uiControl;
    }

    public T UIControl<T>()
    {
        return (T)Convert.ChangeType(uiControl, typeof(T)); 
    }

    public FlowControl FlowControl()
    {
        return flowControl;
    }

    public T FlowControl<T>()
    {
        return (T)Convert.ChangeType(flowControl, typeof(T));
    }

    public HandObjectControl HandObjectControl()
    {
        return handObjectControl;
    }

    public T HandObjectControl<T>()
    {
        return (T)Convert.ChangeType(handObjectControl, typeof(T));
    }
}
