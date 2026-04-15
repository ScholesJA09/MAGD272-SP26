using UnityEngine;

public class SpellSFX : MonoBehaviour
{
    public AudioClip clip;
    private AudioSource audioSource;

     void Start()
     {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null) 
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
     }
}
