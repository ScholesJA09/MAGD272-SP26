using UnityEngine;

[System.Serializable]
public enum PowerupType
{
    // Health,
    Speed,
    Invincibility,
    ProjectileSpeed
}

public class Powerup : MonoBehaviour
{
    [Header("Include a collider with 'Is Trigger' checked to detect player pickups.")]
    public PowerupType type;
    [Tooltip("Duration of effect the powerup provides in seconds.")]
    public float duration = 5f; // Duration of the powerup effect in seconds
    [Tooltip("Amount of effect the Powerup provides. Ignore for health and invincibility.")]
    public float amount = 1f; // Amount of effect (e.g., speed multiplier)
    // 
    [Space(10)]
    [Header("Optional sound to play when the powerup is collected. Leave empty for no sound.")]
    public AudioClip pickupSound;
    public float soundVolume = 1f;
}
