using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColliderSpecificDamage : MonoBehaviour
{
    [Header("Will damage with a specific collider2d, onTrigger or a default collider")]
    public Collider2D[] colliders;

    [Header("Damage attack amount. \n" +
            "Some attack scripts might override damage value.")]
    [Tooltip("Slash attacks will override this value.")]
    public int damage = 1;

    [Header("Does this object damage everything \n it touches or only the player?")]
    public bool onlyDamagePlayer = true;

    [HideInInspector] public bool disable = false;

    void Awake()
    {
        if (damage <= 0)
        {
            Debug.LogWarning(gameObject.name + "'s damage is too low! Defaulting to 1...", gameObject);
            damage = 1;
        }

        if (!GetComponent<Collider2D>())
        {
            Debug.LogError("No collider2D found on " + gameObject.name + "! Add one for this to deal damage.",
                gameObject);
            enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (disable)
        {
            return;
        }

        List<Collider2D> collisions = new List<Collider2D>();
        // foreach (Collider2D collider in colliders)
        // {
        //     Physics2D.OverlapCollider(collider, new ContactFilter2D(), collisions);
        // }
        
        foreach (Collider2D collider in colliders)
        {
            collider.GetContacts(new ContactFilter2D(), collisions);
        }

        collisions = collisions.Distinct().ToList();

        foreach (Collider2D collider in collisions)
        {
            // Check if the collider is not the same as the current object
            if (collider.gameObject == gameObject)
            {
                continue;
            }

            // Check if the collider is not already in the list
            if ((onlyDamagePlayer && collider.gameObject.GetComponent<PlayerHealth>()) ||
                (!onlyDamagePlayer && collider.gameObject.GetComponent<Health>()))
            {
                print("Contact attack!");
                collider.gameObject.GetComponent<Health>().TakeDamage(damage);
            }
        }
    }
}