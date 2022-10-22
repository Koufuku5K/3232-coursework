using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{

    public Slider hpSlider;
    public Slider waitSlider;
    public Slider limitSlider;

    public CharacterAttributes charAttributes;

    float currentTime = 0f;

    void Update()
    {
        incrementWait(currentTime);
    }

    public void playerHUDSetup(CharacterAttributes attributes)
    {
        hpSlider.maxValue = attributes.maxHP;
        hpSlider.value = attributes.currentHP;

        waitSlider.maxValue = attributes.waitMax;
        waitSlider.value = attributes.currentWait;

        limitSlider.maxValue = attributes.limitMax;
        limitSlider.value = attributes.currentLimit;
    }

    public void HPSetup(int hp)
    {
        hpSlider.value = hp;
    }

    public void waitSetup(int wait)
    {
        waitSlider.value = wait;
    }

    public void limitSetup(int limit)
    {
        limitSlider.value = limit;
    }

    public void incrementWait(float currentTime)
    {
        currentTime += 1 * Time.deltaTime;
        waitSlider.value += currentTime;
        limitSlider.value += currentTime;
    }
}
