using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stats : MonoBehaviour
{
    // The variables to be displayed and for calculations 
    public string spriteName;
    public int level;

    public int move1_damage;

    public int maxHP;
    public int currentHP;

    public bool TakeDamage(int damage)
    {
        if (currentHP - damage < 0)
        {
            currentHP = 0;
            return true;
        } else
        {
            currentHP -= damage;
            return false;
        }
    }

    public void Heal(int heal)
    {
        if (currentHP + heal > maxHP)
        {
            currentHP = maxHP;
        }
        else
        {
            currentHP += heal;
        }
    }

}
