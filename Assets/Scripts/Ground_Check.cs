using UnityEngine;

public class Ground_Check : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;
    public Player_Controller playerController; // Reference to main player script

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            playerController.isGrounded = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            playerController.isGrounded = false;
        }
    }
}
