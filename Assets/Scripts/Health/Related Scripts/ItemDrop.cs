using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [Header("Item types to drop on death; these are all PREFABS")]
    public List<GameObject> droppables;

    [Header("Will an item always drop or have a chance to drop?")]
    public bool guaranteed = false;

    [Header("If NOT guaranteed, percent chance for drop (1-99)")]
    public int percentChanceForDrop = 20;

    [Header("Do you want to play a sound when dropping loot? Add an audio clip")]
    public AudioClip sound;

    public float soundVolume = 1f;

    public void dropTheLoot()
    {
        if(droppables.Count < 1)
        {
            Debug.LogWarning("No loot found for " + gameObject.name + ", no loot dropped", gameObject);
        }
        else if(!guaranteed && percentChanceForDrop < 1)
        {
            Debug.LogError(gameObject.name + "cannot have a negative percent. No loot dropped.", gameObject);
        }
        else
        {
            int lootIndex = 0;
            if (droppables.Count > 1) lootIndex = Random.Range(0, droppables.Count);

            bool drop = true;
            if (!guaranteed)
            {
                int chance = Random.Range(0, 100);
                if (chance >= percentChanceForDrop) drop = false;
            }

            if (drop) Instantiate(droppables[lootIndex], transform.position, Quaternion.identity);
            // trigger audio event
            if (sound != null)
                AudioManager.audioManager?.playAudio(sound, soundVolume);
        }
    }
}
