﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

// To define the different game states
public enum BattleState { NOBATTLE, START, PLAYERTURN, ENEMYTURN, WON, LOST }
public class battleSystem : MonoBehaviour
{
    // Making reference to the player stats, so these can be updated
    public GameObject playerPrefab;
    // The enemy prefab, which contains all the correct stats/moves and enemy sprite
    public GameObject enemyPrefab;
    // The object holding the location for the enemy sprite
    public Transform enemyLocation;

    // Reference to the battle UI object
    public GameObject battleInterface;
    // Reference to the attack/item buttons object
    public GameObject attackItemButtons;
    // Reference to all the attack buttons object
    public GameObject attackButtons;
    // Reference to the dialogue box
    public GameObject dialogueBox;

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
    // The enemy name to be loaded
    public TMP_Text enemyNameText;
    // The enemy HP information to be loaded 
    public TMP_Text enemyHPText;

    // The text of the attack names to be loaded
    public TMP_Text attack1Text;
    public TMP_Text attack2Text;
    public TMP_Text attack3Text;
    public TMP_Text attack4Text;

    // The speed of the text to be loaded, set to 0.05 seconds
    private float dialogueSpeed = 0.05F;
    // Whether the InitiateBattle function has already run
    private bool hasRun = false;
    // Whether the player has already chosen their actioon
    private bool actionChosen = false;

    // To notify the system whether the game should be over
    public bool endGame = false;

    // To store the stats of the player and enemy
    stats playerStats;
    stats enemyStats;

    // Reference to the battle camera script to change the camera if there is a battle
    public battleCameraTransition battleCameraScript;
    // Reference to the player movement script
    public playerMovement playerMovementScript;
    // Making reference to the game state, so that it can be changed
    public BattleState gameState;

    // The game over screen
    public GameObject gameOverScreen;

    // Start is called before the first frame update
    private void Start()
    {
        gameState = BattleState.NOBATTLE;
    }

    void Update()
    {
        // Ensure we enter the start state when entering a battle
        if (gameState == BattleState.START && hasRun == false) {
            hasRun = true;
            battleCameraScript.isBattle = true;
            StartCoroutine(InitiateBattle());
        }
    }
    
    // The player specific UI that needs to be changed throughout the battle
    void PlayerUI()
    {
        // Load the player level on the screen
        playerLevelText.text = "Level " + playerStats.level;
        // Load the player's HP on the screen
        playerHPText.text = "HP - " + playerStats.currentHP + "/" + playerStats.maxHP;
        // Load the player's XP on the screen
        playerXPText.text = playerStats.xp + "/" + playerStats.xpMax;
        // Load the names of the attack text
        attack1Text.text = playerStats.move1_name;
        attack2Text.text = playerStats.move2_name;
        attack3Text.text = playerStats.move3_name;
        attack4Text.text = playerStats.move4_name;
        // Load the number of player USBs on the screen
        playerUSBs.text = "USBs - " + playerStats.USB.ToString();
    }

    // The enemy specific UI that needs to be changed throughout the battle
    void EnemyUI()
    {
        // Load the enemy's name on the screen
        enemyNameText.text = enemyStats.spriteName;
        // Load the enemy's HP on the screen
        enemyHPText.text = "HP - " + enemyStats.currentHP + "/" + enemyStats.maxHP;
    }

