using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[System.Serializable]

// CHANGE THE SPRITE FOR EACH DIALOGUE LINE
// (THIS WILL BE SIMILAR TO CHANGING THE AUDIO FOR EACH DIALOGUE LINE)

// WE WANT IT JUST TO END AND THEN SUPPLY ANOTHER SET OF DIALOGUE WITH A DIFFERENT IMAGE - TO A DIALOGUE CUT-SCENE HANDLER
// ENSURE THAT IT HAPPENS - YOU DON'T NEED TO PRESS SPACE AND BECOME FROZEN IN PLACE UNTIL ALL THE DIALOGUE IS OVER

// SET THE FIRST ONE ACTIVE, TURN THE OTHERS OFF. SET THE NEXT ONE ACTIVE, TURN THE OTHERS OFF. UNTIL WE REACH THE END WHERE WE STOP

// STOP THE CONTROLS FROM WORKING DURING A CUTSCENE

public class cutSceneDialogue : MonoBehaviour
{
    // References to the game objects - the dialogue box and the text within this
    public GameObject dialogueBox;
    public TMP_Text text;
    public Image imageHolder;
    // The text to be loaded
    [TextArea(1, 10)]
    public string[] dialogueText;
    // The image of the character who is talking
    public Sprite characterSprite;
    // Whether the player is in the correct area
    public bool interaction;
    // The speed of dialogue
    public float dialogueSpeed;
    // Tells the cut scene script which set of dialogue to go to
    public bool nextSet { get; private set; }

    // Which section of text / sentence the user is currently seeing
    private int index;
    // Whether the index is at zero
    private bool first;
    private bool initial;

    // Start is called before the first frame update
    void Start()
    {
        // Set everything in the box to empty, so the first line of text can be loaded
        text.text = string.Empty;
        // Set the sentence index to zero
        index = 0;
        // Ensure we know the user has not interacted with the dialogue yet
        first = true;
        // Ensure the system knows not to go on to the next set yet
        nextSet = false;
        initial = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Allow the user to continue the dialogue when they press space, or automatically trigger the dialogue if it is a new set
        if ((Input.GetKeyDown(KeyCode.Space) && interaction) || (initial && interaction))
        {
            initial = false;
            // Display the dialogue box
            dialogueBox.SetActive(true);
            // Display the face of the character talking
            imageHolder.sprite = characterSprite;
            imageHolder.preserveAspect = true;
            // The initial case of typing the first sentence - so that it doesn't all display at once
            if (first)
            {
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
            initial = true;
            // Add one to the cut-scene index to ensure that the next child of dialogue is visible, as done within the cut scene handler script
            nextSet = true;
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
            // Set everything in the dialogue box to black when the system changes child
            text.text = string.Empty;
            // Stop the method of loading the sentence
            StopAllCoroutines();
        }
    }
}
