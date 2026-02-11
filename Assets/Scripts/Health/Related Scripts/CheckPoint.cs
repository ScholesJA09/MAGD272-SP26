using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class CheckPoint : MonoBehaviour
{
    [Header("These bools set themselves")]
    [Header("Adjust the Box Collider above as needed")]
    public bool used;

    public bool active;

    [Header("Do you want to play a sound when interacting? Add an audio clip")]
    public AudioClip sound;

    public float soundVolume = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Respawn>() && !used)
        {
            collision.GetComponent<Respawn>().unactivateCheckpoints();
            used = active = true;
            Debug.Log("Checkpoint updated.", gameObject);

            // trigger audio event
            if (sound != null)
                AudioManager.audioManager?.playAudio(sound, soundVolume);
        }
    }

    private void Awake()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
        GetComponent<Rigidbody2D>().gravityScale = 0;
    }
}
