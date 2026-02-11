using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
// keep track of multiple Goals and outcomes
public class ComplexGoalsManager : MonoBehaviour
{

    [Header("Drop Individual Goal Object into Field")]
    public GameObject GoalObject;

    [Header("-------Individual Goals (Do not edit) -------", order = 0)]
    public List<MonoBehaviour> Goals = new List<MonoBehaviour>();

    [Header("Goals Completed")]
    public int goalsCompleted = 0;
    private bool complexGoalsComplete = false;


    [Header("------- COMPLETION OUTCOMES-------", order = 0)]

    [Header("A. Play a sound when goal met?", order = 1), Space(30)]
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
    public delegate void GoalsCompleted();
    // event called when Goal is completed. Other scripts can 
    public static event GoalsCompleted EnemyDeathGoalCompleted;

    public bool completed = false;

    // Start is called before the first frame update
    void Start()
    {
        if (enableAnObject && TargetObject != null) TargetObject.SetActive(!DisableAtStart);
    }

    private void Update()
    {
        goalsCompleted = countGoalsCompleted();
    }

    int countGoalsCompleted()
    {
        int runningGoalsCompleted = 0;
        foreach (MonoBehaviour script in Goals)
        {
            if (script.GetComponent<ICompletible>().Completed())
                runningGoalsCompleted++;
            if (runningGoalsCompleted >= Goals.Count && !complexGoalsComplete)
            {
                complexGoalsComplete = true;
                Debug.Log("COMPLEX GOALS MET ---------");
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
            }
        }
        return runningGoalsCompleted;
    }

    private void OnValidate()
    {
        if (GoalObject != null)
        {
            ICompletible[] completes = GoalObject.GetComponents<ICompletible>();
            Debug.Log(completes.Length);
            foreach (ICompletible goal in completes)
            {
                if (!Goals.Contains(goal as MonoBehaviour))
                {
                    Goals.Add(goal as MonoBehaviour);
                    Debug.Log("add goal...");
                }
                else
                {
                    Debug.Log("You already have the Goal added!");
                }
            }

            GoalObject = null;
        }

        //Goals = Goals;
    }
}
