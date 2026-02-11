using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TimedSpawner : MonoBehaviour
{
    //
    public bool BeginTimerAtStart = false;
    public GameObject ObjectToSpawn;
    public float SpawnInterval;
    public float currentTime;
    //
    public bool decreaseInterval = false;
    public float decreasePercentage = 0.1f;
    [Header("Decrease interval time by a percentage every x seconds")]
    public float decreaseIntervalTime = 1f;
    public float decreaseIntervalTimer;

    //
    public enum SpawnAxis { Horizontal, Vertical };
    [Header("Spawn Along an axis?")]
    public bool SpawnAlongAxis = false;
    public SpawnAxis axis;
    [Header("If spawning on axis, use two empty game objects to define the spawn axis")]
    public Transform axisBoundary1, axisBoundary2;
    private float lesserVal, greaterVal;

    //
    [Header("Play a sound when spawning? - Be sure to check the volume")]
    public AudioClip soundfile;
    AudioSource audioSource;
     

    private void Start()
    {
        if (BeginTimerAtStart) currentTime = SpawnInterval;
        audioSource = GetComponent<AudioSource>();
        if (GetComponent<AudioSource>() == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        if (soundfile != null) audioSource.clip = soundfile;
        decreaseIntervalTimer = decreaseIntervalTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(decreaseInterval)
        {
            if(decreaseIntervalTimer > 0)
            {
                decreaseIntervalTimer -= Time.deltaTime;
            } else
            {
                decreaseIntervalTimer = decreaseIntervalTime;
                SpawnInterval -= SpawnInterval * decreasePercentage;
            }
        }
        if(currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        } else
        {
            Spawn();
            currentTime = SpawnInterval; 
        }
    }

    void Spawn()
    {
        if (ObjectToSpawn == null) return;
        
        if (SpawnAlongAxis)
        {
            Instantiate(ObjectToSpawn, RandomizeSpawnPoint(), Quaternion.identity, null);
        } else
        {
            Instantiate(ObjectToSpawn, gameObject.transform.position, Quaternion.identity, null);
        }
        Debug.Log("Spawning", gameObject);
    }

    Vector3 RandomizeSpawnPoint()
    {
        if(axis == SpawnAxis.Horizontal)
        {
            lesserVal = axisBoundary1.position.x < axisBoundary2.position.x ? axisBoundary1.position.x : axisBoundary2.position.y;
            greaterVal = axisBoundary1.position.x > axisBoundary2.position.x ? axisBoundary1.position.x : axisBoundary2.position.y;
            return new Vector3(Random.Range(lesserVal, greaterVal), axisBoundary1.position.y, 0);
        } else
        {
            lesserVal = axisBoundary1.position.y < axisBoundary2.position.y ? axisBoundary1.position.y : axisBoundary2.position.y;
            greaterVal = axisBoundary1.position.y > axisBoundary2.position.y ? axisBoundary1.position.y : axisBoundary2.position.y;
            return new Vector3(axisBoundary1.position.x, Random.Range(lesserVal, greaterVal), 0);
        }
    }
}
