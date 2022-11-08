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

    void Start()
    {
        currentTime = startTime;
        timer.text = "Time: " + currentTime.ToString("F0");
    }

    void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        timer.text = "Time: " + currentTime.ToString("F0");

        /*if (currentTime <= 0)
        { 
            // Spawn the Boss in the middle of the screen
            GameObject enemyObject = Instantiate(enemyPrefab, enemySpawnPoint);
        }*/
    }

}
