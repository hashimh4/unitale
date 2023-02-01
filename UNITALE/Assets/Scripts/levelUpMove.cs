using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.Linq;

public class levelUpMove : MonoBehaviour
{
    // Reference to the player movement script
    public playerMovement movementScript;
    // Reference to the game state
    public battleSystem battleScript;

    // Reference to the battle UI object
    public GameObject levelUpInterface;
    // Reference to all the attack buttons object
    public GameObject newAttackButtons;
    // Reference to the dialogue box
    public GameObject originalAttackButtons;

    // Reference the stats of the player
    public stats playerStats;

    // The dialogue to be loaded on the UI
    public TMP_Text text;
    // The player level information to be loaded
    public TMP_Text playerLevelText;
    // The player HP information to be loaded 
    public TMP_Text playerHPText;
    // The player XP information to be loaded
    public TMP_Text playerXPText;
    // The number of USBs the player has
    public TMP_Text playerUSBs;

    // The text of the new attack names to be loaded
    public TMP_Text newAttack1Text;
    public TMP_Text newAttack2Text;
    public TMP_Text newAttack3Text;
    public TMP_Text newAttack4Text;
    // The text of the original attack names to be loaded
    public TMP_Text attack1Text;
    public TMP_Text attack2Text;
    public TMP_Text attack3Text;
    public TMP_Text attack4Text;

    // Reference all the possible moves they could pick up
    public attackMove punch;
    public attackMove poke;
    public attackMove kick;
    public attackMove point;
    public attackMove starJump;
    public attackMove shout;
    public attackMove leftHandedPunch;
    public attackMove bubbles;
    public attackMove calculus;
    public attackMove garry;
    public attackMove flute;
    public attackMove compliment;
    public attackMove rain;
    public attackMove theBirds;
    public attackMove wink;
    public attackMove thunderbolt;
    public attackMove fireball;
    public attackMove nunchucks;
    public attackMove hug;

    // Whether the loop has already run
    private bool hasRun = false;
    private int selection;
    private int selectionOld;
    private int newPosition;
    // The speed of the text to be loaded, set to 0.05 seconds
    private float dialogueSpeed = 0.02F;

    // Start is called before the first frame update
    void Start()
    {
        // Load in the initial player moves
        playerStats.move1_name = punch.moveName;
        playerStats.move1_damage = punch.moveDamage;
        playerStats.move1_mp = punch.mentalPhysical;
        playerStats.move1_response = punch.moveResponse;

        playerStats.move2_name = poke.moveName;
        playerStats.move2_damage = poke.moveDamage;
        playerStats.move2_mp = poke.mentalPhysical;
        playerStats.move2_response = poke.moveResponse;

        playerStats.move3_name = kick.moveName;
        playerStats.move3_damage = kick.moveDamage;
        playerStats.move3_mp = kick.mentalPhysical;
        playerStats.move3_response = kick.moveResponse;

        playerStats.move4_name = point.moveName;
        playerStats.move4_damage = point.moveDamage;
        playerStats.move4_mp = point.mentalPhysical;
        playerStats.move4_response = point.moveResponse;
    }

