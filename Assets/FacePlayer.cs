using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    public Transform player; // Drag your Player object here in the Inspector
    private bool isFacingRight = false;

    void Update()
    {
        // 1. Check if the player reference is missing
        if (player == null)
        {
            FindPlayer();
        }

        // 2. Only run logic if we successfully found a player
        if (player != null)
        {
            CheckDirection();
        }
    }

    void FindPlayer()
    {
        // Search for the GameObject tagged "Player"
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void CheckDirection()
    {
        // If player is to the right and enemy is facing left
        if (player.position.x > transform.position.x && !isFacingRight)
        {
            Flip();
        }
        // If player is to the left and enemy is facing right
        else if (player.position.x < transform.position.x && isFacingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;

        // Multiply the x-scale by -1 to flip the sprite
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
