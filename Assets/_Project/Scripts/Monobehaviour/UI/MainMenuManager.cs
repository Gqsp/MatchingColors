using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Button _sliperyBoiStartButton;
    [SerializeField] Button _bouncyBoiStartButton;
    [SerializeField] Button _endGameFirstSelected;
    
    [SerializeField] TextMeshProUGUI _normalStartText;
    [SerializeField] TextMeshProUGUI _sliperyStartText;
    [SerializeField] TextMeshProUGUI _bouncyStartText;

    [SerializeField] GameObject _mainMenu;
    [SerializeField] GameObject _gameEnd;
    [SerializeField] TextMeshProUGUI _gameEndText;
    [SerializeField] TextMeshProUGUI _gameEndTimer;
    [SerializeField] TextMeshProUGUI _gameEndBestTimer;
    [SerializeField] TextMeshProUGUI _gameEndDeaths;
    [SerializeField] TextMeshProUGUI _gameEndCollectables;
    
    private void Start()
    {
        // Load high scores
        var hsNormal = PlayerPrefs.GetFloat("NormalBoiHighscore", 0);
        var hsSlippery = PlayerPrefs.GetFloat("SlipperyBoiHighscore", 0);
        var hsBouncy = PlayerPrefs.GetFloat("BouncyBoiHighscore", 0);

        if (GameData.menuLoadMode == MenuLoadMode.Startup)
        {
            _mainMenu.SetActive(true);
            _gameEnd.SetActive(false);
        }
        else
        {
            _mainMenu.SetActive(false);
            _gameEnd.SetActive(true);
            var highscoreInMode = 0f;

            switch (GameData.startMode)
            {
                case StartMode.Normal:
                    UpdateModeUI(ref hsNormal, ref highscoreInMode, Color.white);
                    break;
                case StartMode.Slippery:
                    UpdateModeUI(ref hsSlippery, ref highscoreInMode, new Color(0, 0.5f, 1));
                    break;
                case StartMode.Bouncy:
                    UpdateModeUI(ref hsBouncy, ref highscoreInMode, new Color(1, 0.5f, 0));
                    break;
            }

            _gameEndTimer.text = "Time : " + TimeSpan.FromSeconds(GameData.timer).ToString("mm':'ss'.'fff");
            _gameEndBestTimer.text = "Best Time : " + TimeSpan.FromSeconds(highscoreInMode).ToString("mm':'ss'.'fff");
            _gameEndDeaths.text = "Deaths : " + GameData.deaths;
            _gameEndCollectables.text = "Watermelons : " + GameData.collectables + " / 9";
            
            _endGameFirstSelected.Select();
            
        }

        _sliperyBoiStartButton.interactable = hsNormal > 0;
        _bouncyBoiStartButton.interactable = hsSlippery > 0;

        UpdateStartText(hsNormal, hsSlippery, hsBouncy);
        
        PlayerPrefs.Save();
    }

    private void UpdateModeUI(ref float currentHighscore, ref float highscoreInMode, Color textColor)
    {
        _gameEndText.color = textColor;
        highscoreInMode = currentHighscore != 0 ? Mathf.Min(GameData.timer, currentHighscore) : GameData.timer;
        if (!Mathf.Approximately(highscoreInMode, currentHighscore))
        {
            PlayerPrefs.SetFloat(GetModeKey(GameData.startMode), highscoreInMode);
            currentHighscore = highscoreInMode;
        }
    }

    private void UpdateStartText(float hs, float hsSB, float hsBB)
    {
        _normalStartText.text = $"NORMAL BOI - {TimeSpan.FromSeconds(hs):mm':'ss'.'fff}";
        _sliperyStartText.text = $"SLIPERY BOI - {TimeSpan.FromSeconds(hsSB):mm':'ss'.'fff}";
        _bouncyStartText.text = $"BOUNCY BOI - {TimeSpan.FromSeconds(hsBB):mm':'ss'.'fff}";
    }

    private static string GetModeKey(StartMode mode)
    {
        return mode switch
        {
            StartMode.Normal => "NormalBoiHighscore",
            StartMode.Slippery => "SlipperyBoiHighscore",
            StartMode.Bouncy => "BouncyBoiHighscore",
            _ => ""
        };
    }

    public void StartGame(int mode)
    {
        GameData.startMode = (StartMode)mode;
        StartCoroutine(StartSequence());
    }

    public void Restart()
    {
        StartCoroutine(StartSequence());
    }
    
    private bool _backdropActive;
    
    private IEnumerator StartSequence()
    {
        BackdropHandler.Instance.Require(0.5f, () => _backdropActive = true);
        
        while (!_backdropActive)
        {
            yield return null;
        }
        
        SceneManager.LoadScene("Level1");
    }
}

[Serializable]
public enum StartMode
{
    Normal,
    Slippery,
    Bouncy
}