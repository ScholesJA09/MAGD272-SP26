using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    private AudioSource source;
    public AudioClip appearSFX;
    public AudioClip attackSFX;
    public AudioClip deathSFX;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // These functions will be called by the Animator
    public void PlayAppearSound()
    {
        source.PlayOneShot(appearSFX);
    }

    public void PlayAttackSound()
    {
        source.PlayOneShot(attackSFX);
    }

    public void PlayDeathSound()
    {
        source.PlayOneShot(deathSFX);
    }
}