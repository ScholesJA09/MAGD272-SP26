using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TopDownRotatorMotor : MonoBehaviour, IMove
{
    Rigidbody2D rb;
    [SerializeField] Animator[] animators;

    IAttack<Health>[] attackScrpits;

    [field: SerializeField]
    public float speed { get; set; } = 10f;
    public float rotationSpeed = 100f;


    void Start()
    {
        attackScrpits = GetComponents<IAttack<Health>>();
        rb = GetComponent<Rigidbody2D>();
    }


    public void Move(Vector2 direction)
    {
        if (Mathf.Abs(direction.y) > .01f){
            rb.linearVelocity = transform.up*speed* direction.y;
        }
        else{
            rb.linearVelocity = Vector2.zero;
        }

        transform.Rotate(new Vector3(0, 0, -1f) * direction.x * rotationSpeed * Time.deltaTime);
        rb.angularVelocity = 0f;

        UpdateAnimations(direction.x, rb.linearVelocity.normalized.y);

    }

    public void UpdateAnimations(float horizontal, float vertical){
        if (animators.Length > 0){
            foreach (Animator a in animators){
                a.SetFloat("HorizontalSpeed", horizontal);
                a.SetFloat("VerticalSpeed", vertical);
            }
        }

        if (attackScrpits.Length > 0) {
            foreach (IAttack<Health> attack in attackScrpits) {
                attack.SetDirection(new Vector2(horizontal, vertical).normalized);
            }
        }
    }
}
