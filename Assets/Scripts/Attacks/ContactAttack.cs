using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactAttack : MonoBehaviour
{
    [Header("Will damage with a trigger or a default collider")]
    
    [Header("Damage attack amount. \n" +
            "Some attack scripts might override damage value.")][Tooltip("Slash attacks will override this value.")]
    public int damage = 1;

    [Header("Does this object damage everything \n it touches or only the player?")]
    public bool onlyDamagePlayer = true;

    [HideInInspector]
    public bool disable = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!disable)
        {
            if ((onlyDamagePlayer && collision.gameObject.GetComponent<PlayerHealth>()) || (!onlyDamagePlayer && collision.gameObject.GetComponent<Health>()))
            {
                print("Contact attack!");
                collision.gameObject.GetComponent<Health>().TakeDamage(damage);
            }
        }
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!disable)
        {
            if ((onlyDamagePlayer && collision.gameObject.GetComponent<PlayerHealth>()) || (!onlyDamagePlayer && collision.gameObject.GetComponent<Health>()))
            {
                print("Contact attack!");
                collision.gameObject.GetComponent<Health>().TakeDamage(damage);
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!disable)
        {
            if ((onlyDamagePlayer && collision.gameObject.GetComponent<PlayerHealth>()) || (!onlyDamagePlayer && collision.gameObject.GetComponent<Health>()))
            {
                print("Contact attack!");
                collision.gameObject.GetComponent<Health>().TakeDamage(damage);
            }
        }
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!disable)
        {
            if ((onlyDamagePlayer && collision.gameObject.GetComponent<PlayerHealth>()) || (!onlyDamagePlayer && collision.gameObject.GetComponent<Health>()))
            {
                print("Contact attack!");
                collision.gameObject.GetComponent<Health>().TakeDamage(damage);
            }
        }
    }

    void Awake()
    {
        if (damage <= 0)
        {
            Debug.LogWarning(gameObject.name + "'s damage is too low! Defaulting to 1...", gameObject);
            damage = 1;
        }

        if (!GetComponent<Collider2D>())
        {
            Debug.LogError("No collider2D found on " + gameObject.name + "! Add one for this to deal damage.", gameObject);
            enabled = false;
        }
    }
}
