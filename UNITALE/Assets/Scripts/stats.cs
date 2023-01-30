using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stats : MonoBehaviour
{
    // The variables to be displayed and for calculations 
    public string spriteName;

    public int level;
    public int xp;
    public int xpMax;
    public int newMove;

    public string move1_name;
    public int move1_damage;

    public string move2_name;
    public int move2_damage;

    public string move3_name;
    public int move3_damage;

    public string move4_name;
    public int move4_damage;

    public int maxHP;
    public int currentHP;

    public bool TakeDamage(int damage)
    {
        if (currentHP - damage <= 0)
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

    public void LevelUp(int experience)
    {
        // Perform the xp calculation based off the level of the opponent
        int xpCalculation = experience;
        // Calculate the new total XP score
        xp = xp + xpCalculation;

        // Level up when the XP is greater than the maximum XP
        while (xp >= xpMax)
        {
            // Calculate the current XP in this level bracket
            xp = xp - xpMax;
            // Calculate the maximum XP for the next level
            xpMax = xpMax * 2;
            // Increase the level by one
            level += 1;
            // Increase the new move counter by one
            newMove += 1;
        }
    }

    public void NewMove()
    {
        if (newMove > 0)
        {
            // (Perhaps do this all in the other script)
            // Show the moves the player can select depending on the skill tree below
            // Let them choose in the interface which move they want and which move to replace it with
            // Replace the current move they have with they one they chose (by loading the correct move prefab)

            // Run a newMove script if they level up, and then have all the move prefab links there 

        }
    }

}
