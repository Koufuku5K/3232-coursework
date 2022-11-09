using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHUD : MonoBehaviour
{
    public bool enemyDead;

    public Slider hpSlider;
    public Slider waitSlider;

    public EnemyAttributes enemyAttributes;

    float currentTime = 0f;

    void Update()
    {
        incrementWait(currentTime);
    }

    public void enemyHUDSetup(EnemyAttributes enemyAttributes)
    {
        hpSlider.maxValue = enemyAttributes.maxHP;
        hpSlider.value = enemyAttributes.currentHP;

        waitSlider.maxValue = enemyAttributes.waitMax;
        waitSlider.value = enemyAttributes.currentWait;
    }

    public void HPSetup(int hp)
    {
        hpSlider.value = hp;
    }

    public void waitSetup(int wait)
    {
        waitSlider.value = wait;
    }

    public void incrementWait(float currentTime)
    {
        currentTime += 1 * Time.deltaTime;
        waitSlider.value += currentTime;
    }
}
