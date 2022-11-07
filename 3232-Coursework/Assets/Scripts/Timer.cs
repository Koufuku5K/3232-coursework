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

    void Start()
    {
        currentTime = startTime;
    }

    void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        timer.text = "Time: " + currentTime.ToString("F0");
    }

}
