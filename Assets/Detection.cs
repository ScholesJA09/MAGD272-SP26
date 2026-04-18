using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. Trigger the animation
            GetComponent<Animator>().SetTrigger("Appear");

            // 2. Enable the shooting script
            ShootAllDirection shooter = GetComponent<ShootAllDirection>();
            if (shooter != null)
            {
                shooter.enabled = true;
            }
        }
    }
}