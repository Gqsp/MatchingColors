using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerMonoBehavior : MonoBehaviour
{
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    protected virtual void OnGameModeStart()
    {

    }

    protected virtual void OnSavePicked()
    {

    }

    protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {

    }

    protected virtual void OnSceneUnloaded(Scene scene)
    {

    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

}
