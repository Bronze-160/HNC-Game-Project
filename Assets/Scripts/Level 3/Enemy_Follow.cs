using System.Collections;
using UnityEngine;

public class Enemy_Follow : MonoBehaviour
{
    /// <summary>
    /// Enemy Follow Room Script 
    /// By Finlay Macmillan
    /// </summary>

    [Header("Other Scripts")]
    public CameraChanger cameraChanger; // references to CameraChanger script
    public Player_Stats playerStats; // references to PlayerStats script

    [Header("Enemy Stats")]
    public int health = 10; // equals how much health the Enemy has
    [SerializeField] int enemyDamage = 10; // How much damage the enemy does to the player
    [SerializeField] int coolDown = 1; // Cooldown on when the enemy can swing
    private bool canAttack = true; // check if the enemy can attack

    [Header("Tracking Target settings")]
    [SerializeField] float speed = 5f; // How fast the Enemy to move towards the target
    [SerializeField] int enemyRoom; // equals what room the Enemy is in
    [SerializeField] float offset = 2;  // How far away from the player
    private string targetTag = "Player"; // Tag to look for 
    private Transform target; // Object to move towards 
    private int playerRoom; // equals what room the player is in
    private bool shouldmove = false; // checks if the Enemy should move
    private float targetX; // holds the Enemy's x position

    private bool inRange = false;// Checks to see if player is in range of enemy to attack

    [SerializeField] Animator animator; // links the script to the Unity animator
    [SerializeField] string isAttacking = "isAttacking"; // Peramitor if player is Attacking 
    [SerializeField] string isRunning = "isRunning"; // Peramitor if player is mRunning 

     

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator.SetBool(isRunning, false); // Makes sure the enemy doesn't run at the start of the game
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
            animator.SetBool(isRunning, false); // Stops the enemy from moving if not in the same room
            shouldmove = false; // the enemy should not move
        }

        // Mathf.Sign checks what side the enemy is on to the player
        float direction = Mathf.Sign(transform.position.x - target.position.x);
        Vector3 enemyScale = new Vector3(-direction, transform.localScale.y, transform.localScale.z); // sets what way the enemy should face
        transform.localScale = enemyScale; // Flips the enemy depending where the player is (Left or Right Side) 
        float dynamicOffset = offset * direction; // this makes sure that the enemy is running to the correct side of the player         

        if (shouldmove == true && !cameraChanger.CinemachineBrain.IsBlending)
        {
            // Moves the Enemy towards the target at a set speed 
            targetX = Mathf.MoveTowards(transform.position.x, target.position.x + dynamicOffset, speed * Time.deltaTime); // Moves the Enemy towards the player
            transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
            animator.SetBool(isRunning, true); // Starts the running animation
        }

        if (cameraChanger.CinemachineBrain.IsBlending)
        {
            animator.SetBool(isRunning, false); // Keeps the enemy idle when blending between cameras (Switching)
        }


        if (inRange == true && canAttack == true && !cameraChanger.CinemachineBrain.IsBlending)
        {
            Attacking(); // Starts the attack function if player is in range, if the enemy is able to attack and the camera is not blending (Switching)

        }

        if (inRange)
        {
            animator.SetBool(isRunning, false); // Stops the running animation
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")) // If the player is in the trigger zone, then they are in range
        {
           inRange = true;
        }
    }
    void OnTriggerExit2D(Collider2D other) // If the player leaves the trigger zone, then they are not in range
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inRange = false;
            animator.SetBool(isAttacking, false);
            canAttack = true;
        }
        if ((other.gameObject.CompareTag("EnemyDeath"))) // Destroys the enemy if in a death zone 
        {
            Destroy(gameObject);
        }
    }


    void Attacking() //stops running and starts the attack animation
    {
        animator.SetBool(isRunning, false);
        canAttack = false;
        animator.SetBool(isAttacking, true);
    }

    void FinishAttacking() // if finished the player will take damage, Then the enemy stops attacking. This gives the enemy a cooldown
    {
        playerStats.TakeDamage(enemyDamage);
        animator.SetBool(isAttacking, false);

        if (inRange == true) // keeps attacking (animation loops)
        {
            StartCoroutine(AttackCooldown());
        }
        else
        {
            canAttack = false; // stop attacking
        }
        
    }

    IEnumerator AttackCooldown() // waits untill the cooldown time
    {
        yield return new WaitForSeconds(coolDown);
        Debug.Log(coolDown);
        canAttack = true;
    }

   
}
