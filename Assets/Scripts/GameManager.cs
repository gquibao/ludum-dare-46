using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public TMP_Text timerText;
    public TMP_Text finalTimerText;
    private int _time = 0;
    private int _minutes = 0;
    private int _seconds = 0;
    public int _spawnTime = 15;
    public int groundAmount = 4;
    public GameObject gameOverObject;
    public GameObject tutorialCanvas;
    public GameObject waterCubePrefab;
    public GameObject waterGroundPrefab;
    public GameObject magmaCubePrefab;
    public GameObject magmaGroundPrefab;
    public List<GameObject> magmaTiles;
    public List<GameObject> waterTiles;
    public Transform[] groundPositons;
    private Coroutine _countdownCoroutine;

    private void Awake()
    {
        instance = this;
    }

    private IEnumerator Start()
    {
        SpawnGround();
        tutorialCanvas.SetActive(true);
        yield return new WaitForSeconds(3);
        Destroy(tutorialCanvas, 5);
        gameOverObject.SetActive(false);
        StartCoroutine(SpawnElementals());
        _countdownCoroutine = StartCoroutine(Countdown());
    }

    private void SpawnGround()
    {
        var magma = 0;
        var water = 0;
        foreach (var groundPosition in groundPositons)
        {
            var random = Random.Range(0, 2);
            if (random == 0)
            {
                if (magma < groundAmount)
                {
                    magmaTiles.Add(Instantiate(magmaGroundPrefab, groundPosition.position, Quaternion.identity));
                    magma++;
                }
                else
                {
                    waterTiles.Add(Instantiate(waterGroundPrefab, groundPosition.position, Quaternion.identity));
                    water++;
                }
            }
            else
            {
                if (water < groundAmount)
                {
                    waterTiles.Add(Instantiate(waterGroundPrefab, groundPosition.position, Quaternion.identity));
                    water++;
                }
                else
                {
                    magmaTiles.Add(Instantiate(magmaGroundPrefab, groundPosition.position, Quaternion.identity));
                    magma++;
                }
            }
        }
    }

    IEnumerator Countdown()
    {
        var spawnTimer = 0;
        while (true)
        {
            if (spawnTimer == _spawnTime)
            {
                StartCoroutine(SpawnElementals());
                spawnTimer = 0;
            }

            spawnTimer++;
            _time++;
            ShowTimer(timerText);
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator SpawnElementals()
    {
        var random = Random.Range(0, groundAmount);
        var timeBetweenSpawn = Random.Range(0, 3);
        Instantiate(magmaCubePrefab, magmaTiles[random].transform.position, Quaternion.identity);
        yield return new WaitForSeconds(timeBetweenSpawn);
        Instantiate(waterCubePrefab, waterTiles[random].transform.position, Quaternion.identity);
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
