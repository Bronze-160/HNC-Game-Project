using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour
{
    //These variables are to hold the Action references
    InputAction moveAction;
    InputAction jumpAction;

    [Header("Player Component Refrences")]
    [SerializeField] Rigidbody2D rb;

    [Header("Player Settings")]
    [SerializeField] float speed;
    [SerializeField] float jumpingPower;

    private float direction = 0.8f;
    private Vector2 moveinput;

    public bool isGrounded;

    [Header("Mantle Mechanic")]

    [SerializeField] Transform wallCheck;
    [SerializeField] Transform headCheck;
    [SerializeField] float checkDistance = 0.5f;
    [SerializeField] LayerMask groundLayer;


    bool isTouchingWall;
    bool isTooTall;


    void Start()
    {
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
        Flip();
         
        //sends your movment code to the new input system
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        rb.linearVelocity = new Vector2(moveValue.x * speed, rb.linearVelocity.y);

        //checks if jump button is pressed
        if (jumpAction.triggered && isGrounded == true)
        {
            Jump();
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            direction = .8f;

            isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, checkDistance, groundLayer);
            Debug.DrawRay(wallCheck.position, transform.right, Color.green);

            isTooTall = Physics2D.Raycast(headCheck.position, transform.right, checkDistance, groundLayer);
            Debug.DrawRay(headCheck.position, transform.right, Color.green);

        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            direction = -.8f;

            isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, checkDistance, groundLayer);
            Debug.DrawRay(wallCheck.position, -transform.right, Color.green);

            isTooTall = Physics2D.Raycast(headCheck.position, transform.right, checkDistance, groundLayer);
            Debug.DrawRay(headCheck.position, -transform.right, Color.green);
        }

        if (isTouchingWall && !isTooTall && !isGrounded && jumpAction.triggered)
        {
            StartMantle();
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
        if(Input.GetKeyDown(KeyCode.D))
        {
            direction = .8f;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            direction = -.8f;
        }

        transform.localScale = new Vector3(direction, 2, 1);
    }

}

