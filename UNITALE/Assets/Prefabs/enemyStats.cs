using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class enemyStats : MonoBehaviour
{
    // The variables to be displayed and for caulations
    public string spriteName;

    // Variables related to the level and XP
    public int level;
    public int xp;
    public int xpMax;
    public int newMove;

    // Variables related to the four moves each character can have
    public string move1_name;
    public int move1_damage;
    // Whether a move is mentally or physically geared
    public bool move1_mp;
    public string move1_response;

    public string move2_name;
    public int move2_damage;
    public bool move2_mp;
    public string move2_response;

    public string move3_name;
    public int move3_damage;
    public bool move3_mp;
    public string move3_response;

    public string move4_name;
    public int move4_damage;
    public bool move4_mp;
    public string move4_response;

    // The number of USBs the player has
    public int USB;

    // Variables related to HP
    public int maxHP;
    public int currentHP;

    // Mental-physical stat - a number between -1 and 1
    // -1 means a more mentally skilled character
    // 1 means a more physically skilled character
    public float mentalPhysical;

    public bool TakeDamage(int damage, bool moveType, float victimMP)
    {
        int moveTypeMultiplier;
        int finalDamage;

        if (moveType)
        {
            // The move is a physical attack
            moveTypeMultiplier = 1;
        }
        else
        {
            // The move is a mental attack
            moveTypeMultiplier = -1;
        }

        // A score of 0 will do normal damage
        // A negative score will do more damage
        // A positive score will do less damage
        float victimCalc = moveTypeMultiplier + victimMP;

        // Create a random multiplier 
        float randomMult = UnityEngine.Random.Range(0.8f, 1.2f);

        // Calculate the damage
        float damageCalc = victimCalc * (damage / 4f) * randomMult;

        // If the damage is less than zero, set it to one
        if ((damageCalc + damage) < 0)
        {
            finalDamage = 1;
        }
        else
        {
            // Round the damage, so that it is an integer value
            finalDamage = (int)Math.Round(damageCalc + damage);
        }

        // Adjust the final HP
        if (currentHP - finalDamage <= 0)
        {
            // Set the final HP to zero if it goes below zero
            currentHP = 0;
            // Notify the system that the player / enemy has died
            return true;
        }
        else
        {
            // Otherwise subtract the correct value
            currentHP -= finalDamage;
            return false;
        }
    }

    public int Heal(int heal)
    {
        if (currentHP + heal > maxHP)
        {
            int difference = maxHP - currentHP;
            currentHP = maxHP;
            return difference;
        }
        else
        {
            currentHP += heal;
            return heal;
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
            if (xpMax < 30)
            {
                xpMax += 5;
            }
            else if (30 <= xpMax && xpMax < 120)
            {
                xpMax += 10;
            }
            else
            {
                xpMax += 20;
            }

            // Calculate the maximum HP for the next level
            if (maxHP < 30)
            {
                maxHP += 5;
            }
            else if (30 <= maxHP && maxHP < 120)
            {
                maxHP += 10;
            }
            else
            {
                maxHP += 20;
            }

            // Increase the level by one
            level += 1;
            // Increase the new move counter by one
            newMove += 1;
        }
    }


}
