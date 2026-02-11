using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBuyer : MonoBehaviour
{

    public BuyObject[] possibleBuys;

    float timeOfLastPurchace;

    void Update(){
        if (Time.time > timeOfLastPurchace + .65f) {
            timeOfLastPurchace = Time.time;
            Purchace();
        }
    }

    void Purchace() {
        
        if (possibleBuys.Length == 0) {
            return;
        }

        int r = 0;
        int count = 0;
        do{
            r = Random.Range(0, possibleBuys.Length);
            possibleBuys[r].Buy(1);
            count++;
        }
        while (possibleBuys[r] == null && count < possibleBuys.Length);
    }
}