    // The START battle state
    IEnumerator InitiateBattle()
    {
        // Display the battle interface
        battleInterface.SetActive(true);
        // Activate the dialogue box
        dialogueBox.SetActive(true);

        // Retrieve the player stats and store these
        playerStats = playerPrefab.GetComponent<stats>();

        // Load the enemy in the correct location
        GameObject enemyInitiate = Instantiate(enemyPrefab, enemyLocation);
        // Retrieve the enemy stats and store these
        enemyStats = enemyInitiate.GetComponent<stats>();

        // Load the dialogue lines on the screen
        text.text = string.Empty;
        System.Random rnd = new System.Random();
        int randomText = rnd.Next(1, 6);
        string[] initialText = { enemyStats.spriteName + " readies up...", enemyStats.spriteName + " takes a deep breath.", 
            enemyStats.spriteName + " is thinking deeply.", enemyStats.spriteName + " looks nervous.", 
            enemyStats.spriteName + " prepares themselves.", enemyStats.spriteName + " looks serious." };
        string[] initial = { initialText[randomText - 1] };
        StartCoroutine(TypeLine(initial));

        // Update the UI on the screen
        PlayerUI();
        EnemyUI();

        // Define the player's mental-physical stat based on the moves they currently have
        playerStats.mentalPhysical = (Convert.ToInt32(playerStats.move1_mp) + Convert.ToInt32(playerStats.move2_mp)
            + Convert.ToInt32(playerStats.move3_mp) + Convert.ToInt32(playerStats.move4_mp) - 2) / 2;

        // DIALOGUE //////////////////////////////////////////////////////////
        yield return new WaitForSeconds(3f);

        // Change the game state to the correct turn ////////////////////////////////////
        gameState = BattleState.PLAYERTURN;
        StartCoroutine(PlayerTurn());
    }

    // The method to display each character in a sentence one by one, when it is loaded
    IEnumerator TypeLine(string[] dialogueText)
    {
        // Clears the text on the screen
        text.text = string.Empty;

        // Go through each string passed to the function
        foreach (string i in dialogueText) {
            // Go through each character of each string
            foreach (char c in i.ToCharArray())
            {
                // Print the character
                text.text += c;
                // Set the time to wait for each character to be displayed
                yield return new WaitForSeconds(dialogueSpeed);
            }

            // Ensures the final sentence stays on screen
            if (i != dialogueText.Last())
            {
                text.text = string.Empty;
            }
        }
    }


