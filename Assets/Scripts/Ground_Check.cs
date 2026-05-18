using UnityEngine;

public class Ground_Check : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask platformLayer;

    public Player_Controller playerController; // Reference to main player script

    void OnTriggerEnter2D(Collider2D collision)
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
    void OnTriggerExit2D(Collider2D collision)
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
