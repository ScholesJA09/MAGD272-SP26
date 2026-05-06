using UnityEngine;

public class NoTriggerEnemyDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damage = 10;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryDamagePlayer(collision.gameObject);
    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    // Optional: allows continuous damage attempts (immunity will block repeats)
    //    TryDamagePlayer(collision.gameObject);
    //}

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    TryDamagePlayer(other.gameObject);
    //}

    //private void OnTriggerStay2D(Collider2D other)
    //{
    //    // Optional: same idea for triggers
    //    TryDamagePlayer(other.gameObject);
    //}

    void TryDamagePlayer(GameObject obj)
    {
        if (!obj.CompareTag("Player")) return;

        PlayerHealth player = obj.GetComponent<PlayerHealth>();

        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }
}