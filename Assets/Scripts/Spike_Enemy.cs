using UnityEngine;

public class Spike_Enemy : MonoBehaviour
{
    /// <summary>
    /// Spike Script 
    /// By Finlay Macmillan
    /// </summary>


    [SerializeField] int spikeDamage = 5; // Stores the spikes damage 

    public Player_Stats playerStats; // references to PlayerStats script

    void OnTriggerEnter2D(Collider2D other) // If the player land on the spikes then they take damage
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerStats.TakeDamage(spikeDamage);
        }
    }
}
