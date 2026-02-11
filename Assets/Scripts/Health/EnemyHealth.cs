using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class EnemyHealth : Health
{
    [Header("Does this drop something when dead?")][Tooltip("If yes, needs ItemDrop component")]
    public bool dropThings = false;

    [Header("Does this have a health bar?")][Tooltip("If yes, drag EnemyMiniCanvas PREFAB onto this object")]
    public bool healthBar = false;
    Image healthBarFill;

    [Header("Time before enemy is destroyed.")][Tooltip("Used for death animations")]
    public float timeToDie = 0F;

    [Header("When enemy dies, should it count towards Enemy Death Goal?")]
    public bool deathCountsTowardGoal;

    [Header("Automatically set this object to 'Enemy' Layer?")]
    public bool autosetLayerToEnemy = true;

    override public void TakeDamage(int amount)
    {
        if (!isImmune && !dead && !immortal)
        {
            currentHealth -= amount;
            if (healthBar) healthBarFill.fillAmount = (float)currentHealth / (float)maxHealth;

            if (currentHealth <= 0)
            {
                dead = true;
                if (GetComponent<Animator>()) GetComponent<Animator>().SetBool("Death", true);
                if (GetComponent<ShootAllDirection>()) GetComponent<ShootAllDirection>().disablePool();

                if (deathCountsTowardGoal)
                {
                    EnemyDeathGoal.enemyDeathGoal?.AddEnemyDeath();
                }

                if (GetComponent<Rigidbody2D>())
                {
                    Rigidbody2D rb = GetComponent<Rigidbody2D>();
                    rb.bodyType = RigidbodyType2D.Static;
                }
                
                foreach (Collider2D a in GetComponents<Collider2D>()) a.enabled = false;

                Invoke("WhenDead", timeToDie);
            }
            else StartCoroutine(ImmunityReset());

            // trigger audio event
            if (sound)
            {
                AudioManager.audioManager?.playAudio(sound, soundVolume);
            }
        }
    }

    public override void WhenDead()
    {
        if (!immortal)
        {
            //dead = true;

            //if (GetComponent<ShootAllDirection>()) GetComponent<ShootAllDirection>().disablePool();

            //if (GetComponent<Animator>()) GetComponent<Animator>().SetBool("Death", true);

            if (dropThings)
            {
                ItemDrop id = GetComponent<ItemDrop>();
                if (!id) Debug.LogError("ItemDrop component not found on " + gameObject.name + "! No items will be dropped.", gameObject);
                else id.dropTheLoot();
            }

            Destroy(gameObject);
        }
    }

    public override void fillHealth(int fill)
    {
        if (!immortal)
        {
            if (currentHealth + fill > maxHealth) currentHealth = maxHealth;
            else if (fill > 0) currentHealth += fill;
            else Debug.LogError("Invalid heal amount.");

            if (healthBar) healthBarFill.fillAmount = (float)currentHealth / (float)maxHealth;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        if (autosetLayerToEnemy)
        {
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            // Debug.Log("Set Layer to Enemy");
        }
        
        if (healthBar)
        {
            if(GetComponentInChildren<Canvas>())
            {
                Canvas c = GetComponentInChildren<Canvas>();
                foreach(Image i in c.GetComponentsInChildren<Image>())
                {
                    if (i.name == "HealthBarFill")
                    {
                        healthBarFill = i;
                        return;
                    }
                }
                Debug.LogError("Error setting " + gameObject.name + "'s health bar. Disabling health bar.", gameObject);
                healthBar = false;
            }
            else
            {
                Debug.LogError("Health bar not found on " + gameObject.name + "");
                healthBar = false;
            }
        }
    }
}