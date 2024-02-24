using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeManager : MonoBehaviour
{
    private bool _isSwitching;
    private IGameMode _currentMode;

    public static RegularMode RegularMode;
    public static MainMenuMode MainMenuMode;
    public static MatrixMode MatrixMode;

    private void Awake()
    {
        Time.timeScale = 0;

#if UNITY_EDITOR
        switch(SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                HandleStartRequested(MainMenuMode);
                break;
            case 1:
                _currentMode = MainMenuMode;
                _currentMode.OnEditorStart();
                SceneManager.LoadScene(0, LoadSceneMode.Additive);
                break;
            default:
                App.IsEditor = true;
                _currentMode = RegularMode;
                _currentMode.OnEditorStart();
                SceneManager.LoadScene(0, LoadSceneMode.Additive);
                break;
        }
#else
    HandleStartRequested(MainMenuMode);
#endif
    }

    public void HandleStartRequested(IGameMode mode)
    {

    }

    private IEnumerator SwitchMode(IGameMode mode)
    {
        yield return new WaitUntil(() => !_isSwitching);
        if (_currentMode == mode) yield break;

        _isSwitching = true;
        yield return App.Backdrop.Require();

        if (_currentMode != null)
            yield return _currentMode.OnEnd();
        _currentMode = mode;
        yield return _currentMode.OnStart();

        yield return App.Backdrop.Release();
        _isSwitching = false;
    }
}
