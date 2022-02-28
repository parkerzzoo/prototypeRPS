using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singletone<GameController>
{
    [SerializeField] protected UIControl uiControl;
    [SerializeField] protected FlowControl flowControl;
    [SerializeField] protected HandObjectControl handObjectControl;
}
