using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Can be placed on Enemy or Third-party Object
// Triggers an outcome if Non-player character arrives at a destination

public class EnemyDestinationGoal : MonoBehaviour, ICompletible
{

    //public static DestinationGoal destinationGoal;

    public string enemyTag;

    [Header("Place this script on the object at the Destination")]
    public bool enemyArrived = false;

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

    //private void Awake()
    //{
    //    if (destinationGoal == null) destinationGoal = this;
    //    // don't destory gameObject, please;
    //    else Destroy(this);
    //}

    // Start is called before the first frame update
    void Start()
    {
        if(enableAnObject && TargetObject != null)TargetObject.SetActive(!DisableAtStart);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(enemyTag))
        {
            enemyArrived = true;
            Debug.Log("Arrived");

            Debug.Log("Destination GOAL MET ---------");
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(enemyTag))
        {
            enemyArrived = true;
            Debug.Log("Arrived");

            Debug.Log("Destination GOAL MET ---------");
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


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemyArrived = false;
            Debug.Log("departed");

            completed = false;
        }
    }

    public bool Completed()
    {
        return completed;
    }
}
