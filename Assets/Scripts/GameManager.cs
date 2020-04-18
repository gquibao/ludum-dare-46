using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public TMP_Text timerText;
    public TMP_Text finalTimerText;
    private int _time = 0;
    private int _minutes = 0;
    private int _seconds = 0;
    private int _spawnTime = 15;
    public GameObject gameOverObject;
    public GameObject waterCubePrefab;
    public GameObject magmaCubePrefab;
    public Transform waterStartingPoint;
    public Transform magmaStartingPoint;
    private Coroutine _countdownCoroutine;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gameOverObject.SetActive(false);
        SpawnCubes();
        _countdownCoroutine = StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        var spawnTimer = 0;
        while (true)
        {
            if (spawnTimer == _spawnTime)
            {
                SpawnCubes();
                spawnTimer = 0;
            }

            spawnTimer++;
            _time++;
            ShowTimer(timerText);
            yield return new WaitForSeconds(1);
        }
    }

    void SpawnCubes()
    {
        Instantiate(magmaCubePrefab, magmaStartingPoint.position, Quaternion.identity);
        Instantiate(waterCubePrefab, waterStartingPoint.position, Quaternion.identity);
    }

    private void ShowTimer(TMP_Text text)
    {
        _minutes = _time / 60;
        _seconds = _time - _minutes * 60;
        text.text = $"{_minutes:00}:{_seconds:00}";
    }

    public void GameOver()
    {
        Cursor.visible = true;
        gameOverObject.SetActive(true);
        ShowTimer(finalTimerText);
        StopCoroutine(_countdownCoroutine);
    }
}
