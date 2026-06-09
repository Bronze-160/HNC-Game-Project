using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Health Bar Script 
/// By Finlay Macmillan
/// </summary>
/// 
public class Health_Bar : MonoBehaviour
{
   public Slider healthSlider;

    public void SetMaxHealth(int health) // Sets the max health to what the sliders max is 
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;
    }
    

   public void SetHealth(int health) // Sets Health to whatever is stated when the function is called
   {
        healthSlider.value = health;
   }
}