    // Update is called once per frame
    void Update()
    {
        attackMove[] l1 = { starJump, shout, leftHandedPunch, bubbles }; // The moves for level 1
        attackMove[] l2 = { calculus, garry, flute, compliment };
        attackMove[] l3 = { rain, theBirds, wink, thunderbolt };
        attackMove[] l4 = { fireball, nunchucks, hug };
        attackMove[][] moveListofLists = { l1, l2, l3, l4 };
        int moveSet = 0;
        if (playerStats.level > 1)
        {
            moveSet = playerStats.level - 2;
        }
        // The list of possible moves the player can obtain in the game
        attackMove[] newMoveList = moveListofLists[moveSet];

        if (playerStats.newMove > 0 && hasRun == false && battleScript.gameState != BattleState.LOST) // And as long as there are four moves in the moves list
        {
            // Stop the player from moving
            movementScript.canMove = false;
            // Notify the script that this loop has already run
            hasRun = true;
            // Reset the selection to 0
            selection = 0;
            newPosition = 0;
            // Disable the original buttons
            originalAttackButtons.SetActive(false);
            // Make the interface appear
            levelUpInterface.SetActive(true);

            // Load the player level on the screen
            playerLevelText.text = "Level " + playerStats.level;
            // Load the player's HP on the screen
            playerHPText.text = "HP - " + playerStats.currentHP + "/" + playerStats.maxHP;
            // Load the player's XP on the screen
            playerXPText.text = playerStats.xp + "/" + playerStats.xpMax;
            // Load the original names of the attack text
            attack1Text.text = playerStats.move1_name;
            attack2Text.text = playerStats.move2_name;
            attack3Text.text = playerStats.move3_name;
            attack4Text.text = playerStats.move4_name;

            // Load the number of player USBs on the screen
            //playerUSBs.text = playerStats.USB.ToString();

            // Load the CORRECT next four attacks
            newAttack1Text.text = newMoveList[0].moveName;
            newAttack2Text.text = newMoveList[1].moveName;
            newAttack3Text.text = newMoveList[2].moveName;
            newAttack4Text.text = newMoveList[3].moveName;

            // Output the text notifying the user to decide on a new move
            text.text = string.Empty;
            string[] startLine = {"Choose a new action which speaks to your soul. Some moves may set you back and perform less damage than you intend. " +
                "Decide carefully as the fate of the world lies in your hands." };
            StartCoroutine(TypeLine(startLine));

            StartCoroutine(ButtonsAppear());
        }

        if (selection != 0)
        {
            if (selection == 1)
            {
                StartCoroutine(TypeLine(newMoveList[0].moveDescription));

                string newMoveName = newMoveList[0].moveName;
                int newMoveDamage = newMoveList[0].moveDamage;
                bool newMoveMP = newMoveList[0].mentalPhysical;
                string newMoveResponse = newMoveList[0].moveResponse;
            }
            if (selection == 2)
            {
                StartCoroutine(TypeLine(newMoveList[1].moveDescription));

                string newMoveName = newMoveList[1].moveName;
                int newMoveDamage = newMoveList[1].moveDamage;
                bool newMoveMP = newMoveList[1].mentalPhysical;
                string newMoveResponse = newMoveList[1].moveResponse;
            }
            if (selection == 3)
            {
                StartCoroutine(TypeLine(newMoveList[2].moveDescription));

                string newMoveName = newMoveList[2].moveName;
                int newMoveDamage = newMoveList[2].moveDamage;
                bool newMoveMP = newMoveList[2].mentalPhysical;
                string newMoveResponse = newMoveList[2].moveResponse;
            }
            if (selection == 4)
            {
                StartCoroutine(TypeLine(newMoveList[3].moveDescription));

                string newMoveName = newMoveList[3].moveName;
                int newMoveDamage = newMoveList[3].moveDamage;
                bool newMoveMP = newMoveList[3].mentalPhysical;
                string newMoveResponse = newMoveList[3].moveResponse;
            }
            newPosition = selection - 1;
            selection = 0;

            // Deactivate the new attack buttons
            newAttackButtons.SetActive(false);
            // Activate the current stored moves
            originalAttackButtons.SetActive(true);
            StartCoroutine(AfterFirstSelection());
        }

        if (selectionOld != 0)
        {
            if (selectionOld == 1)
            {
                playerStats.move1_name = newMoveList[newPosition].moveName;
                playerStats.move1_damage = newMoveList[newPosition].moveDamage;
                playerStats.move1_mp = newMoveList[newPosition].mentalPhysical;
                playerStats.move1_response = newMoveList[newPosition].moveResponse;
            }
            if (selectionOld == 2)
            {
                playerStats.move2_name = newMoveList[newPosition].moveName;
                playerStats.move2_damage = newMoveList[newPosition].moveDamage;
                playerStats.move2_mp = newMoveList[newPosition].mentalPhysical;
                playerStats.move2_response = newMoveList[newPosition].moveResponse;
            }
            if (selectionOld == 3)
            {
                playerStats.move3_name = newMoveList[newPosition].moveName;
                playerStats.move3_damage = newMoveList[newPosition].moveDamage;
                playerStats.move3_mp = newMoveList[newPosition].mentalPhysical;
                playerStats.move3_response = newMoveList[newPosition].moveResponse;
            }
            if (selectionOld == 4)
            {
                playerStats.move4_name = newMoveList[newPosition].moveName;
                playerStats.move4_damage = newMoveList[newPosition].moveDamage;
                playerStats.move4_mp = newMoveList[newPosition].mentalPhysical;
                playerStats.move4_response = newMoveList[newPosition].moveResponse;
            }
            // Deactivate the dialogue box
            originalAttackButtons.SetActive(false);
            // Make the interface disappear
            levelUpInterface.SetActive(false);
            selectionOld = 0;
            hasRun = false;
            playerStats.newMove += -1;
            // Let the player move again
            movementScript.canMove = true;
        }
    }

    // The method to display each character in a sentence one by one, when it is loaded
    IEnumerator TypeLine(string[] dialogueText)
    {
        foreach (string i in dialogueText)
        {
            text.text = string.Empty;
            foreach (char c in i.ToCharArray())
            {
                text.text += c;
                // Set the time to wait for each character to be displayed
                yield return new WaitForSeconds(dialogueSpeed);
            }
            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator AfterFirstSelection()
    {
        yield return new WaitForSeconds(9f);
        string[] endLine = { "Choose the move you would like to replace..." };
        StartCoroutine(TypeLine(endLine));
    }

    IEnumerator ButtonsAppear()
    {
        yield return new WaitForSeconds(4.5f);
        // Activate the attack buttons
        newAttackButtons.SetActive(true);
    }

    public void FirstNewAttack()
    {
        selection = 1;
    }
    public void SecondNewAttack()
    {
        selection = 2;
    }
    public void ThirdNewAttack()
    {
        selection = 3;
    }
    public void FourthNewAttack()
    {
        selection = 4;
    }

    public void FirstOldAttack()
    {
        selectionOld = 1;
    }
    public void SecondOldAttack()
    {
        selectionOld = 2;
    }
    public void ThirdOldAttack()
    {
        selectionOld = 3;
    }
    public void FourthOldAttack()
    {
        selectionOld = 4;
    }

}

/// TO DO:
/// CODE THE PLAYER MOVES, SKILL TREE AND SIMPLE HEALING
/// CODE THE PLAYER MOVE DIALOGUE
/// DRAW OUT THE MACHINATIONS