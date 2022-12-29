using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class simpleSignDialogue : MonoBehaviour
{
    public GameObject dialogueBox;
    public TMP_Text text;
    public string dialogueText;
    public bool interaction;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // If the user presses space and can interact with an object, either show the dialogue for that object or close it
        if (Input.GetKeyDown(KeyCode.Space) && interaction)
        {
            if (dialogueBox.activeInHierarchy)
            {
                dialogueBox.SetActive(false);
            } else
            {
                dialogueBox.SetActive(true);
                text.text = dialogueText;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            // If the player is within the collider box, then they can interact with the object
            interaction = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // If the player is outside the collider box, then they cannot interact with the object
            interaction = false;
            // If the player leaves the interaction area, close the dialogue box
            dialogueBox.SetActive(false);
        }
    }
}
