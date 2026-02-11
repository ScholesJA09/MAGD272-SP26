using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableMotor : MonoBehaviour
{
    public LinearMover mover;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            mover.gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            mover.enabled = false;
        }
    }
}
