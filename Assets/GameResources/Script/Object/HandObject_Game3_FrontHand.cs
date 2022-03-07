using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandObject_Game3_FrontHand : HandObject_Game3
{
    public override void SetHand(HandType handType)
    {
        if (handType == HandType.empty)
            PlayRandom();
        else
            StopRandom();

        UpdateFingerObject(handType);
    }
}
