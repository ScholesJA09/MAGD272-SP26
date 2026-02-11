using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class PlatformChaser : MonoBehaviour
{
    [Tooltip("The target we try to chase.")]
    public GameObject target;

    [Tooltip("The distance at which we start chasing the target.")]
    [SerializeField] float agroDistance = 10f;

    [Tooltip("The distance at which we stop chasing the target.")]
    [SerializeField] float stoppingDistance = .1f;

    [Tooltip("The minimum distance we try to be from the target.")]
    [SerializeField] float minimumDistance = .05f;

    IMove motor;
    IJump jumpMotor;
    bool isAgro = false;
    float lastUpdate = 0f;

    void Start()
    {
        motor = GetComponent<IMove>();
        jumpMotor = GetComponent<IJump>();
        target = Utility.Utility.FindPlayer();
    }

    void FixedUpdate(){
        if (!target)
        {
            target = Utility.Utility.FindPlayer();
        }
        
        //Check to see if we should go aggresive
        if (!isAgro && (target.transform.position - transform.position).magnitude <= agroDistance){
            isAgro = true;
        }

        //if we are aggresive, lets move towards our target
        if (isAgro && Time.time > lastUpdate + .1f){
            lastUpdate = Time.time;
            if (target){
                if (Mathf.Abs(target.transform.position.x - transform.position.x) <= minimumDistance)
                {
                    //We are closer than we want, so lets back up to our minimum distance

                    float retreatDirection = transform.position.x - target.transform.position.x;
                    retreatDirection = retreatDirection / Mathf.Abs(retreatDirection);

                    motor.Move(new Vector2(retreatDirection, 0));

                }
                else if (Mathf.Abs(target.transform.position.x - transform.position.x) <= stoppingDistance) {
                    motor.Move(Vector2.zero);
                }
                else
                {
                    //We are not too close, lets move closer
                    motor.Move(new Vector2(target.transform.position.x - transform.position.x, 0).normalized);
                }

            }
            else{
                Debug.Log(gameObject.name + " is agro, but has no target assigned");
            }

            if (jumpMotor != null) {
                if (jumpMotor.CheckEdge() || jumpMotor.CheckWall()) {
                    jumpMotor.Jump();
                }
            }
        }
    }
}
