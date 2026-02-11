using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectible
{ 
    void UpdateValue(Collectible_Type type, int amount);
}
