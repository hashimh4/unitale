using System.Collections;
using System.Collections.Generic;
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

    // The dialogue to be loaded on the UI
    public TMP_Text text;
    public TMP_Text playerLevelText;
    // The enemy HP information to be loaded 
    public TMP_Text playerHPText;
    // The enemy name to be loaded
    public TMP_Text enemyNameText;
    // The enemy HP information to be loaded 
    public TMP_Text enemyHPText;

    // To store the stats of the player and enemy
    stats playerStats;
    stats enemyStats;

    // Making reference to the game state, so that it can be changed
    public BattleState gameState;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure we enter the start state when entering a battle
        gameState = BattleState.START;
        StartCoroutine(InitiateBattle());
    }

    // put in update and check that the state is not NOBATTLE
    // otherwise load the correct enemy and play the dialogue
    /// /////////////////////////////////////////////////////////////
    
    // The player specific UI that needs to be changed throughout the battle
    void PlayerUI()
    {
        // Load the player level on the screen
        playerLevelText.text = "Level" + playerStats.level;
        // Load the player's HP on the screen
        playerHPText.text = "HP - " + playerStats.currentHP + "/" + playerStats.maxHP;
    }

    // The enemy specific UI that needs to be changed throughout the battle
    void EnemyUI()
    {
        // Load the enemy's name on the screen
        enemyNameText.text = enemyStats.name;
        // Load the enemy's HP on the screen
        enemyHPText.text = "HP - " + enemyStats.currentHP + "/" + enemyStats.maxHP;
    }

    // The START battle state
    IEnumerator InitiateBattle()
    {
        // Retrieve the player stats and store these
        playerStats = playerPrefab.GetComponent<stats>();

        // Load the enemy in the correct location
        GameObject enemyInitiate = Instantiate(enemyPrefab, enemyLocation);
        // Retrieve the enemy stats and store these
        enemyStats = enemyInitiate.GetComponent<stats>();

        // Load the dialogue lines on the screen
        text.text = string.Empty;
        text.text = enemyStats.name + " approaches...";

        // Update the UI on the screen
        PlayerUI();
        EnemyUI();

        // DIALOGUE //////////////////////////////////////////////////////////
        yield return new WaitForSeconds(2f);

        // Change the game state to the correct turn ////////////////////////////////////
        gameState = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    // If the player chooses to attack on their turn
    IEnumerator Attack()
    {
        // Update the enemy stats
        bool dead = enemyStats.TakeDamage(playerStats.move1_damage);

        // Update the enemy UI details
        EnemyUI();
        // Dialogue line
        text.text = "You hit " + enemyStats.name;

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
        text.text = "You heal yourself.";

        yield return new WaitForSeconds(2f);

        // Continue the battle and change the state to the enemy's turn
        gameState = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    // The enemy AI
    IEnumerator EnemyTurn()
    {
        // Dialogue line
        text.text = enemyStats.name + " attacks.";

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
        text.text = "Choose your next action:";
    }

    // The WON and LOST states
    void EndBattle()
    {
        if(gameState == BattleState.WON)
        {
            // The dialogue and rewards for when the battle is won //////////////
            text.text = "You beat " + enemyStats.name;

        } else if (gameState == BattleState.LOST)
        {
            // The dialogue and losses for when the battle is lost ///////////////
            text.text = "You were beated. You passed out.";
        }
    }

    // For when the ATTACK button is selected by the user
    public void AttackButton()
    {
        // Allow the player to attack if they press the attack button during their turn
        if (gameState == BattleState.PLAYERTURN)
        {
            StartCoroutine(Attack());
        }
    }

        // For when the ITEM button is selected by the user
    public void ItemButton()
    {
        // Allow the player to  heal if they press the ITEM button during their turn
        if (gameState == BattleState.PLAYERTURN)
        {
            StartCoroutine(Item());
        }

    }

    // ENSURE YOU CAN ONLY CLICK ONCE

}
 