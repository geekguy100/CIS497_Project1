using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //private variables
    private float moveHorizontal;
    private float moveVertical;
    public Vector3 velocity;
    private bool isGrounded;

    //public variables
    public float gravity = -20f;
    public float speed = 10f;
    public float groundDistance = .3f;
    public float jumpHeight = 2f;
    public CharacterController Controller;
    public Transform groundCheck;
    public LayerMask groundMask;


    // Update is called once per frame
    void FixedUpdate()
    {
        //Can only move once the game starts. Cannot move when it ends.
        if (GameManager.instance.gameStarted && !GameManager.instance.GameOver)
            Movement();
    }

    private void Movement()
    {
        //Checks if the player is on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //resets the Up/down velocity of the player if the player is on the ground
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //Sets the inputs for movement
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        //allows the player to move around
        Vector3 move = transform.right * moveHorizontal + transform.forward * moveVertical;
        Controller.Move(move * speed * Time.deltaTime);

        //Checks if the player is on the gound and if the player is it will allow the player to jump
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //Pulls the player back down to the ground using gravity
        velocity.y += gravity * Time.deltaTime;
        Controller.Move(velocity * Time.deltaTime);


    }
}
