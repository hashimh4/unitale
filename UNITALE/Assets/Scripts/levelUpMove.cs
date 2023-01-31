using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class levelUpMove : MonoBehaviour
{
    
    public stats playerStats;
    public attackMove leftHandedPunch;

    // Start is called before the first frame update
    void Start()
    {
        // Load in the initial player moves
        playerStats.move1_name = leftHandedPunch.moveName;
        playerStats.move1_damage = leftHandedPunch.moveDamage;
        playerStats.move1_mp = leftHandedPunch.mentalPhysical;

    }

    // Update is called once per frame
    void Update()
    {
        if (playerStats.newMove > 0)
        {
            // Define the skill tree here
            // Replace the current move they have with they one they chose (by loading the correct move prefab)




        }
    }
}

/// TO DO:
/// CODE THE PLAYER MOVES, SKILL TREE AND SIMPLE HEALING
/// CODE THE PLAYER MOVE DIALOGUE
/// DRAW OUT THE MACHINATIONS