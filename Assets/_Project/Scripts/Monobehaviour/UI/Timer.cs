using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    
    public static Action<bool> PauseEvent;
    
    private bool _isPaused;

    public float ElapsedTime { get; private set; }

    private void Start()
    {
        PauseEvent += PauseTimer;
    }

    private void OnDestroy()
    {
        PauseEvent -= PauseTimer;
    }

    void Update()
    {
        if (!_isPaused)
        {
            ElapsedTime += Time.deltaTime;
            UpdateTimerText();
        }
    }

    void UpdateTimerText()
    {
        float minutes = Mathf.FloorToInt(ElapsedTime / 60);
        float seconds = Mathf.FloorToInt(ElapsedTime % 60);
        float milliseconds = Mathf.FloorToInt(ElapsedTime * 1000 % 1000);

        string formattedTime = $"{minutes:00}:{seconds:00}.<size=80%><color=grey>{milliseconds:000}</color></size>";

        timerText.text = formattedTime;
    }

    public void PauseTimer(bool pause)
    {
        _isPaused = pause;
    }
}