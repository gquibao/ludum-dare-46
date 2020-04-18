using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public TMP_Text timerText;
    private int _time = 0;
    private int _minutes = 0;
    private int _seconds = 0;
    private Coroutine _countdownCoroutine;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _countdownCoroutine = StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        while (true)
        {
            ShowTimer();
            _time++;
            yield return new WaitForSeconds(1);
        }
    }

    private void ShowTimer()
    {
        _minutes = _time / 60;
        _seconds = _time - _minutes * 60;
        timerText.text = $"{_minutes:00}:{_seconds:00}";
    }

    public void GameOver()
    {
        Cursor.visible = true;
        StopCoroutine(_countdownCoroutine);
    }
}
