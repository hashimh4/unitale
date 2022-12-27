using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomPlayerTransition : MonoBehaviour
{
    // Define how much the player will move by during the room transition
    public Vector3 playerChange;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            other.transform.position += playerChange;
        }
    }
}
