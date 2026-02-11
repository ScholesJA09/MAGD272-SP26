using UnityEngine;

public class TowerDefenseEnemyMotor : MonoBehaviour, IMove
{
    Rigidbody2D rb;
    Vector2 destination;
    [SerializeField] Animator[] animators;

    IAttack<Health>[] attackScripts;

    [field: SerializeField]
    public float speed { get; set; } = 10f;

    void Start()
    {
        destination = transform.position;
        attackScripts = GetComponents<IAttack<Health>>();
        rb = GetComponent<Rigidbody2D>() != null ? GetComponent<Rigidbody2D>() : gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    void FixedUpdate()
    {
        Vector2 direction = new Vector2(destination.x - transform.position.x, destination.y - transform.position.y);
        if (direction.sqrMagnitude > .01f)
        {
            rb.linearVelocity = direction.normalized * speed;
            UpdateAnimations(direction.normalized.x, direction.normalized.y);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            UpdateAnimations(0, 0);
        }
    }


    public void Move(Vector2 position)
    {
        destination = position;
    }

    public void UpdateAnimations(float horizontal, float vertical)
    {
        if (animators == null) return;

        if (animators.Length > 0)
        {
            foreach (Animator a in animators)
            {
                a.SetFloat("HorizontalSpeed", horizontal);
                a.SetFloat("VerticalSpeed", vertical);
            }
        }

        if (attackScripts.Length > 0)
        {
            foreach (IAttack<Health> attack in attackScripts)
            {
                attack.SetDirection(new Vector2(horizontal, vertical).normalized);
            }
        }
    }
}