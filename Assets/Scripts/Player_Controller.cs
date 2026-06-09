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
    InputAction grappleAction; // Not Used
    InputAction attackAction;
    InputAction doubleJumpAction; // Ross

    private Scene activeScene; // Stores the Active Scene
 // [SerializeField] string nextScene = "Death"; // Stores what the next scene will be and can be changed


    [Header("Other Scripts")] // Referance to other Script
    public CameraChanger cameraChanger;
    public Player_Stats playerStats; // references to PlayerStats script
    public Level_Loading levelLoading;


    [Header("Player Component Refrences")]
    [SerializeField] Rigidbody2D rb;

    [Header("Player Settings")] // -- Finlay Macmillan
    [SerializeField] int attackDamage = 15; // Stores players damage
    [SerializeField] float speed; // Stores players Speed 
    [SerializeField] float jumpingPower;// Stores players Jump Power
    [SerializeField] Vector3 playerScale; // Stores the players scale
    [SerializeField] int jumpCount = 1;
    [SerializeField] Animator animator; // links the script to the Unity animator

    public Vector2 moveValue;

    //Check if Player is on the ground/ a Platform
    public bool isGrounded;
    public bool onPlatfrom;
        
    [Header("Attack Settings")]
    [SerializeField] int hitForce = 5;
    private bool inRange = false;// Checks to see if enemy is in range of player's to attack

    public Transform attackPoint;
    public float radius = 1f;



    [Header("Animation")] // names for peramitors of the animation - Names -- Finlay Macmillan
    [SerializeField] string isRunning = "isRunning"; // Peramitor if player is Moving 
    [SerializeField] string isJumping = "isJumping"; // Peramitor if player is Jumping
    [SerializeField] string isAttacking = "isAttacking"; // Peramitor if player is Attacking 
    public string isHit = "isHit";// Peramitor if player is Hit 



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


    IEnumerator Start()
    {
        Time.timeScale = 1f;
        activeScene = SceneManager.GetActiveScene();
        Debug.Log("Active Scene is '" + activeScene.name + "'."); // Tells Console What the Active Scene Is called

        playerScale = transform.localScale; // Makes sure the obejcts scale is the players scale

        // Referance to find each action
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        grappleAction = InputSystem.actions.FindAction("Grapple");
        attackAction = InputSystem.actions.FindAction("Attack");
        doubleJumpAction = InputSystem.actions.FindAction("DJump");
        //EditorApplication.isPaused = false;

        if (activeScene.name == "Level_3") // -- Finlay Macmillan
        {

            //starts on Starts camera
            cameraChanger.SwitchCamera(cameraChanger.cameraStart);

            Debug.Log("Switched to Start Camera");
            Debug.Log("Current Camera: " + cameraChanger.currentCamera.name);

            // wait's a frame so Unity can register the code 
            yield return null;

            //Blends to Next camera
            cameraChanger.SwitchCamera(cameraChanger.camera_1);

        }
    }

    /*
    // makes sure Inputs don't Break //
    void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        grappleAction.Enable();
        attackAction.Enable();
        doubleJumpAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        grappleAction.Disable();
        attackAction.Disable();
        doubleJumpAction.Disable();
    }
    // //
    */
    void Update()
    {
        if (attackAction.triggered)
        {
            Attack();
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

        if (!isGrounded && !onPlatfrom && jumpAction.triggered && jumpCount == 1)
        {
            DJump();
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

        //Slide Mechanic -- Ben.P
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isSliding)
        {
            StartCoroutine(Slide());
        }



    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (activeScene.name == "Level_3") // -- Finlay Macmillan
        {
            //This blends the camera between rooms with colliding with the room trigger
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

        // End the scene When triggering the object with this tag -- Finlay Macmillan
        if (other.gameObject.CompareTag("End"))
        {
            levelLoading.LoadLevel(3);
        }

        if (other.gameObject.CompareTag("Next"))
        {
            levelLoading.LoadLevel(2);
        }


        if ((other.gameObject.CompareTag("EnemyDeath"))) // Destroys the enemy if in a death zone 
        {
            playerStats.TakeDamage(100);
        }

        if (other.gameObject.CompareTag("Enemy")) // If the player is in the trigger zone, then they are in range
        {
            inRange = true;
        }
        if (attackAction.triggered && inRange == true)
        {
            if (isFacingRight == true)
            {
                other.attachedRigidbody.AddForce(transform.right * hitForce);
                Debug.Log("Hit");
            }
            else if (isFacingRight == false)
            {
                other.attachedRigidbody.AddForce(-transform.right * hitForce);
                Debug.Log("Hit");
            }
        }

    }

    void OnTriggerExit2D(Collider2D other) // If the player leaves the trigger zone, then they are not in range
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            inRange = false;
            animator.SetBool(isAttacking, false);
        }
    }



    //Slide - Ben.P
    void FixedUpdate()
    {
        if (!isSliding)
        {
            rb.linearVelocity = new Vector2(moveValue.x * speed, rb.linearVelocity.y);
        }
    }


    void Jump() // Pushes the player with Force -- Finlay Macmillan
    {
        // Adds an Upwards force to the player
        rb.AddForce(Vector2.up * jumpingPower, ForceMode2D.Impulse);
        jumpCount = 1; // Ross
    }

    void DJump() // Ross
    {
        rb.AddForce(Vector2.up * jumpingPower, ForceMode2D.Impulse);
        jumpCount = 0;
    }

    void StartMantle() // Pushes the player up -- Finlay Macmillan
    {
        transform.Translate(Vector2.up * mantlePower);

    }

    void Flip() // flips the player's object -- Finlay Macmillan
    {
        if (isFacingRight == true)
        {
            playerScale.x = Mathf.Abs(playerScale.x); // Mathf just makes sure what direction the player should face
        }
        if (isFacingRight == false)
        {
            playerScale.x = -Mathf.Abs(playerScale.x);
        }

        transform.localScale = playerScale;
    }

    void Attack()
    {
        animator.SetBool(isAttacking, true); // Attack Animation Play's
    }

    void AttackHit() // if hit the enemy will take damage, Then the player stops attacking. This gives the player a cooldown
    {

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, radius);
        Debug.Log("hit");
        
        foreach (var hit in hits)
        {
            hit.GetComponent<Enemy_Stats>()?.EnemyTakeDamage(attackDamage);
            Rigidbody2D enemyRb = hit.GetComponent<Rigidbody2D>();

            if (enemyRb != null)
            {
                if (isFacingRight == true)
                {
                    enemyRb.AddForce(transform.right * hitForce);
                }
                else if (isFacingRight == false)
                {
                    enemyRb.AddForce(-transform.right * hitForce);

                }
            }
        }

       
    }
    void FinishAttacking()
    {
        animator.SetBool(isAttacking, false); // Stops the attack Animation
    }


    void EndOfHit()
    {
        animator.SetBool(isHit, false); // Stops the hit Animation
    }

    // Not Working
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, radius);
    }

}

