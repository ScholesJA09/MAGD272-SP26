using UnityEngine;

public class Campfire : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the trigger is the Player
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                // We call the public method directly. 
                // Your PlayerHealth script already handles the "max health" cap!
                playerHealth.fillHealth(playerHealth.maxHealth);

                Debug.Log("Player healed at campfire!");
            }
        }
    }
}