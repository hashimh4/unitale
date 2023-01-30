using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{

    // Defining the move speed
    public float moveSpeed = 5f;
    // Create the rigid body that moves our player
    public Rigidbody2D rb;
    // Refer to the animator
    public Animator animator;
    // Tells us if the player is allowed to move or not
    public bool canMove;

    // Stores x and y values
    private Vector2 movement;

    private void Start()
    {
        // Ensure the player can move right away
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Define movement as a 2D vector
        movement = Vector2.zero;
        
        // To keep up with input
        if (canMove)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }

        // Ensure that the magnitude for diagnoal inputs remains the same (a maximum of 1)
        movement = Vector2.ClampMagnitude(movement, 1);

        // Sets our animator to the correct values so we see the right player animation
        if (movement != Vector2.zero)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);

            // If we are moving, then use the walking animations, otherwise use the idle animations
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }

        // Set the speed variable
        animator.SetFloat("Speed", movement.sqrMagnitude);

    }

    // For physics, to keep up with the frame rate
    void FixedUpdate()
    {
        // Moves the player to the new position (ensuring the speed stays the same)
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
