using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovingPlatform
{
    IEnumerator movePlatform();
    bool isDest();
    void move();
    void resetPosition();

}
