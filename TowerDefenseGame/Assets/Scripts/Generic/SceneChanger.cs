using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : Singleton<SceneChanger>
{
    public Action OnUnloadSceneAction;

    public void ChangeScene(string _sceneName)
    {
        OnUnloadSceneAction?.Invoke();

        SceneManager.LoadScene(_sceneName);
    }
}
