using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController_Lobby : UIController
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
