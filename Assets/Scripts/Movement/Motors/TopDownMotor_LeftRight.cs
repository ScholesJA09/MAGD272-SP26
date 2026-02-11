using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TopDownMotor_LeftRight : MonoBehaviour, IMove
{
    [Header("This script is required for any top-down movement")]
    [Header("This does not flip the sprite.")]
    Rigidbody2D rb;
    [SerializeField] Animator[] animators;
    IAttack<Health>[] attackScripts;
    public bool enableVerticalMovement = false;
    
    void Start() {
        attackScripts = GetComponents<IAttack<Health>>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    [field: SerializeField]
    public float speed { get; set; }

    public void Move(Vector2 direction) {
        if (!enableVerticalMovement)
        {
            direction = new Vector2(direction.x, 0);
        }
        if (direction.sqrMagnitude < .01f){
            rb.linearVelocity = Vector2.zero;
            UpdateAnimations(direction.x, direction.y);
        }
        else {
            rb.linearVelocity = direction.normalized * speed;
            UpdateAnimations(direction.normalized.x, direction.normalized.y);
        }
    }

    public void UpdateAnimations(float horizontal, float vertical) {
        if (animators.Length > 0) {
            foreach (Animator a in animators) {
                a.SetFloat("HorizontalSpeed", horizontal);
                a.SetFloat("VerticalSpeed", vertical);
                a.SetFloat("AbsHorizontalSpeed", Mathf.Abs(horizontal));
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
