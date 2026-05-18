using Unity.Cinemachine;
using UnityEditor.PackageManager;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Enemy_Follow : MonoBehaviour
{
    /// <summary>
    /// Enemy Follow Room Script 
    /// By Finlay Macmillan
    /// </summary>

    [Header("Other Scripts")]
    public CameraChanger cameraChanger; // references to CameraChanger script

    [Header("Tracking Target settings")]
    [SerializeField] float speed = 5f; // How fast the Enemy to move towards the target
    [SerializeField] int enemyRoom; // equals what room the Enemy is in
    [SerializeField] float offset = 2;
    private string targetTag = "Player"; // Tag to look for 
    private Transform target; // Object to move towards 
    private int playerRoom; // equals what room the player is in
    private bool shouldmove = false; // checks if the Enemy should move
    private float targetX; // holds the Enemy's x position

    [SerializeField] Animator animator; // links the script to the Unity animator
    [SerializeField] string isAttacking = "isAttacking"; // Peramitor if player is Attacking 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag(targetTag); // Finds the player object 
        target = player.transform; // sets the target the enemy should follow to the players transform
    }

    // Update is called once per frame
    void Update()
    {
        playerRoom = cameraChanger.currentCameraNumber; // sets the playerRoom value to what camera the player is currently at

        if (playerRoom == enemyRoom)// checks if the enemy and player are in the same room
        {
            shouldmove = true; // the enemy should move
        }
        else
        {
            shouldmove = false; // the enemy should not move
        }

        // Mathf.Sign checks what side the enemy is on to the player
        float direction = Mathf.Sign(transform.position.x - target.position.x);
        Vector3 enemyScale = new Vector3(-direction, transform.localScale.y, transform.localScale.z);
        transform.localScale = enemyScale;
        float dynamicOffset = offset * direction;
        

        if (shouldmove == true && !cameraChanger.CinemachineBrain.IsBlending)
        {
            // Moves the Enemy towards the target at a set speed 
            targetX = Mathf.MoveTowards(transform.position.x, target.position.x + dynamicOffset, speed * Time.deltaTime);
            transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
        }

        if (transform.position.x == target.position.x + dynamicOffset)
        {
            
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Attacking");
            Attacking();
        }
    }


    void Attacking()
    {
        animator.SetBool(isAttacking, true);
    }

    void FinishAttacking()
    {
        animator.SetBool(isAttacking, false);
    }

   
}
