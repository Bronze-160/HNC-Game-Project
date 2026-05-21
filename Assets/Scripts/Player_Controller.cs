using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Player_Controller : MonoBehaviour
{
    //These variables are to hold the Action references
    InputAction moveAction;
    InputAction jumpAction;
    InputAction grappleAction;
    InputAction attackAction;

    private Scene activeScene; // Stores the Active Scene
    [SerializeField] string nextScene = "Death"; // Stores what the next scene will be and can be changed


    [Header("Other Scripts")] // Referance to other Script
    public CameraChanger cameraChanger;
    public Player_Stats playerStats; // references to PlayerStats script

    [Header("Player Component Refrences")]
    [SerializeField] Rigidbody2D rb;

    [Header("Player Settings")] // -- Finlay Macmillan
    [SerializeField] float speed; // Stores players Speed 
    [SerializeField] float jumpingPower;// Stores players Jump Power
    [SerializeField] Vector3 playerScale; // Stores the players scale

    public Vector2 moveValue;

    //Check if Player is on the ground/ a Platform
    public bool isGrounded; 
    public bool onPlatfrom;

    // Temp //
    private Animator animator; // links the script to the Unity animator
    
    [Header("Animation")] // names for peramitors of the animation - Names -- Finlay Macmillan

    [SerializeField] string isRunning = "isRunning"; // Peramitor if player is Moving 
    [SerializeField] string isJumping = "isJumping"; // Peramitor if player is Jumping
    [SerializeField] string isAttacking = "isAttacking"; // Peramitor if player is Attacking 

    //     //

    [Header("Mantle Mechanic")] // Allows the player to mantle on Ledges -- Finlay Macmillan

    [SerializeField] Transform wallCheck;
    [SerializeField] Transform headCheck;
    [SerializeField] float checkDistance = 0.2f;
    [SerializeField] LayerMask groundLayer;
    private bool isFacingRight = false;
    bool isTouchingWall;
    bool isTooTall;
    [SerializeField] float mantlePower = 2;

    [Header("Slide Mechanic")] // Allows the player to Slide -- Ben

    public float slideSpeed = 12f;
    public float slideDuration = 0.4f;
    private bool isSliding;


    void Start()
    {
        activeScene = SceneManager.GetActiveScene();
        Debug.Log("Active Scene is '" + activeScene.name + "'."); // Tells Console What the Active Scene Is called

        animator = GetComponent<Animator>(); // Collets the Animator Componant 

        playerScale = transform.localScale; // Makes sure the obejcts scale is the players scale

        // Referance to find each action
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        grappleAction = InputSystem.actions.FindAction("Grapple");
        attackAction = InputSystem.actions.FindAction("Attack");
        //EditorApplication.isPaused = false;
    }


    // makes sure Inputs don't Break //
    void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        grappleAction.Enable();
        attackAction.Enable();  
    }

    void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        grappleAction.Disable();
        attackAction.Disable();
    }
    // //

    void Update()
    {
        if (attackAction.triggered)
        {
            animator.SetBool(isAttacking, true); // Attack Animation Play's 
        }

        if (moveValue.x >= 1 || moveValue.x <= -1) // Checks if player is moving 
        {
            animator.SetBool(isRunning, true);// Run Animation Play's
        }
        else
        {
            animator.SetBool(isRunning, false);// Ruin Animation stops
        }

        //sends your movment code to the new input system
        moveValue = moveAction.ReadValue<Vector2>();
        rb.linearVelocity = new Vector2(moveValue.x * speed, rb.linearVelocity.y);

        //checks if jump button is pressed
        if (jumpAction.triggered && isGrounded == true || jumpAction.triggered && onPlatfrom == true)
        {
            animator.SetTrigger("Jump"); // Jump Animation Play's
            Jump(); // Makes player Jump
        }

        if (isGrounded == true || onPlatfrom == true)
        {
            animator.SetBool(isJumping, false);
        }
        else
        {
            animator.SetBool(isJumping, true);
        }


        if (moveValue.x > 0) { isFacingRight = true; } //makes sure the player faces the way they are moving
        if (moveValue.x < 0) { isFacingRight = false; }

        // Makes sure the mantle detectors also flip when the player does -- Finlay Macmillan
        if (isFacingRight == true)
        {
            isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, checkDistance, groundLayer);
            Debug.DrawRay(wallCheck.position, transform.right, Color.green);

            isTooTall = Physics2D.Raycast(headCheck.position, transform.right, checkDistance, groundLayer);
            Debug.DrawRay(headCheck.position, transform.right, Color.green);

        }
        if (isFacingRight == false)
        {
            isTouchingWall = Physics2D.Raycast(wallCheck.position, -transform.right, checkDistance, groundLayer);
            Debug.DrawRay(wallCheck.position, -transform.right, Color.green);

            isTooTall = Physics2D.Raycast(headCheck.position, -transform.right, checkDistance, groundLayer);
            Debug.DrawRay(headCheck.position, -transform.right, Color.green);
        }

        
        if (isTouchingWall && !isTooTall && !isGrounded && jumpAction.triggered) // -- Finlay Macmillan
        {
            StartMantle(); //starts the mantle mechanic
        }

        Flip(); // starts the Flip Function

        if (activeScene.name == "Level_3") // makes sure this does not effect the other scenes -- Finlay Macmillan
        {
            {
                if (cameraChanger.currentCamera == cameraChanger.cameraStart)
                {
                    cameraChanger.SwitchCamera(cameraChanger.camera_1);
                }
            }
        }
        //Slide Mechanic -- Ben.P
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isSliding)
        {
            StartCoroutine(Slide());
        }


        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(activeScene.name == "Level_3") // -- Finlay Macmillan
        {
            //This blends the camera between rooms with colliding with the room trigger
            if (cameraChanger.currentCamera != cameraChanger.cameraStart)
            {
                if (other == cameraChanger.room1)
                {
                        cameraChanger.SwitchCamera(cameraChanger.camera_1);
                }
                else if (other == cameraChanger.room2)
                {
                        cameraChanger.SwitchCamera(cameraChanger.camera_2);
                }
                else if (other == cameraChanger.room3)
                {
                        cameraChanger.SwitchCamera(cameraChanger.camera_3);
                }
                else if (other == cameraChanger.room4)
                {
                    cameraChanger.SwitchCamera(cameraChanger.camera_4);
                }
                else if (other == cameraChanger.room5)
                {
                    cameraChanger.SwitchCamera(cameraChanger.camera_5);
                }
            }
        }

        // End the scene When triggering the object with this tag -- Finlay Macmillan
        if (other.gameObject.CompareTag("End"))
        {
            SceneManager.LoadScene(nextScene);
        }

        if ((other.gameObject.CompareTag("EnemyDeath"))) // Destroys the enemy if in a death zone 
        {
            playerStats.TakeDamage(100);
        }

    }

   

    //Slide - Ben.P
    void FixedUpdate()
    {
        if (!isSliding)
        {
            rb.linearVelocity = new Vector2 (moveValue.x * speed, rb.linearVelocity.y);
        }
    }


    void Jump() // Pushes the player with Force -- Finlay Macmillan
    {
        // Adds an Upwards force to the player
        rb.AddForce(Vector2.up * jumpingPower, ForceMode2D.Impulse); 
    }

    void StartMantle() // Pushes the player up -- Finlay Macmillan
    {
        transform.Translate(Vector2.up * mantlePower);
        
    }

    void Flip() // flips the player's object -- Finlay Macmillan
    {
        if(isFacingRight == true)
        {
            playerScale.x = Mathf.Abs(playerScale.x); // Mathf just makes sure what direction the player should face
        }
        if (isFacingRight == false)
        {
            playerScale.x = -Mathf.Abs(playerScale.x);
        }

        transform.localScale = playerScale;
    }

    void FinishAttacking()
    {
        animator.SetBool(isAttacking, false); // Stops the attack Animation
    }
    
    IEnumerator Slide() // Ben.P
    {
        isSliding = true;

        float direction = transform.localScale.x > 0 ? 1 : -1;

        rb.linearVelocity = new Vector2(direction * slideSpeed, rb.linearVelocity.y);
        //Debug.Log("Sliding");

        // Shrink Collider - Will come back to

        yield return new WaitForSeconds(slideDuration);

        // Restore Collider - Will come back to

        isSliding = false;
    }
    
}

