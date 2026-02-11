using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Can be placed on Player or Third-party Object

public class EnemyDeathGoal : MonoBehaviour, ICompletible
{
    [Header("Death Count Goal")]
    public int DeathGoal;
    [Header("Current Death Count")]
    public int EnemyDeathCount = 0;

    [Header("Displaying count to UI? Connect to 'EnemyKillCountUI.cs")]
    public EnemyKillCountUI enemyKillCountUI;

    [Header("-------GOAL OUTCOMES-------", order =0 )]

    [Header("A. Play a sound when goal met?", order =1), Space(30)]
    public bool playSound;
    public AudioClip sound;
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


    //singleton
    public static EnemyDeathGoal enemyDeathGoal;

    private void Awake()
    {
        if (enemyDeathGoal == null) enemyDeathGoal = this;
        // don't destroy gameObject, please;
        else Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        if(enableAnObject && TargetObject != null)TargetObject.SetActive(!DisableAtStart);
    }

    public void AddEnemyDeath()
    {
        EnemyDeathCount++;
        if (enemyKillCountUI != null)
        {
            enemyKillCountUI.updateKillCount(EnemyDeathCount);
        }

        if (EnemyDeathCount >= DeathGoal)
        {
            Debug.Log("DEATH GOAL MET ---------");
            // execute goal outcomes

            // play a sound
            if (playSound)
            {
                AudioManager.audioManager?.playAudio(sound, volume);
            }

            // change scene
            if (changeScene)
            {
                SceneController.sceneController?.delayedSceneLoad(sceneName, sceneChangeDelay);
            }

            // enable object
            if (enableAnObject && TargetObject != null) TargetObject.SetActive(true);

            //

            completed = true;
        }
    }

    public bool Completed()
    {
        return completed;
    }
}
