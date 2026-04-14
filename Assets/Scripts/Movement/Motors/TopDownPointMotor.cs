using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TopDownPointMotor : MonoBehaviour, IMove
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
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // 1. Calculate horizontal distance only
        float xDistance = destination.x - transform.position.x;
        float stoppingDistance = 0.5f; // Adjust this if the slime "overshoots"

        // 2. Check if we are far enough away to need to move
        if (Mathf.Abs(xDistance) > stoppingDistance)
        {
            float moveDir = Mathf.Sign(xDistance);

            // 3. Move horizontally but PRESERVE gravity (rb.linearVelocity.y)
            rb.linearVelocity = new Vector2(moveDir * speed, rb.linearVelocity.y);

            UpdateAnimations(moveDir, 0);
        }
        else
        {
            // 4. We reached the point! Stop horizontal movement, keep gravity.
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            UpdateAnimations(0, 0);
        }
    }




    public void Move(Vector2 position){
        destination = position;
    }

    public void UpdateAnimations(float horizontal, float vertical) {
        if (animators.Length > 0){
            foreach (Animator a in animators){
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
