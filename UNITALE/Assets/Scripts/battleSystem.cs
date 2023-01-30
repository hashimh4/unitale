using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    // The dialogue to be loaded on the UI
    public TMP_Text text;
    public TMP_Text playerLevelText;
    // The enemy HP information to be loaded 
    public TMP_Text playerHPText;
    // The enemy name to be loaded
    public TMP_Text enemyNameText;
    // The enemy HP information to be loaded 
    public TMP_Text enemyHPText;

    // The speed of the text to be loaded, set to 0.05 seconds
    private float dialogueSpeed = 0.05F;
    // Whether the InitiateBattle function has already run
    private bool hasRun = false;
    // Whether the player has already chosen their actioon
    private bool actionChosen = false;

    // To store the stats of the player and enemy
    stats playerStats;
    stats enemyStats;

    // Reference to the battle camera script to change the camera if there is a battle
    public battleCameraTransition battleCameraScript;
    // Reference to the player movement script
    public playerMovement playerMovementScript;
    // Making reference to the game state, so that it can be changed
    public BattleState gameState;


    /// <summary>
    /// ////////////////////////////////////////////////// CREATE DIALOGUE SIMILAR TO SIGN DIALOGUE 
    /// </summary>


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
        
        // Retrieve the player stats and store these
        playerStats = playerPrefab.GetComponent<stats>();

        // Load the enemy in the correct location
        GameObject enemyInitiate = Instantiate(enemyPrefab, enemyLocation);
        // Retrieve the enemy stats and store these
        enemyStats = enemyInitiate.GetComponent<stats>();

        // Load the dialogue lines on the screen
        text.text = string.Empty;
        string[] initial = { enemyStats.spriteName + " approaches..."};
        StartCoroutine(TypeLine(initial));

        // Update the UI on the screen
        PlayerUI();
        EnemyUI();

        // DIALOGUE //////////////////////////////////////////////////////////
        yield return new WaitForSeconds(2f);

        // Change the game state to the correct turn ////////////////////////////////////
        gameState = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    // The method to display each character in a sentence one by one, when it is loaded
    // Ensures the final sentence is displayed
    IEnumerator TypeLine(string[] dialogueText)
    {
        text.text = string.Empty;

        foreach (string i in dialogueText) {
            foreach (char c in i.ToCharArray())
            {
                text.text += c;
                // Set the time to wait for each character to be displayed
                yield return new WaitForSeconds(dialogueSpeed);
            }
            if (i != dialogueText.Last())
            {
                text.text = string.Empty;
            }
        }
    }


    // If the player chooses to attack on their turn
    IEnumerator Attack()
    {
        // Update the enemy stats
        bool dead = enemyStats.TakeDamage(playerStats.move1_damage);

        // Update the enemy UI details
        EnemyUI();
        // Dialogue line
        string[] playerAttackLines = { "You hit " + enemyStats.spriteName};
        StartCoroutine(TypeLine(playerAttackLines));

        yield return new WaitForSeconds(2f);

        // Check whether the enemy is dead
        if(dead)
        {
            // The battle is won when the enemy is dead
            gameState = BattleState.WON;
            EndBattle();

        } else
        {
            // Continue the battle and go to the enemy's turn if they are not dead
            gameState = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator Item()
    {
        // Heal the player by 5 HP
        playerStats.Heal(5);

        // Update the player UI
        PlayerUI();

        // Dialogue line
        string[] playerItemLines = { "You heal yourself."};
        StartCoroutine(TypeLine(playerItemLines));

        yield return new WaitForSeconds(2f);

        // Continue the battle and change the state to the enemy's turn
        gameState = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    // The enemy AI
    IEnumerator EnemyTurn()
    {
        // Reset the variable for the next player turn, so they can choose an action again
        actionChosen = false;

        // Dialogue line
        string[] enemyAttackLines = { enemyStats.spriteName + " attacks."};
        StartCoroutine(TypeLine(enemyAttackLines));

        yield return new WaitForSeconds(2f);

        // Update the player stats
        bool dead = playerStats.TakeDamage(enemyStats.move1_damage);

        // Update the player UI details
        PlayerUI();

        yield return new WaitForSeconds(2f);

        // Check whether the player has lost all their HP
        if (dead)
        {
            // The battle is lost when the player is dead
            gameState = BattleState.LOST;
            EndBattle();
        } else
        {
            // Continue the battle if the player is still alive once the enemy's turn is over
            gameState = BattleState.PLAYERTURN;
            PlayerTurn();
        }

    }

    // The player turn
    void PlayerTurn()
    {
        string[] playerTurnLines = { "Choose your next action:"};
        StartCoroutine(TypeLine(playerTurnLines));
    }

    // The WON and LOST states
    void EndBattle()
    {
        if(gameState == BattleState.WON)
        {
            // The dialogue and rewards for when the battle is won
            string[] wonBattleLines = { "You beat " + enemyStats.spriteName};
            StartCoroutine(TypeLine(wonBattleLines));
            gameState = BattleState.NOBATTLE;

        } else if (gameState == BattleState.LOST)
        {
            // The dialogue and losses for when the battle is lost
            string[] lostBattleLines = { "You were beated. You passed out."};
            StartCoroutine(TypeLine(lostBattleLines));
            gameState = BattleState.NOBATTLE;

        }
        // Get rid of the battle interface
        battleInterface.SetActive(false);
        // Change the camera in the battle script by letting the script know the battle is over
        battleCameraScript.isBattle = false;
        // Allow the player to move again
        playerMovementScript.canMove = true;
        // Reset the system variables
        hasRun = false;
    }

    // For when the ATTACK button is selected by the user
    public void AttackButton()
    {

        // Allow the player to attack if they press the attack button during their turn
        // Ensure the player only chooses an action once
        if (gameState == BattleState.PLAYERTURN && actionChosen == false)
        {
            StartCoroutine(Attack());
            actionChosen = true;
        }

    }

        // For when the ITEM button is selected by the user
    public void ItemButton()
    {
        // Allow the player to  heal if they press the ITEM button during their turn
        // Enusre the player only chooses an action once
        if (gameState == BattleState.PLAYERTURN && actionChosen == false)
        {
            StartCoroutine(Item());
            actionChosen = true;
        }

    }

    // ENSURE YOU CAN ONLY CLICK ONCE

}
 