using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour, IInteractable
{
    public static Timer timerFail;

    [Header("Activate this Timer at Start?")]
    public bool activateAtStart = false;

    [Header("Timer Start Value")]
    public float startTime;
    public bool timerStarted = false;
    public float currentTime;

    [Header("Display Timer on UI Text ")]
    public Text timerText;
    //[Header("Display Decimals?")]
    //public int numOfDecimals = 0;

    [Header("-------TIMER OUTCOMES-------", order = 0)]

    [Header("A. Play a sound when timer completed?", order = 1), Space(30)]
    public bool playSound;
    public AudioClip sound;
    [Range(0, 1f)]
    public float volume = 1f;

    [Header("B. Change Scenes when timer completed?"), Space(30)]
    public bool changeScene;
    [Range(0, 5f)]
    public float sceneChangeDelay;
    [Header("Type in name of scene")]
    public string sceneName;

    [Header("C. Enable an Object when timer completed?"), Space(30)]
    public bool enableAnObject;
    public GameObject TargetObject;
    public bool DisableAtStart;

    [Header("D. Deal damage when timer completed?"), Space(30)]
    public bool dealDamage;
    [Tooltip("The target that you want to deal damage to.")]
    public Health target;
    public int damageAmount = 1;


    // event delegate
    public delegate void TimerFailEvent();
    // event called when Goal is completed. Other scripts can 
    public static event TimerFailEvent onTimerFail;

    [Header("Is the timer complete?"), Space(30)]
    public bool timerComplete = false;

    private void Awake()
    {
        if (timerFail == null) timerFail = this;
        // don't destroy gameObject, please;
        else Destroy(this);
    }

    void OnEnable()
    {
        if (activateAtStart)
        {
            timerStarted = true;
        }
    }

    void OnDisable()
    {
        timerStarted = false;
    }

    void Start()
    {
        if (enableAnObject && TargetObject != null) TargetObject.SetActive(!DisableAtStart);
        currentTime = startTime;
        timerStarted = activateAtStart;
    }

    private void Update()
    {
        if (timerStarted && !timerComplete)
        {
            currentTime -= Time.deltaTime;

            displayTimerTime(currentTime);

            if(currentTime < 0)
            {
                timerComplete = true;
    

                Debug.Log("Timer ENDED ---------");
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

                // deal damage
                if (dealDamage && target != null)
                {
                    target.TakeDamage(damageAmount);
                }
            }
        }
    }

    public void toggleTimer()
    {
        timerStarted = !timerStarted;
    }

    private void displayTimerTime(float currentTime)
    {
        currentTime = (currentTime < 0) ? 0 : currentTime;
        if (!timerText)
        {
            return;
        } else {
            timerText.text = currentTime.ToString("F0");
        }
    }

    public bool Failed()
    {
        return timerComplete;
    }

    public bool isActive()
    {
        return timerStarted;
        //throw new System.NotImplementedException();
    }

    public bool isInteractable()
    {
        return true;
        throw new System.NotImplementedException();
    }

    public void interact()
    {
        toggleTimer();
        throw new System.NotImplementedException();
    }
}
