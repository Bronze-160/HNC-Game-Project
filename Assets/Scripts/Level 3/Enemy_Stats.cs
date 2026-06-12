using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy_Stats : MonoBehaviour
{

    /// <summary>
    /// Enemy Health Script 
    /// By Finlay Macmillan
    /// </summary>

    [Header("Enemy Stats")]
    public int enemymaxHealth = 100; // Stores the enemies Max health
    public int enemyCurrentHealth; // Stores the current health

    //public Health_Bar healthBar; // refrenace to the Health UI

    [SerializeField] AudioClip swordHit;
    [SerializeField] AudioClip death;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyCurrentHealth = enemymaxHealth;

    }

    // Update is called once per frame
    void Update()
    {
        if (enemyCurrentHealth <= 0) // If player's health is 0 then the player dies
        {
            AudioSource.PlayClipAtPoint(death, transform.position, .6f);
            Destroy(gameObject);
            
        }
    }

    public void EnemyTakeDamage(int damage) // Function so when called will inflict damage 
    {
        enemyCurrentHealth -= damage;
        Debug.Log(enemyCurrentHealth);
        AudioSource.PlayClipAtPoint(swordHit, transform.position, 1f);
    }


}