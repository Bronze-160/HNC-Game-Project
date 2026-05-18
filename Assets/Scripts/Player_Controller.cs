using Unity.Cinemachine;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour
{
    //These variables are to hold the Action references
    InputAction moveAction;
    InputAction jumpAction;

    [Header("Other Scripts")]
    public CameraChanger cameraChanger;

    [Header("Player Component Refrences")]
    [SerializeField] Rigidbody2D rb;

    [Header("Player Settings")]
    [SerializeField] float speed;
    [SerializeField] float jumpingPower;
    [SerializeField] Vector3 playerScale;

    public Vector2 moveValue;

    public bool isGrounded;
    public bool onPlatfrom;
    //public bool doubleJump;

    // Temp //
    private Animator animator; // links the script to the Unity animator
    
    [Header("Animation")] // names for peramitors of the animation - Names

    [SerializeField] string isRunning = "isRunning"; // Peramitor if player is Moving 
    [SerializeField] string isJumping = "isJumping"; // Peramitor if player is Jumping
    [SerializeField] string isAttacking = "isAttacking"; // Peramitor if player is Attacking 

    //      // 
    [Header("Mantle Mechanic")]

    [SerializeField] Transform wallCheck;
    [SerializeField] Transform headCheck;
    [SerializeField] float checkDistance = 0.2f;
    [SerializeField] LayerMask groundLayer;
    private bool isFacingRight = false;
    bool isTouchingWall;
    bool isTooTall;




    void Start()
    {
        animator = GetComponent<Animator>();
        playerScale = transform.localScale;
        // Referance to find each action
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool(isAttacking, true);
        }

        if (moveValue.x >= 1 || moveValue.x <= -1)
        {
            animator.SetBool(isRunning, true);
        }
        else
        {
            animator.SetBool(isRunning, false);
        }

        //sends your movment code to the new input system
        moveValue = moveAction.ReadValue<Vector2>();
        rb.linearVelocity = new Vector2(moveValue.x * speed, rb.linearVelocity.y);

        //checks if jump button is pressed
        if (jumpAction.triggered && isGrounded == true || jumpAction.triggered && onPlatfrom == true)
        {
            animator.SetTrigger("Jump");
            Jump();
        }

        /*
        if (jumpAction.triggered && doubleJump == true && isGrounded == false   )
        {
            Jump();
            doubleJump = false;
        }
        
        */

        if (isGrounded == true || onPlatfrom == true)
        {
            animator.SetBool(isJumping, false);
            //   doubleJump = true;
        }
        else
        {
            animator.SetBool(isJumping, true);
        }


        if (moveValue.x > 0) { isFacingRight = true; }
        if (moveValue.x < 0) { isFacingRight = false; }

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

        if (isTouchingWall && !isTooTall && !isGrounded && jumpAction.triggered)
        {
            StartMantle();
        }

        Flip();

        if (cameraChanger.currentCamera == cameraChanger.cameraStart)
        {
            cameraChanger.SwitchCamera(cameraChanger.camera_1);
        }
    }

    void Jump()
    {
        // Adds an Upwards force to the player
        rb.AddForce(Vector2.up * jumpingPower, ForceMode2D.Impulse); 
    }

    void StartMantle()
    {
        transform.Translate(Vector2.up * 2);
        
    }

    void Flip()
    {
        if(isFacingRight == true)
        {
            playerScale.x = Mathf.Abs(playerScale.x);
        }
        if (isFacingRight == false)
        {
            playerScale.x = -Mathf.Abs(playerScale.x);
        }

        transform.localScale = playerScale;
    }

    void FinishAttacking()
    {
        animator.SetBool(isAttacking, false);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
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
        }

    }

}

