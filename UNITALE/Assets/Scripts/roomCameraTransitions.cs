using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class roomCameraTransitions : MonoBehaviour
{
    public GameObject virtualCam;
    // Whether the location requires text
    public bool needText;
    // Allows us to set the text
    public string locationName;
    // Defining the text object
    public GameObject text;
    // Defining the text for the object
    public Text locationText;

    // If the player is in a particular collider box, use that camera
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the player is in the collider box, use the camera
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            virtualCam.SetActive(true);

            // Add the location place if necessary
            if (needText)
            {
                StartCoroutine(locationNameCo());
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        // If the player is not in the collider box, do not use the camera
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            virtualCam.SetActive(false);
        }
    }  

    // Allows the location place to pop up and then disappear
    private IEnumerator locationNameCo()
    {
        // The text appears
        text.SetActive(true);
        locationText.text = locationName;
        // Wait five seconds and then make the text disappear
        yield return new WaitForSeconds(5f);
        text.SetActive(false);
    }
}