    // If the player chooses to attack on their turn
    IEnumerator Attack(int move_damage, bool move_type, float victim_mp, string move_reponse)
    {
        // Update the enemy stats
        bool dead = enemyStats.TakeDamage(move_damage, move_type, victim_mp);

        // Update the enemy UI details
        EnemyUI();
        // Dialogue line
        string[] playerAttackLines = { enemyStats.spriteName + " " + move_reponse};
        StartCoroutine(TypeLine(playerAttackLines));

        yield return new WaitForSeconds(5f);

        // Check whether the enemy is dead
        if(dead)
        {
            // The battle is won when the enemy is dead
            gameState = BattleState.WON;
            StartCoroutine(EndBattle());

        } else
        {
            // Continue the battle and go to the enemy's turn if they are not dead
            gameState = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator Item()
    {
        // Heal the player by 30 HP
        int difference = playerStats.Heal(30);

        // Update the player UI
        PlayerUI();

        // Dialogue line
        string[] playerItemLines = { "You heal yourself by " + difference + "HP."};
        StartCoroutine(TypeLine(playerItemLines));

        yield return new WaitForSeconds(2.5f);

        // Continue the battle and change the state to the enemy's turn
        gameState = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    // The enemy AI
    IEnumerator EnemyTurn()
    {
        // Reset the variable for the next player turn, so they can choose an action again
        actionChosen = false;

        // Random move between 1 and 4
        System.Random rnd = new System.Random();
        int enemyMove = rnd.Next(1, 4);

        bool dead = false;

        if (enemyMove == 1)
        {
            dead = playerStats.TakeDamage(enemyStats.move1_damage, enemyStats.move1_mp, playerStats.mentalPhysical);
            // Dialogue Line
            string[] enemyAttackLines = { enemyStats.spriteName + " uses " + enemyStats.move1_name + " and " + enemyStats.move1_response };
            StartCoroutine(TypeLine(enemyAttackLines));
        }

        if (enemyMove == 2)
        {
            dead = playerStats.TakeDamage(enemyStats.move2_damage, enemyStats.move2_mp, playerStats.mentalPhysical);
            // Dialogue Line
            string[] enemyAttackLines = { enemyStats.spriteName + " uses " + enemyStats.move2_name + " and " + enemyStats.move2_response };
            StartCoroutine(TypeLine(enemyAttackLines));
        }

        if (enemyMove == 3)
        {
            dead = playerStats.TakeDamage(enemyStats.move3_damage, enemyStats.move3_mp, playerStats.mentalPhysical);
            // Dialogue Line
            string[] enemyAttackLines = { enemyStats.spriteName + " uses " + enemyStats.move3_name + " and " + enemyStats.move3_response };
            StartCoroutine(TypeLine(enemyAttackLines));

        }
        if (enemyMove == 4)
        {
            dead = playerStats.TakeDamage(enemyStats.move4_damage, enemyStats.move4_mp, playerStats.mentalPhysical);
            // Dialogue Line
            string[] enemyAttackLines = { enemyStats.spriteName + " uses " + enemyStats.move4_name + " and " + enemyStats.move4_response };
            StartCoroutine(TypeLine(enemyAttackLines));
        }

        // Update the player UI details
        PlayerUI();

        yield return new WaitForSeconds(5f);

        // Check whether the player has lost all their HP
        if (dead)
        {
            // The battle is lost when the player is dead
            gameState = BattleState.LOST;
            StartCoroutine(EndBattle());
        } else
        {
            // Continue the battle if the player is still alive once the enemy's turn is over
            gameState = BattleState.PLAYERTURN;
            StartCoroutine(PlayerTurn());
        }

    }

    // The player turn
    IEnumerator PlayerTurn()
    {
        string[] playerTurnLines = { "Choose your next action:"};
        StartCoroutine(TypeLine(playerTurnLines));

        yield return new WaitForSeconds(2.5f);

        // Remove the text display
        dialogueBox.SetActive(false);
        // Make battle player section, attack and item buttons disappear
        attackItemButtons.SetActive(true);


    }

    // The WON and LOST states
    IEnumerator EndBattle()
    {
        if (gameState == BattleState.WON)
        {

            // The dialogue and rewards for when the battle is won
            System.Random rnd = new System.Random();
            int randomText = rnd.Next(1, 3);
            string[] finalText = { "What a fine effort. You beat " + enemyStats.spriteName, "Excellent progress. You beat " + enemyStats.spriteName,
            "One more enemy down. You beat " + enemyStats.spriteName };
            string[] wonBattleLines = { finalText[randomText - 1] };
            StartCoroutine(TypeLine(wonBattleLines));

            yield return new WaitForSeconds(2.5f);

            // Guide the player to talk to the professor
            if (enemyStats.spriteName == "Prof. Kazan")
            {
                // Set the end game variable to true
                endGame = true;

                string[] hint = { "Go and talk to Prof. Kazan" };
                StartCoroutine(TypeLine(hint));
                yield return new WaitForSeconds(2.5f);
            }

            gameState = BattleState.NOBATTLE;
        }
        else if (gameState == BattleState.LOST)
        {
            // The dialogue and losses for when the battle is lost
            string[] lostBattleLines = { "You were beaten. You passed out." };
            StartCoroutine(TypeLine(lostBattleLines));

            yield return new WaitForSeconds(2.5f);

            // LOAD A "GAME OVER"
            battleInterface.SetActive(false);
            gameOverScreen.SetActive(true);
            yield return new WaitForSeconds(1f);

            // Allow the player to press any key to play again
            while (!Input.anyKeyDown)
            {
                yield return null;
            }

            gameState = BattleState.NOBATTLE;
            SceneManager.LoadScene("new");
        }

        // Make sure the player still gains XP (no matter whether they win or lose)
        playerStats.LevelUp(enemyStats.level);
        // Get rid of the battle interface
        battleInterface.SetActive(false);
        // Change the camera in the battle script by letting the script know the battle is over
        battleCameraScript.isBattle = false;
        // Allow the player to move again
        playerMovementScript.canMove = true;
        // Reset the system variables
        hasRun = false;
        actionChosen = false;
    }

    // For when the ATTACK button is selected by the user
    public void AttackButton()
    {
        if (gameState == BattleState.PLAYERTURN)
            // Make battle player section, attack and item buttons disappear
            attackItemButtons.SetActive(false);
            // Make the attack buttons appear
            attackButtons.SetActive(true);
    }

    public void FirstAttack()
    {
        // Allow the player to attack if they press the attack button during their turn
        // Ensure the player only chooses an action once
        // Ensure all the text has loaded onto the screen
        if (gameState == BattleState.PLAYERTURN && actionChosen == false)
        {
            // Start the attack with the correct damage chosen
            StartCoroutine(Attack(playerStats.move1_damage, playerStats.move1_mp, enemyStats.mentalPhysical, playerStats.move1_response));
            // Pass in the correct attack
            actionChosen = true;

            // Make the action buttons disappear
            attackButtons.SetActive(false);
            // Reactivate the dialogue box
            dialogueBox.SetActive(true);
        }
    }
    public void SecondAttack()
    {
        // Allow the player to attack if they press the attack button during their turn
        // Ensure the player only chooses an action once
        // Ensure all the text has loaded onto the screen
        if (gameState == BattleState.PLAYERTURN && actionChosen == false)
        {
            // Start the attack with the correct damage chosen
            StartCoroutine(Attack(playerStats.move2_damage, playerStats.move2_mp, enemyStats.mentalPhysical, playerStats.move2_response));
            // Pass in the correct attack
            actionChosen = true;

            // Make the action buttons disappear
            attackButtons.SetActive(false);
            // Reactivate the dialogue box
            dialogueBox.SetActive(true);
        }
    }

    public void ThirdAttack()
    {
        // Allow the player to attack if they press the attack button during their turn
        // Ensure the player only chooses an action once
        // Ensure all the text has loaded onto the screen
        if (gameState == BattleState.PLAYERTURN && actionChosen == false)
        {
            // Start the attack with the correct damage chosen
            StartCoroutine(Attack(playerStats.move3_damage, playerStats.move3_mp, enemyStats.mentalPhysical, playerStats.move3_response));
            // Pass in the correct attack
            actionChosen = true;

            // Make the action buttons disappear
            attackButtons.SetActive(false);
            // Reactivate the dialogue box
            dialogueBox.SetActive(true);
        }
    }
    public void FourthAttack()
    {
        // Allow the player to attack if they press the attack button during their turn
        // Ensure the player only chooses an action once
        // Ensure all the text has loaded onto the screen
        if (gameState == BattleState.PLAYERTURN && actionChosen == false)
        {
            // Start the attack with the correct damage chosen
            StartCoroutine(Attack(playerStats.move4_damage, playerStats.move4_mp, enemyStats.mentalPhysical, playerStats.move4_response));
            // Pass in the correct attack
            actionChosen = true;

            // Make the action buttons disappear
            attackButtons.SetActive(false);
            // Reactivate the dialogue box
            dialogueBox.SetActive(true);
        }
    }

    // For when the ITEM button is selected by the user
    public void ItemButton()
    {
        // Allow the player to  heal if they press the ITEM button during their turn
        // Enusre the player only chooses an action once
        // Ensure all the text has loaded onto the screen
        if (gameState == BattleState.PLAYERTURN && actionChosen == false && playerStats.USB > 0)
        {
            // Allow the player to heal
            StartCoroutine(Item());
            // Ensure we know an action was chosen
            actionChosen = true;
            //Reduce the number of USBs the player has
            playerStats.USB += -1;
            // Make battle player section, attack and item buttons disappear
            attackItemButtons.SetActive(false);
            // Reactivate the dialogue box
            dialogueBox.SetActive(true);
        }

        if (gameState == BattleState.PLAYERTURN && actionChosen == false && playerStats.USB <= 0)
        {
            StartCoroutine(ItemButtonFailure());
        }
    }

    IEnumerator ItemButtonFailure()
    {
        // Make battle player section, attack and item buttons disappear
        attackItemButtons.SetActive(false);
        // Reactivate the dialogue box
        dialogueBox.SetActive(true);
        string[] mustAttackLines = { "You don't have any USBs left. Time to attack!" };
        StartCoroutine(TypeLine(mustAttackLines));
        yield return new WaitForSeconds(3.5f);
        // Reactivate the dialogue box
        dialogueBox.SetActive(false);
        // Make the attack buttons appear
        attackButtons.SetActive(true);
    }


    // IF A CERTAIN MOVE(S) ARE USED ON A CERTAIN ENEMY, GIVE THEM SPECIAL DIALOGUE IN A CUT-SCENE PERHAPS

}
 