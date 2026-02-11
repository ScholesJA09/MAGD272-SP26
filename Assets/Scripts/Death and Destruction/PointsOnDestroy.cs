using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsOnDestroy : MonoBehaviour
{
    public int pointValue;
    private void OnDestroy()
    {
        PointsUI.pointsUI?.addPoints(pointValue);
    }
}
