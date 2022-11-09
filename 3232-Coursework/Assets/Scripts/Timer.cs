using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float currentTime;
    public float startTime;
    public TextMeshProUGUI timer;

    public GameObject enemyPrefab;
    public Transform enemySpawnPoint;
    public GameObject bossSpawnText;

    bool bossSpawned = false;

    void Start()
    {
        currentTime = startTime;
        timer.text = "Time: " + currentTime.ToString("F0");
    }

    void Update()
    {
        if (currentTime <= 5)
        {
            bossSpawnText.SetActive(true);
        }
        if (currentTime >= 0)
        {
            currentTime -= 1 * Time.deltaTime;
            timer.text = "Time: " + currentTime.ToString("F0");
        }
        else
        {
            // Spawn the Boss in the middle of the screen
            if (bossSpawned == false)
            {
                GameObject enemyObject = Instantiate(enemyPrefab, enemySpawnPoint);
            }
            bossSpawned = true;
        }
    }
}
