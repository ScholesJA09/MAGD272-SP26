using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroySound : MonoBehaviour
{
    [Header("Do you want sound to play when the object disables or destroys? Drag audio in below")]
    public AudioClip sound;
    [Header("Adjust the volume of that sound")]
    public float soundVolume = 1f;

    private void OnDestroy()
    {
        playSound();
    }

    private void OnDisable()
    {
        playSound();
    }

    private void playSound()
    {
        if (sound == null)
        {
            return;
        }
        else
        {
            AudioManager.audioManager.playAudio(sound, soundVolume);
        }
    }
}
