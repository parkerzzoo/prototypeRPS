using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControl_Login : UIControl
{
    public void OnClickLogin()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
    }
}
