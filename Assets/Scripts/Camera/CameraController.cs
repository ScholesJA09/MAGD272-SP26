using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;

    public float smoothingTimePercentage = .1f;

    public bool constrainX = false;
    public bool constraintY = false;

    void Start()
    {
        
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
        }
        
    }
}
