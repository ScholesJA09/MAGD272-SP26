using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ledgeFall : MonoBehaviour
{

    [SerializeField] string playersTag;
    [SerializeField] float fallDelay;
    [SerializeField] bool disableColliderWhenFalling = false;
    [SerializeField] float fallGravityScale = 1;
    [SerializeField] float destroyTimeAfterFall = 0;
    bool triggered = false;

    Rigidbody2D rb;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag(playersTag))
        {
            print("ledgeFall triggered");
            if (!triggered)
            {
                triggered = true;
                StartCoroutine(fall());
            }
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag(playersTag))
        {
            print("ledgeFall triggered");
            if (!triggered)
            {
                triggered = true;
                StartCoroutine(fall());
            }
        }
    }

    IEnumerator fall()
    {
        yield return new WaitForSeconds(fallDelay);
        if (disableColliderWhenFalling)
        {
            Collider2D[] colliders = GetComponents<Collider2D>();
            foreach (var coll in colliders)
            {
                coll.enabled = false;
            }
        }

        if(TryGetComponent(out Rigidbody2D rigidbody2D))
        {
            rb = rigidbody2D;
        } else
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.gravityScale = fallGravityScale;

        yield return new WaitForSeconds(destroyTimeAfterFall);
        Destroy(gameObject);


        yield return true;
    }

}
