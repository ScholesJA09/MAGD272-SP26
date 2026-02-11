using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomeOverTime : MonoBehaviour{
    public CollectibleManager collectibleManager;
    public int amount = 1;
    public float paymentInterval = 1f;

    float lastPayment;


    void Start() {
        if (collectibleManager == null) {
            collectibleManager = GetComponent<CollectibleManager>();
        }
        
    }

    void Update(){
        if (collectibleManager != null) {

            if (Time.time >= lastPayment + paymentInterval) {
                lastPayment = Time.time;
                collectibleManager.UpdateValue(Collectible_Type.Coin, amount);
            }

        }
    }
}
