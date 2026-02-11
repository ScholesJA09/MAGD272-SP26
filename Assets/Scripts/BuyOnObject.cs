using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyOnObject : MonoBehaviour
{
    public CollectibleManager collectibleManager;
    public GameObject thingToBuyPrefab;
    public Transform spawnPoint;

    public void Buy(int cost)
    {
        if (collectibleManager.coinsCollected >= cost)
        {
            collectibleManager.UpdateValue(Collectible_Type.Coin, -cost);
            if (spawnPoint == null)
            {
                Debug.LogError("No spawn point set!");
            }
            else
            {
                thingToBuyPrefab.SetActive(true);
            }

        }
    }
}
