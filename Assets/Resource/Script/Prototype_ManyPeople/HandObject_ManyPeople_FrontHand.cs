using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandObject_ManyPeople_FrontHand : HandObject_ManyPeople
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
