using System.Collections;
using UnityEngine;

public class LimitedLifetime : MonoBehaviour
{
    public float lifetime = 5f; // Lifetime in seconds
    public bool damageToExpire = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(lifetimeCoroutine());
    }

    IEnumerator lifetimeCoroutine()
    {
        yield return new WaitForSeconds(lifetime);
        if (damageToExpire)
        {
            var damageable = GetComponent<IDamagable>();
            damageable?.TakeDamage(9999); // Apply a large amount of damage to ensure destruction
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
