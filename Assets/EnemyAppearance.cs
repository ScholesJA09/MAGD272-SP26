using UnityEngine;

public class EnemyAppearance : MonoBehaviour
{
    [Header("Settings")]
    public float wakeUpRange = 5f;
    public string appearTriggerName = "Appear";

    private bool hasAppeared = false;
    private Animator anim;
    private EnemyAttackManager attackManager;
    private GameObject player;

    void Start()
    {
        anim = GetComponent<Animator>();
        attackManager = GetComponent<EnemyAttackManager>();

        // Find the player by tag
        player = GameObject.FindGameObjectWithTag("Player");

        // Disable attacks immediately so the ghost doesn't fire while invisible
        if (attackManager != null)
        {
            attackManager.attacksEnabled = false;
        }

        // Optional: Hide the sprite until it's time to appear
        // GetComponent<SpriteRenderer>().enabled = false;
    }

    void Update()
    {
        if (player == null) return;

        // Check distance between Ghost and Player
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (!hasAppeared && distance < wakeUpRange)
        {
            Appear();
        }
    }

    void Appear()
    {
        hasAppeared = true;

        // Turn the visuals back on!
        if (GetComponent<SpriteRenderer>() != null)
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }

        if (anim != null)
        {
            anim.SetTrigger(appearTriggerName);
        }

        Invoke(nameof(EnableCombat), 1.0f);
    }

    void EnableCombat()
    {
        if (attackManager != null)
        {
            attackManager.attacksEnabled = true;
        }
    }
}