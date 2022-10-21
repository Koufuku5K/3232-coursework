using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{

    public Slider hpSlider;
    public Slider waitSlider;

    public CharacterAttributes attributes;

    float currentTime = 0f;

    void Update()
    {
        incrementWait(currentTime);
    }

    public void HUDSetup(CharacterAttributes attributes)
    {
        hpSlider.maxValue = attributes.maxHP;
        hpSlider.value = attributes.currentHP;

        waitSlider.maxValue = attributes.waitMax;
        waitSlider.value = attributes.currentWait;
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
        Debug.Log("Current Time: " + currentTime);
        Debug.Log("Current value: " + waitSlider.value);    
    }
}
