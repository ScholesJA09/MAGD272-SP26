using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformLauncher : MonoBehaviour
{
    [Header("Set Collider to a layer that Character does NOT consider 'ground'")]
    [SerializeField] float jumpForce;

    private void Start()
    {
        if (TryGetComponent<Collider2D>(out var coll) == false)
        {
            Debug.Log("PlatformLauncher.cs on " + gameObject.name + "needs a collider2D.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("on ground? " + collision.GetComponent<JumpMotor>().CheckGround());
            if (!collision.GetComponent<JumpMotor>().CheckGround())
            {
                collision.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
                collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
            }
        }
    }
}
