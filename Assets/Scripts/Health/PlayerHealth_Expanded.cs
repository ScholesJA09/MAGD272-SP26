using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth_Expanded : PlayerHealth
{
    //[Header("Does the player have UI to see their health?")][Tooltip("PREFABS: HealthBar, HealthIcons, HealthText (They go in the Canvas!)")]
    //public HealthUI healthUI;

    //[Header("Does the player start with full health?")]
    //public bool startsFullHealth = true;
    int startingHealth = -1;

    [Header("Can picking up health expand your Max Health Value?")]
    public bool expandMaxHealth = false;


    public enum healthExpander
    {
        oneUnit,
        useHealthObjectValue
    }

    [Header("If exanding Max Health, by how much?")]
    public healthExpander healthExpand = healthExpander.oneUnit;

    //override protected void Awake()
    //{
    //    if (maxHealth <= 0)
    //    {
    //        Debug.LogError(gameObject.name + " needs to start with more than 0 health! Setting health to 1...", gameObject);
    //        maxHealth = 1;
    //    }

    //    dead = false;
    //    if (startsFullHealth) currentHealth = startingHealth = maxHealth;
    //    else startingHealth = currentHealth;

    //    if (healthUI)
    //    {
    //       if (startsFullHealth) healthUI.setHealth(maxHealth);
    //       else healthUI.setHealth(maxHealth, currentHealth);
    //    }
    //    else Debug.LogWarning("No UI set for player health. Player health will only be visible in the Inspector.");
    //}

    //override public void TakeDamage(int amount)
    //{
    //    if (!isImmune && !dead && !immortal)
    //    {
    //        currentHealth -= amount;
    //        if (healthUI) healthUI.updateHealth(currentHealth);

    //        if (currentHealth <= 0) WhenDead();
    //        else StartCoroutine(ImmunityReset());

    //        // trigger audio event
    //        if (sound != null)
    //            AudioManager.audioManager?.playAudio(sound, soundVolume);
    //    }
    //}

    //override public void WhenDead()
    //{
    //    if (!immortal)
    //    {
    //        dead = true;

    //        foreach (ShootAllDirection a in GetComponents<ShootAllDirection>()) a.disablePool();

    //        if (GetComponent<Animator>()) GetComponent<Animator>().SetBool("Death", true);

    //        Respawn r = GetComponent<Respawn>();
    //        if (r == null)
    //        {
    //            Debug.LogWarning("Respawn component not found. Reloading current scene.");
    //            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //        }
    //        else r.useRespawn();
    //    }
    //}

    public override void fillHealth(int fill)
    {
        if (!immortal)
        {
            if (currentHealth + fill > maxHealth)
            {
                if (expandMaxHealth)
                {
                    switch (healthExpand)
                    {
                        case healthExpander.oneUnit:
                            maxHealth++;
                            currentHealth = maxHealth;
                            if (healthUI) healthUI.setHealth(maxHealth, maxHealth);
                            break;
                        case healthExpander.useHealthObjectValue:
                            maxHealth = maxHealth + fill;
                            currentHealth = maxHealth;
                            if (healthUI) healthUI.setHealth(maxHealth, maxHealth);
                            break;
                        default:
                            break;
                    }
                } else
                {
                    currentHealth = maxHealth;
                }
            }
                
            else if (fill > 0) currentHealth += fill;
            else Debug.LogError("Invalid heal amount.");

            if (healthUI) healthUI.updateHealth(currentHealth);
        }
    }

    override public void revive()
    {
        currentHealth = startingHealth;
        if (healthUI) healthUI.setHealth(maxHealth, startingHealth);
        if (GetComponent<Animator>()) GetComponent<Animator>().SetBool("Death", false);
        dead = false;
    }
}
