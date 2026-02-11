using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController_withBounds : MonoBehaviour
{

    public Transform target;

    public float smoothingTimePercentage = .1f;

    public bool constrainX = false;
    public bool constraintY = false;

    public Transform leftBounds, rightBounds, upperBounds, lowerBounds;

    void Start()
    {
        if (leftBounds == null || rightBounds == null || upperBounds == null || lowerBounds == null)
        {
            Debug.Log("bounds objects missing on Camera Controller on " + gameObject.name);
        }

    }



    void FixedUpdate(){
        if (target != null) {
            Vector3 differance = target.position - transform.position;
            if (constrainX) {
                differance.x = 0;
            }

            if (constraintY) {
                differance.y = 0;
            }
            
            transform.position = new Vector3(transform.position.x + differance.x * smoothingTimePercentage, transform.position.y + differance.y * smoothingTimePercentage, -10f);

            float newX;
            if (transform.position.x < leftBounds.position.x)
            {
                newX = leftBounds.position.x;
            } else if (transform.position.x > rightBounds.position.x) {
                newX = rightBounds.position.x;
            } else
            {
                newX = transform.position.x;
            }
            float newY;
            if (transform.position.y < lowerBounds.position.y)
            {
                newY = lowerBounds.position.y;
            }
            else if (transform.position.y > upperBounds.position.y)
            {
                newY = upperBounds.position.y;
            } else
            {
                newY = transform.position.y;
            }
            transform.position = new Vector3(newX, newY, transform.position.z);
        }
        
    }
}
