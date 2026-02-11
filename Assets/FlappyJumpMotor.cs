using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlappyJumpMotor : MonoBehaviour, IJump
{
    Rigidbody2D rb;
    public float jumpForce = 10;
    public float fallMultiplier = 2.5f;
    public float regularGravity = 1;
    bool willJump = false;
    [Header("Play a sound when jumping?")]
    public AudioClip sound;
    public float soundVolume = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        if(!TryGetComponent<Rigidbody2D>(out rb))
        {
            Debug.LogError("FlappyJumpMotor requires a Rigidbody2D component to work.");
        }
    }

    void FixedUpdate()
    {
        rb.gravityScale = rb.linearVelocity.y < 0 ? fallMultiplier : regularGravity;
        if (willJump)
        {
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            if (sound)
                AudioManager.audioManager?.playAudio(sound, soundVolume);
            willJump = false;
        }
    }
    
    public bool Jump()
    {
        willJump = true;
        return true;
    }

    public bool CheckGround()
    {
        // throw new System.NotImplementedException();
        return false;
    }

    public bool CheckEdge()
    {
        // throw new System.NotImplementedException();
        return false;
    }

    public bool CheckWall()
    {
        // throw new System.NotImplementedException();
        return false;
    }
}
