using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControl_Lobby : UIControl
{
    public void OnClickGame1Start()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("InGame_FewPeople");
    }

    public void OnClickGame2Start()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("InGame_Game2");
    }

    public void OnClickGame3Start()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("InGame_ManyPeople");
    }
}
