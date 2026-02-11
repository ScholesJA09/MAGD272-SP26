using System.Collections;
using UnityEngine;

public class PowerupHandler : MonoBehaviour
{
    private Coroutine invincibilityCoroutine, speedCoroutine, projectileSpeedCoroutine = null;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
            Powerup powerup = other.GetComponent<Powerup>();
            if (powerup != null)
            {
                ApplyPowerup(powerup.type, powerup.duration, powerup.amount, powerup.pickupSound, powerup.soundVolume);
                Destroy(other.gameObject);
            }
    }

    void ApplyPowerup(PowerupType type, float duration, float amount, AudioClip pickupSound = null,
        float soundVolume = 1f)
    {
        switch (type)
        {
            // case PowerupType.Health:
            //     GetComponent<PlayerHealth>().fillHealth((int)amount); // Heal 1 health point
            //     break;
            case PowerupType.Speed:
                if (speedCoroutine != null)
                {
                    StopCoroutine(speedCoroutine);
                }
                speedCoroutine = StartCoroutine(ApplySpeedBoost(duration, amount));
                break;
            case PowerupType.Invincibility:
                if (invincibilityCoroutine != null)
                {
                    StopCoroutine(invincibilityCoroutine);
                }
                invincibilityCoroutine = StartCoroutine(ApplyInvincibility(duration));
                break;
            case PowerupType.ProjectileSpeed:
                if (projectileSpeedCoroutine != null)
                {
                    StopCoroutine(projectileSpeedCoroutine);
                }
                projectileSpeedCoroutine = StartCoroutine(ApplyProjectileSpeedBoost(duration, amount));
                break;
        }

        if (pickupSound != null)
        {
            AudioManager.audioManager?.playAudio(pickupSound, soundVolume);
        }
    }

    IEnumerator ApplySpeedBoost(float duration, float amount)
    {
        IMove motor = GetComponent<IMove>();
        
        if (motor != null)
        {
            var oldSpeed = motor.speed;
            motor.speed = amount;
            yield return new WaitForSeconds(duration);
            motor.speed = oldSpeed; 
        }
    }

    IEnumerator ApplyInvincibility(float duration)
    {
        var health = GetComponent<PlayerHealth>();
        if (health)
        {
            health.immortal = true;
            yield return new WaitForSeconds(duration);
            health.immortal = false;
        }
        else
        {
            Debug.LogWarning("Player Health component not found on player for invincibility.");
        }
    }

    IEnumerator ApplyProjectileSpeedBoost(float duration, float amount)
    {
        
        var shooter = GetComponent<ShootAllDirection>();
        var oldSpeed = shooter.projectileSpeed;
        if (shooter)
        {
            shooter.projectileSpeed = amount;
            print("Applying projectile speed boost: " + shooter.projectileSpeed);
            yield return new WaitForSeconds(duration);
            shooter.projectileSpeed = oldSpeed;
        }
        else
        {
            Debug.LogWarning("Shoot component not found on player for projectile speed boost.");
        }
    }
}