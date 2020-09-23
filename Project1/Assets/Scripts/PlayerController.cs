﻿using System.Collections;
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
    public float gravity = -9.81f;
    public float speed = 12f;
    public float groundDistance = .4f;
    public float jumpHeight = 3f;
    public CharacterController Controller;
    public Transform groundCheck;
    public LayerMask groundMask;


    // Update is called once per frame
    void Update()
    {
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
