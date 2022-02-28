using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControl_Lobby : UIControl
{
    public void OnClickBasicModeStart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("InGame_BasicMode");
    }

    public void OnClickCardModeStart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("InGame_CardMode");
    }
}
