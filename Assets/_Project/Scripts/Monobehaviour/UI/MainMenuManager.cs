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
        var hs = PlayerPrefs.GetFloat("NormalBoiHighscore", 0);
        var hsSB =PlayerPrefs.GetFloat("SlipperyBoiHighscore", 0);
        var hsBB = PlayerPrefs.GetFloat("BouncyBoiHighscore", 0);
        
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
                    _gameEndText.color = Color.white;
                    hs = GameData.timer > hs ? GameData.timer : hs;
                    highscoreInMode = hs;
                    PlayerPrefs.SetFloat("NormalBoiHighscore", highscoreInMode);
                    break;
                case StartMode.Slippery:
                    _gameEndText.color = new Color(0, 0.5f, 1);
                    hsSB = GameData.timer > hsSB ? GameData.timer : hsSB;
                    highscoreInMode = hsSB;
                    PlayerPrefs.SetFloat("SlipperyBoiHighscore", highscoreInMode);
                    break;
                case StartMode.Bouncy:
                    _gameEndText.color = new Color(1, 0.5f, 0);
                    hsBB = GameData.timer > hsBB ? GameData.timer : hsBB;
                    highscoreInMode = hsBB;
                    PlayerPrefs.SetFloat("BouncyBoiHighscore", highscoreInMode);
                    break;
            }
            
            _gameEndTimer.text = "Time : " + TimeSpan.FromSeconds(GameData.timer).ToString("mm':'ss'.'fff");
            _gameEndBestTimer.text = "Best Time : " + TimeSpan.FromSeconds(highscoreInMode).ToString("mm':'ss'.'fff");
            _gameEndDeaths.text = "Deaths : " + GameData.deaths;
            _gameEndCollectables.text = "Watermelons : " + GameData.collectables + " / 9";
            
            _endGameFirstSelected.Select();
        }
        
        if (hs > 0)
        {
            _sliperyBoiStartButton.interactable = true;
        }
        
        if (hsSB > 0)
        {
            _bouncyBoiStartButton.interactable = true;
        }
        
        _normalStartText.text = $"NORMAL BOI - {TimeSpan.FromSeconds(hs).ToString("mm':'ss'.'fff")}";
        _sliperyStartText.text = $"SLIPERY BOI - {TimeSpan.FromSeconds(hsSB).ToString("mm':'ss'.'fff")}";
        _bouncyStartText.text = $"BOUNCY BOI - {TimeSpan.FromSeconds(hsBB).ToString("mm':'ss'.'fff")}";
        
        PlayerPrefs.Save();
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