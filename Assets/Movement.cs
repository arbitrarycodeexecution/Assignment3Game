using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;        // Horizontal movement speed
    public float jumpForce = 10f;       // Force applied when jumping
    public Rigidbody rb;              // Reference to Rigidbody2D component

    private float verticalInput;      // Store vertical input
    private float horizontalInput;      // Store horizontal input
    public bool isGrounded;            // Is the player on the ground?
    private bool canDoubleJump;         // Can the player perform a double jump?


    public Transform groundCheck;       // Position to check if grounded
    public float groundCheckRadius = 0.2f; // Radius of ground check circle
    public LayerMask groundLayer;       // Layer considered as ground






    void Update()
    {
        // Get horizontal input (-1 for left, 1 for right)
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");





        // Check if the player is on the ground (should happen before jump input)
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, groundCheckRadius, groundLayer);



        // Jump input
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                // First jump
                Jump();
                canDoubleJump = false; // Enable double jump
            }
            else if (canDoubleJump)
            {
                // Double jump
                Jump();
                canDoubleJump = false; // Use up double jump
            }
        }
    }


    void FixedUpdate()
    {
        // Get camera forward/right directions (flattened on ground)
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0f;     // Don't tilt movement upward/downward
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        // Combine input with camera direction
        Vector3 moveDir = (camRight * horizontalInput + camForward * verticalInput).normalized;

        // Apply movement while keeping gravity
        rb.velocity = new Vector3(moveDir.x * moveSpeed, rb.velocity.y, moveDir.z * moveSpeed);
    }


    void Jump()
    {
        // Apply vertical velocity for jumping
        rb.velocity = new Vector3(rb.velocity.x, jumpForce);
    }


    
}


