using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSpawner : MonoBehaviour
{
    public bool spawnOnEnable = true;
    public GameObject prefabToSpawn;

    private void OnEnable()
    {
        Spawn();
    }

    void Spawn()
    {
        if (prefabToSpawn != null)
        {
            GameObject spawnedObject = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
            spawnedObject.transform.SetParent(transform);
        }
        else
        {
            Debug.LogWarning("Prefab to spawn is not assigned.");
        }
    }
    
}
