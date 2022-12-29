using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class dialogue2 : MonoBehaviour
{
    // References to the game objects - the dialogue box and the text within this
    public GameObject dialogueBox;
    public TMP_Text text;
    // The text to be loaded
    public string[] dialogueText;

    // Whether the player is in the correct area
    public bool interaction;
    public float dialogueSpeed;
    // Which section of text / sentence the user is currently seeing
    private int index;
    // Whether the index is at zero
    private bool first;

    // Start is called before the first frame update
    void Start()
    {
        // Set everything in the box to empty, so the first line of text can be loaded
        text.text = string.Empty;
        // Set the sentence index to zero
        index = 0;
        // Ensure we know the user has not interacted with the dialogue yet
        first = true;
    }

    // Update is called once per frame
    void Update()
    {
        // When the user presses space and can interact with an object
        if (Input.GetKeyDown(KeyCode.Space) && interaction)
        {
            // Display the dialogue box
            dialogueBox.SetActive(true);
            // The initial case of typing the first sentence - so that it doesn't all display at once
            if (first) {
                text.text = string.Empty;
                StartCoroutine(TypeLine());
                // Ensure this only happens the first time around
                first = false;
            }
            // If we are on the next sentence, and the dialogue box is active, load the next line
            else if (dialogueBox.activeInHierarchy && text.text == dialogueText[index])
            {
                NextLine();
            }
            else
            {
            // Stop the method of loading the sentence
                StopAllCoroutines();
            // Set the next line of text to the next predefined sentence
                text.text = dialogueText[index];
            }
        }
    }

    // The method to display each character in a sentence one by one, when it is loaded
    IEnumerator TypeLine()
    {
        foreach (char c in dialogueText[index].ToCharArray())
        {
            text.text += c;
            // Set the time to wait for each character to be displayed
            yield return new WaitForSeconds(dialogueSpeed);
        }
    }

    // The method to load the next line of text
    void NextLine()
    {   
        // When there is still a sentence to display
        if (index < dialogueText.Length - 1)
        {
            // Ensure we know what sentence to display next
            index++;
            // Set the text on screen to nothing
            text.text = string.Empty;
            // Load the next sentence of pre-defined text
            StartCoroutine(TypeLine());
        }
        else
        {
            // When there are no more sentences to display, hide the dialogue box
            dialogueBox.SetActive(false);
            // Set the sentence index and first encounter booleon back to their initial values
            index = 0;
            first = true;
        }
    }

    // When the player enters the collider box for some dialogue
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the player is within the collider box, then they can interact with the object
        if (other.CompareTag("Player"))
        {
            interaction = true;
        }
    }

    // When the player exists the collider box for some dialogue
    private void OnTriggerExit2D(Collider2D other)
    {
        // If the player is outside the collider box, then they cannot interact with the object
        if (other.CompareTag("Player"))
        {
            interaction = false;
            // If the player leaves the interaction area, close the dialogue box
            dialogueBox.SetActive(false);
            // Set the sentence index and first encounter booleon back to their initial values
            index = 0;
            first = true;
            // Stop the method of loading the sentence
            StopAllCoroutines();
        }
    }
}
