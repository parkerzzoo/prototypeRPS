using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController_Login : UIController
{
    public void OnClickLogin()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
    }
}
