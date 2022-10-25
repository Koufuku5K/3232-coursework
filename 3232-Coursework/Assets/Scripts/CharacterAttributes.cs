using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttributes : MonoBehaviour
{
    public int maxHP;
    public int currentHP;

    public int waitMax;
    public int currentWait;

    public int limitMax;
    public int currentLimit;

    public bool immune = false;

    public int normalAttackDamage;
    public int limitDamage;

    public bool takeDamage(int dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0)
        {
            // return true if enemy is dead
            return true;
        }
        else
        {
            // return false if enemy is not dead
            return false;
        }
    }

}
