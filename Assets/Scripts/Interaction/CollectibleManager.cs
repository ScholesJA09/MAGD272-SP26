using UnityEngine;
using UnityEngine.UI;
using System;

[System.Serializable]
public enum Collectible_Type
{
    Coin,
    Ammo,
    Key,
    Health,
}

public class CollectibleManager : MonoBehaviour, ICollectible, ICompletible
{
    //This is used to keep track of what has been collected.

    [Header("Amount of coins collected")]
    public int coinsCollected;
    public bool showCoins;
    public bool coinsCollectedGoal;
    public int coinGoalAmount;
    [Header("Amount of ammo collected")]
    public int ammoCollected;
    public bool showAmmo;
    [Header("Drag in the ui text you want to display.")]
    public Text coinCount;
    public Text ammoCount;

    public bool keyCollected;
    public GameObject keyImage;
    public bool showKey;

    private AudioClip sound;
    [Header("Do you want to play an sound when collecting? Add an audio clip")]
    public AudioClip coinSound;
    public AudioClip ammoSound;
    public AudioClip healthSound;
    public AudioClip keySound;
    public float soundVolume = 1f;
    public static event Action<AudioClip, float> OnAudioEvent = delegate { };


    [Header("-------GOAL OUTCOMES-------", order = 0)]

    [Header("A. Play a sound when goal met?", order = 1), Space(30)]
    public bool playSound;
    public AudioClip goalSound;
    [Range(0, 1f)]
    public float volume = 1f;

    [Header("B. Change Scenes when goal met?"), Space(30)]
    public bool changeScene;
    [Range(0, 5f)]
    public float sceneChangeDelay;
    [Header("Type in name of scene")]
    public string sceneName;

    [Header("C. Enable an Object when goal met?"), Space(30)]
    public bool enableAnObject;
    public GameObject TargetObject;
    public bool DisableAtStart;

    // event delegate
    public delegate void GoalCompleted();
    // event called when Goal is completed. Other scripts can 
    public static event GoalCompleted goalCompleted;

    public bool completed = false;

    private void Start()
    {
        if (DisableAtStart && TargetObject != null)
        {
            TargetObject.SetActive(false);
        }

        // Initialize coins and ammo counts
        coinsCollected = 0;
        ammoCollected = 0;
        keyCollected = false;

        // Check if coin goal is set
        if (coinsCollectedGoal && coinGoalAmount <= 0)
        {
            Debug.LogError("Coin goal amount must be greater than 0!");
            coinsCollectedGoal = false;
        }
    }
    
    private void Update()
    {
        WhatTextAmI();
        KeyUi();
    }

    /// <summary>
    /// This method updates the ammount of collectibles the player has
    /// </summary>
    /// <param the enum on the collected object ="type"></param>
    /// <param the amount that has been set on the collected object ="amount"></param>
    public void UpdateValue(Collectible_Type type, int amount)
    {
        if(type == Collectible_Type.Coin)
        {
            coinsCollected += amount;
            sound = coinSound;
        }
        else if(type == Collectible_Type.Ammo)
        {
            ammoCollected += amount;
            sound = ammoSound;
        }
        else if(type == Collectible_Type.Health)
        {
            //Add to health system
            GetComponent<Health>().fillHealth(amount);
            sound = healthSound;
        }
        else if(type == Collectible_Type.Key)
        {
            keyCollected = true;
            sound = keySound;
        }

        if (amount > 0)
        {
            // trigger audio event
            if (sound != null)
                AudioManager.audioManager?.playAudio(sound, soundVolume);
        }

        if(coinsCollected >= coinGoalAmount)
        {
            Debug.Log("COIN GOAL MET ---------");
            // execute goal outcomes

            // play a sound
            if (playSound)
            {
                AudioManager.audioManager?.playAudio(goalSound, volume);
            }

            // change scene
            if (changeScene)
            {
                SceneController.sceneController?.delayedSceneLoad(sceneName, sceneChangeDelay);
            }

            // enable object
            if (enableAnObject && TargetObject is not null) TargetObject.SetActive(true);

            //

            completed = true;
        }
    }

    /// <summary>
    /// Checking if player has enough ammo to be able to shoot
    /// </summary>
    /// <returns></returns>
    public bool CheckForAmmo(int amount)
    {
        if (ammoCollected >= amount)
        {
            // Able to shoot
            return true;
        }
        else
        {
            // Not able to shoot
            Debug.Log(gameObject.name + " is out of ammo!", gameObject);
            return false;
        }
    }

    
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collectItem(collision.gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collectItem(collision.gameObject);
    }

    private void collectItem(GameObject collision)
    {
        if (collision.CompareTag("Collectible") && collision.GetComponent<Collectible>())
        {
            Collectible_Type type = collision.gameObject.GetComponent<Collectible>().type;
            int amount = collision.gameObject.GetComponent<Collectible>().amount;

            UpdateValue(type, amount);

            Destroy(collision.gameObject);
        }
        else if (!collision.CompareTag("Collectible") && collision.GetComponent<Collectible>())
        {
            Debug.LogError("Object needs to be tagged as 'Collectible' ");
        }
        else if (collision.CompareTag("Collectible") && !collision.GetComponent<Collectible>())
        {
            Debug.LogError("Object is missing the 'Collectible.cs' script");
        }
    }

    public void WhatTextAmI()
    {
        if (showCoins && coinCount != null)
        {
            coinCount.text = coinsCollected.ToString();
        }
        else if(showCoins && coinCount == null)
        {
            Debug.LogError("Missing the 'Coin Count' UI text");
        }

        if(showAmmo && ammoCount != null)
        {
            ammoCount.text = ammoCollected.ToString();
        }
        else if(showAmmo && ammoCount == null)
        {
            Debug.LogError("Missing the 'Ammo Count' UI text");
        }
    }

    public void UseKey()
    {
        if (keyCollected)
        {
            keyCollected = false;
        }
    }

    public void KeyUi()
    {
        if (showKey)
        {
            if (keyCollected)
                keyImage.SetActive(true);
            else
                keyImage.SetActive(false);
        }
    }

    public bool Completed()
    {
        return completed;
    }
}

