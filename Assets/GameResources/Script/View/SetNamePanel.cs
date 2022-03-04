using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetNamePanel : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button startButton;

    private void Start()
    {
        CheckName(null);
    }

    public void CheckName(string name)
    {
        startButton.interactable = !string.IsNullOrEmpty(name);
    }
}
