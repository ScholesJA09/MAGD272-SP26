using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlatformHealth : Health
{
    [Header("Does this drop something when dead?")][Tooltip("If yes, needs ItemDrop component")]
    public bool dropThings = false;

    [Header("Does this have a health bar?")][Tooltip("If yes, drag EnemyMiniCanvas PREFAB onto this object")]
    public bool healthBar = false;
    Image healthBarFill;

    [Header("Time before enemy is destroyed.")][Tooltip("Used for death animations")]
    public float timeToDie = 0F;

    public delegate void deathEvent();

    public static event deathEvent EnemyDeath;

    public float PlatformWaitTime;
    public float PlatformAnimationDestoryTime;

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

                if (EnemyDeath != null)
                {
                    EnemyDeath();
                }

                if (GetComponent<Rigidbody2D>())
                {
                    Rigidbody2D rb = GetComponent<Rigidbody2D>();
                    rb.bodyType = RigidbodyType2D.Static;
                }
                
                //foreach (Collider2D a in GetComponents<Collider2D>()) a.enabled = false;

                StartCoroutine(WhenDead());
                //Invoke("WhenDead", timeToDie);
            }
            else StartCoroutine(ImmunityReset());

            // trigger audio event
            if (sound != null)
                AudioManager.audioManager?.playAudio(sound, soundVolume);
        }
    }

    new public IEnumerator WhenDead()
    {
        if (!immortal)
        {
            //dead = true;

            //if (GetComponent<ShootAllDirection>()) GetComponent<ShootAllDirection>().disablePool();

            //if (GetComponent<Animator>()) GetComponent<Animator>().SetBool("Death", true);

            yield return new WaitForSeconds(PlatformWaitTime);

            foreach (Collider2D a in GetComponents<Collider2D>()) a.enabled = false;

            if (GetComponent<Animator>()) GetComponent<Animator>().SetBool("Death", true);


            if (dropThings)
            {
                ItemDrop id = GetComponent<ItemDrop>();
                if (id == null) Debug.LogError("ItemDrop component not found on " + gameObject.name + "! No items will be dropped.", gameObject);
                else id.dropTheLoot();
            }

            /*if(EnemyDeath != null)
            {
                EnemyDeath();
            }*/

            yield return new WaitForSeconds(PlatformAnimationDestoryTime);


            Destroy(gameObject);
        }
        yield return true;
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