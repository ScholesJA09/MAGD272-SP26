using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetChaser : MonoBehaviour
{
    [Header("This chaser needs a Top Down Point Motor \n in order to work.")]
    
    [Header("Auto set to player?")] public bool autoSetPlayer = true;

    [Tooltip("The target we try to chase.")]
    public GameObject target;


    [Tooltip("The distance at which we start chasing the target.")] [SerializeField]
    float agroDistance = 10f;

    [Tooltip("The distance at which we stop chasing the target.")] [SerializeField]
    float stoppingDistance = .1f;

    [Tooltip("The minimum distance we try to be from the target.")] [SerializeField]
    float minimumDistance = .05f;

    IMove motor;
    bool isAgro = false;
    float lastUpdate = 0f;

    void Start()
    {
        motor = GetComponent<IMove>();
        if (autoSetPlayer)
        {
            target = Utility.Utility.FindPlayer();
        }
    }

    void FixedUpdate()
    {
        if(!target && autoSetPlayer)
        {
            target = Utility.Utility.FindPlayer();
        }
        //Check to see if we should go aggresive
        if (!isAgro && (target.transform.position - transform.position).magnitude <= agroDistance)
        {
            isAgro = true;
        }

        //if we are aggressive, lets move towards our target
        if (isAgro && Time.time > lastUpdate + .1f)
        {
            lastUpdate = Time.time;
            if (target)
            {
                if ((target.transform.position - transform.position).sqrMagnitude <= minimumDistance * minimumDistance)
                {
                    //We are closer than we want, so lets back up to our minimum distance
                    Vector3 retreatVector = transform.position - target.transform.position;
                    retreatVector.z = 0;
                    retreatVector = retreatVector.normalized * minimumDistance;
                    Vector3 retreatLocation = retreatVector + target.transform.position;

                    motor.Move(new Vector2(retreatLocation.x, retreatLocation.y));
                }
                else if ((target.transform.position - transform.position).sqrMagnitude <=
                         stoppingDistance * stoppingDistance)
                {
                    // within the stopping distance
                    motor.Move(Vector2.zero);
                }
                else
                {
                    //We are not too close, lets move closer
                    motor.Move(new Vector2(target.transform.position.x, target.transform.position.y));
                }
            }
            else
            {
                Debug.Log(gameObject.name + " is agro, but has no target assigned");
            }
        }
    }
}