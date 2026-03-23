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

    public bool isGrounded;

    void Start()
    {
        // Looks for the players Rigidbody
        rb = GetComponent<Rigidbody2D>();
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
        //sends your movment code to the new input system
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        rb.linearVelocity = new Vector2(moveValue.x * speed, rb.linearVelocity.y);

        //checks if jump button is pressed
        if (jumpAction.triggered && isGrounded == true)   
        {
            Jump();
        }
    }

    void Jump()
    {
        // Adds an Upwards force to the player
        rb.AddForce(Vector2.up * jumpingPower, ForceMode2D.Impulse); 
    }
}

