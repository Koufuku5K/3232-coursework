using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{

    public Slider hpSlider;
    public Slider waitSlider;

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
}
