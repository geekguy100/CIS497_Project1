/*
 * Chandler Wesoloski, Chris Smith
 * Project 1+2
 * Controls player movement
 */ 

using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //private variables
    private float moveHorizontal;
    private float moveVertical;
    public Vector3 velocity;
    private bool isGrounded;
    private AudioSource playerAudio;

    //public variables
    public float gravity = -20f;
    public float speed = 10f;
    public float groundDistance = .3f;
    public float jumpHeight = 2f;
    public CharacterController Controller;
    public Transform groundCheck;
    public LayerMask groundMask;
    public AudioClip jumpSFX;
    public AudioClip bgMusic;

    private void Awake()
    {
        playerAudio = GetComponent<AudioSource>();
    }

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

        //Kyle Grenier (10/7): Clamping mouse input between (-1,1) and mulitplying it by the sensitivity to prevent WebGL issues.
        moveHorizontal = Mathf.Clamp(moveHorizontal, -1, 1);
        moveVertical = Mathf.Clamp(moveVertical, -1, 1);

        //allows the player to move around
        Vector3 move = transform.right * moveHorizontal + transform.forward * moveVertical;
        Controller.Move(move * speed * Time.deltaTime);

        //Checks if the player is on the gound and if the player is it will allow the player to jump
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            playerAudio.PlayOneShot(jumpSFX, 1.0f);
        }
       
        //Pulls the player back down to the ground using gravity
        velocity.y += gravity * Time.deltaTime;
        Controller.Move(velocity * Time.deltaTime);


    }
}
