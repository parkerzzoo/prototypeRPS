using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Load : MonoBehaviour 
{
    public string nextSceneName;
    public bool goOnStart = true;
    public bool isAsync = false;
    public float loadSec;

    AsyncOperation async;

    void Start()
    {
        //UnityEditor.PlayerSettings.WebGL.threadsSupport = true;
        Application.targetFrameRate = -1;
        if( goOnStart ) 
        {
            if( isAsync ) 
            {
                StartCoroutine( LoadSceneAsync());
            } 
            else 
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }

    IEnumerator LoadSceneAsync()
    {
        bool _loaded = false;

        if (!_loaded)
            yield return null;

        async = SceneManager.LoadSceneAsync(nextSceneName);
        float progress = async.progress;
        while (progress < 0.9f)
        {
            yield return null;
            progress = async.progress;
        }
        yield return new WaitForSeconds( loadSec );
        async.allowSceneActivation = true;
    }
}
