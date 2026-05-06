using UnityEngine;

public class HeartHeal : MonoBehaviour
{
    [Header("Heal Settings")]
    public int healAmount = 2;

    [Header("Effects")]
    public GameObject collectEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            // Use the new public function we just made
            if (playerHealth != null && playerHealth.CanHeal())
            {
                playerHealth.fillHealth(healAmount);

                Debug.Log("Player healed by heart.");

                if (collectEffect != null)
                {
                    Instantiate(collectEffect, transform.position, Quaternion.identity);
                }

                Destroy(gameObject);
            }
            else if (playerHealth != null)
            {
                Debug.Log("Player already at max health.");
            }
        }
    }
}