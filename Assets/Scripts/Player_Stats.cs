using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Stats : MonoBehaviour
{

    /// <summary>
    /// Health Script 
    /// By Finlay Macmillan
    /// </summary>

    [Header("Other Scripts")] // refrenace to other scrips
    public Player_Controller player_Controller; 

    [Header("Player Stats")] 
    public int maxHealth = 100; // Stores the players Max health
    public int currentHealth; // Stores the current health

    public Health_Bar healthBar; // refrenace to the Health UI

    [Header("Audio")]
    [SerializeField] AudioClip swordHit;
    [SerializeField] AudioClip death;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth; 
        healthBar.SetMaxHealth(maxHealth); // sets the UI to the max Health
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0) // If player's health is 0 then the player dies
        {
            SceneManager.LoadScene("Death");
            AudioSource.PlayClipAtPoint(death, transform.position, .3f);

        }
    }

    public void TakeDamage(int damage) // Function so when called will inflict damage 
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        AudioSource.PlayClipAtPoint(swordHit, transform.position, 1f);
        
        
    }

    
}
