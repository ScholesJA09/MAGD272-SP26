using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthNew : Health
{
    [Header("Does the player have UI to see their health?")]
    public HealthUI healthUI;

    [Header("Does the player start with full health?")]
    public bool startsFullHealth = true;

    override protected void Awake()
    {
        // 1. Ensure the GameManager exists
        if (GameManager.Instance == null)
        {
            Debug.LogError("No GameManager found in scene! Health won't persist.");
            return;
        }

        // 2. Setup Max Health
        if (maxHealth <= 0) maxHealth = 10; // Default for HK-style
        GameManager.Instance.maxHealth = maxHealth;

        // 3. Sync Current Health
        // If it's the start of the game (0), set to max. Otherwise, keep what's in the Manager.
        if (GameManager.Instance.currentHealth <= 0 && startsFullHealth)
        {
            GameManager.Instance.currentHealth = maxHealth;
        }

        // Local variable sync (so the base 'Health' script stays happy)
        currentHealth = GameManager.Instance.currentHealth;
        dead = false;

        // 4. Update UI
        if (healthUI)
        {
            healthUI.setHealth(maxHealth, currentHealth);
        }
    }

    override public void TakeDamage(int amount)
    {
        if (!isImmune && !dead && !immortal)
        {
            // Update the Manager's value
            GameManager.Instance.currentHealth -= amount;
            currentHealth = GameManager.Instance.currentHealth; // Sync local copy

            if (healthUI) healthUI.updateHealth(currentHealth);

            if (currentHealth <= 0) WhenDead();
            else StartCoroutine(ImmunityReset());

            if (sound != null)
                AudioManager.audioManager?.playAudio(sound, soundVolume);
        }
    }

    public override void fillHealth(int fill)
    {
        if (!immortal)
        {
            // Update the Manager's value
            int newHealth = GameManager.Instance.currentHealth + fill;
            GameManager.Instance.currentHealth = Mathf.Clamp(newHealth, 0, maxHealth);
            currentHealth = GameManager.Instance.currentHealth; // Sync local

            if (healthUI) healthUI.updateHealth(currentHealth);
        }
    }

    override public void revive()
    {
        // When reviving, we usually reset to full or a checkpoint value
        GameManager.Instance.currentHealth = maxHealth;
        currentHealth = maxHealth;

        if (healthUI) healthUI.setHealth(maxHealth, currentHealth);
        if (GetComponent<Animator>()) GetComponent<Animator>().SetBool("Death", false);
        dead = false;
    }

    // Keep your WhenDead() as is, or update to handle scene reloading through the transition
}