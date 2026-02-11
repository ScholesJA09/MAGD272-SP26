using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health : MonoBehaviour, IDamagable
{
    [Header("Max health of object")]
    public int maxHealth = 10;
    [SerializeField]
    protected int currentHealth = 0;

    [Header("Time before object can be damaged again")]
    public float immunityTime = 0.2F;
    [Header("Should object flash red when hurt?")]
    public bool immunityVisible = true;
    protected bool isImmune = false;

    [Header("Does this object not lose health?")][Tooltip("Some attacks require health to tell if they are enemies or not, but you can turn off taking damage using this")]
    public bool immortal = false;

    public bool dead { get; protected set; }

    [Header("Play a sound when taking damage?")]
    public AudioClip sound;
    public float soundVolume = 1f;

    virtual protected void Awake()
    {
        if(LayerMask.NameToLayer("Enemy") == -1)
        {
            Debug.LogWarning("Layer 'Enemy' does not exist. Please create it in the Tags and Layers Manager.");
        }
        if(LayerMask.NameToLayer("Player") == -1)
        {
            Debug.LogWarning("Layer 'Player' does not exist. Please create it in the Tags and Layers Manager.");
        }
        
        if (gameObject.layer != LayerMask.NameToLayer("Enemy") && gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            Debug.LogWarning(gameObject.name + " is not on the 'Enemy' or 'Player' layer. Please set it to one of those layers.");
        }
        
        if (maxHealth <= 0)
        {
            Debug.LogError(gameObject.name + " needs to start with more than 0 health! Setting health to 1...", gameObject);
            maxHealth = 1;
        }

        dead = false;
        currentHealth = maxHealth;
    }

    virtual public void TakeDamage(int amount)
    {
        if (!isImmune && !dead && !immortal)
        {
            currentHealth -= amount;
            if (currentHealth <= 0) WhenDead();
            else StartCoroutine(ImmunityReset());

            // trigger audio event
            if (sound != null)
                AudioManager.audioManager?.playAudio(sound, soundVolume);
        }
    }

    virtual public void WhenDead()
    {
        if(!immortal) Debug.LogWarning(gameObject.name + " is dead.", gameObject);
    }

    protected IEnumerator ImmunityReset()
    {
        isImmune = true;

        Color32 spriteColor = GetComponent<SpriteRenderer>().color;
        if (immunityVisible) GetComponent<SpriteRenderer>().color = new Color(140, 0, 0); 

        yield return new WaitForSeconds(immunityTime);

        if (immunityVisible) GetComponent<SpriteRenderer>().color = spriteColor;
        isImmune = false;
    }

    public virtual void fillHealth(int fill)
    {
        if (!immortal)
        {
            if (currentHealth + fill > maxHealth) currentHealth = maxHealth;
            else if (fill > 0) currentHealth += fill;
            else Debug.LogError("Invalid heal amount.");
        }
    }

    virtual public void revive()
    {
        currentHealth = maxHealth;
        dead = false;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
