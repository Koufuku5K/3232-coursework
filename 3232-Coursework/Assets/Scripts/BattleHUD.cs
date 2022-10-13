using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{

    public Slider hpSlider;

    public void HUDSetup(CharacterAttributes attributes)
    {
        hpSlider.maxValue = attributes.maxHP;
        hpSlider.value = attributes.currentHP;
    }

    public void HPSetup(int hp)
    {
        hpSlider.value = hp;
    }
}
