using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health
{
    [Header("Audio Settings")]
    public AudioClip healSound;
    public float healVolume = 1f;

    // 1. ADD THIS: A static variable to hold health between scenes
    private static int savedHealth = -1;

    [Header("Does the player have UI to see their health?")]
    [Tooltip("PREFABS: HealthBar, HealthIcons, HealthText (They go in the Canvas!)")]
    public HealthUI healthUI;

    [Header("Does the player start with full health?")]
    public bool startsFullHealth = true;
    int startingHealth = -1;

    public static PlayerHealth instance;

    override protected void Awake()
    {
        // 2. ADD THIS: Prevents the player object from being deleted on scene change
        DontDestroyOnLoad(gameObject);

        if (maxHealth <= 0)
        {
            Debug.LogError(gameObject.name + " needs to start with more than 0 health! Setting health to 1...", gameObject);
            maxHealth = 1;
        }

        dead = false;

        // 3. ALTERED THIS: Check if we have a saved health value first
        if (savedHealth != -1)
        {
            currentHealth = savedHealth;
        }
        else if (startsFullHealth)
        {
            currentHealth = maxHealth;
        }

        startingHealth = currentHealth;

        // ADD THIS: Look for a HealthUI in the new scene if ours is missing
        if (healthUI == null)
        {
            healthUI = GetComponentInChildren<HealthUI>();
            //healthUI = FindObjectOfType<HealthUI>();
        }

        // Refresh the UI with our current health
        if (healthUI != null)
        {
            healthUI.setHealth(maxHealth, currentHealth);
        }
    }

    void Start()
    {
        if(currentHealth <= 0)
        {
            Debug.Log("O health!");
            currentHealth = maxHealth;
            healthUI.updateHealth(currentHealth);
        }
    }

    // 4. ADD THIS: Save the health whenever it changes
    void Update()
    {
        // 1. Check if we are in the Main Menu
        // Replace "MainMenu" with the EXACT name of your menu scene
        if (SceneManager.GetActiveScene().name == "Fallen Crown - Main Menu" || SceneManager.GetActiveScene().name == "Fallen Crown - Win")
        {
            // Reset the static instance so it doesn't point to a dead object
            instance = null;
            savedHealth = -1;

            Destroy(gameObject);
            return; // Stop running the rest of the code
        }

        // 2. Your existing update logic
        savedHealth = currentHealth;
    }

    override public void TakeDamage(int amount)
    {
        print("taking damage: " + amount);
        if (!isImmune && !dead && !immortal)
        {
            currentHealth -= amount;
            savedHealth = currentHealth; // Save on damage
            if (healthUI) healthUI.updateHealth(currentHealth);

            if (currentHealth <= 0) WhenDead();
            else StartCoroutine(ImmunityReset());

            // trigger audio event
            if (sound != null)
                AudioManager.audioManager?.playAudio(sound, soundVolume);
        }
    }

    override public void WhenDead()
    {
        if (!immortal)
        {
            dead = true;

            foreach (ShootAllDirection a in GetComponents<ShootAllDirection>()) a.disablePool();

            if (GetComponent<Animator>()) GetComponent<Animator>().SetBool("Death", true);

            Respawn r = GetComponent<Respawn>();
            if (r == null)
            {
                Debug.LogWarning("Respawn component not found. Reloading current scene.");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else r.useRespawn();
        }
    }

    public override void fillHealth(int fill)
    {
        if (!immortal)
        {
            // 1. Check if the player actually needs healing
            if (currentHealth < maxHealth)
            {
                // 2. Perform the heal
                if (currentHealth + fill > maxHealth)
                    currentHealth = maxHealth;
                else if (fill > 0)
                    currentHealth += fill;
                else
                    Debug.LogError("Invalid heal amount.");

                // 3. Play the sound ONLY because we actually healed
                if (healSound != null)
                {
                    AudioManager.audioManager?.playAudio(healSound, healVolume);
                }

                // 4. Update the UI and Save
                if (healthUI) healthUI.updateHealth(currentHealth);
                // If you removed Update(), make sure you save here:
                // savedHealth = currentHealth; 
            }
        }
    }

    override public void revive()
    {
        currentHealth = startingHealth;
        if (healthUI) healthUI.setHealth(maxHealth, startingHealth);
        if (GetComponent<Animator>()) GetComponent<Animator>().SetBool("Death", false);
        dead = false;
    }

    public bool CanHeal()
    {
        return currentHealth < maxHealth;
    }
}
