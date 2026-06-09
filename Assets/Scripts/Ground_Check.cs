using UnityEngine;

public class Ground_Check : MonoBehaviour
{
    /// <summary>
    /// Ground Check Script 
    /// By Finlay Macmillan
    /// </summary>

    [SerializeField] LayerMask groundLayer; // Stores the Ground layer
    [SerializeField] LayerMask platformLayer; // Stores the Platfrom layer

    public Player_Controller playerController; // Reference to main player script

    void OnTriggerEnter2D(Collider2D collision) // if colliding wit objects with these tags the player is grounded meaning they can jump
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            playerController.isGrounded = true;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            playerController.onPlatfrom = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision) // if not with objects with these tags the player is not grounded meaning they can't jump (except when using double Jump)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            playerController.isGrounded = false;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            playerController.onPlatfrom = false;
        }
    }
}
