using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] Button _firstSelected;
    
    public static event Action<bool> PauseEvent;
    
    private void Start()
    {
        _pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }


    public void Pause()
    {
        _pauseMenu.SetActive(!_pauseMenu.activeSelf);
        PauseEvent?.Invoke(_pauseMenu.activeSelf);
        Time.timeScale = _pauseMenu.activeSelf ? 0 : 1;
        if (_pauseMenu.activeSelf)
        {
            _firstSelected.Select();
        }
    }
    
    public void Restart()
    {
        Time.timeScale = 1;
        BackdropHandler.Instance.Require(1f, () => OnFadeOutComplete("Level1"));
    }
    
    public void LoadMainMenu()
    {
        GameData.menuLoadMode = MenuLoadMode.Startup;
        GameData.collectables = 0;
        GameData.deaths = 0;
        GameData.timer = 0;
        Time.timeScale = 1;
        BackdropHandler.Instance.Require(1f, () => OnFadeOutComplete("MainMenu"));
        
    }
    
    private void OnFadeOutComplete(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
