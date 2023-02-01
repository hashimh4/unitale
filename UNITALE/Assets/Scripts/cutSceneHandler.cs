using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cutSceneHandler : MonoBehaviour
{
    // Reference to the player movement script to prevent movement during the cut-scene
    public playerMovement movementScript;
    // Reference to the player stats
    public stats playerStats;
    // Reference to the dialogue box object so it can be closed once all the dialogue is seen
    public GameObject dialogueBox;
    // If the cut scene is before a battle
    public bool battleCutScene;
    // Whether the player has collected a USB
    public bool USB;
    // Reference to the battle script to update the game state
    public battleSystem battleScript;
    // To pass the correct enemy prefab to the battle system, when there is due to be a battle
    public GameObject battleEnemyPrefab;

    private void Start()
    {
        // Run the method to handle the correct ordering of dialogue
        StartCoroutine(childSequence());
    }

    private IEnumerator childSequence()
    {        
        // We ensure the user sees all the sections of dialogue
        for (int i = 0; i < transform.childCount; i++)
        {
            Deactivate();
            // The next section of dialogue is activated, which has not been seen yet
            transform.GetChild(i).gameObject.SetActive(true);
            // We wait before we loop, until the complete set of diaologue is seen before moving on to the next set
            yield return new WaitUntil(() => transform.GetChild(i).GetComponent<cutSceneDialogue>().nextSet);
        }

        // Once all the dialogue has been seen, deactivate the cut-scene object so it is not seen again, and the dialogue box displayed on screen
        gameObject.SetActive(false);
        dialogueBox.SetActive(false);

        // If the player has collected a USB, increase the USB count by one
        if (USB)
        {
            playerStats.USB += 1;
        }

        // Only pass on a battle enemy sprite and start a battle if this is a battle cut scene
        if (battleCutScene)
        {
            // Pass the correct enemy prefab to the battle system script
            battleScript.enemyPrefab = battleEnemyPrefab;
            // Change the state in the battle script
            battleScript.gameState = BattleState.START;
        } else
        {
            // If there is not a battle to follow, allow the player to move right away
            movementScript.canMove = true;
        }

    }

    private void Deactivate()
    {
        // All the sections of dialogue are deactivated
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
