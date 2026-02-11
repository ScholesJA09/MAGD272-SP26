using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationChaser : MonoBehaviour
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
    bool isAgro = false;
    float lastUpdate = 0f;
    Vector2 movementInput = Vector2.zero;

    void Start(){
        motor = GetComponent<IMove>();
    }

    void FixedUpdate()
    {
        //Check to see if we should go aggresive
        if (!isAgro && (target.transform.position - transform.position).magnitude <= agroDistance){
            isAgro = true;
        }

        //if we are aggresive, lets determin how we should be moving
        if (isAgro)
        {
            lastUpdate = Time.time;
            if (target != null){
                //Some vector math to figure out which way to turn;
                Vector3 direction = target.transform.position - transform.position;
                direction.z = 0;
                direction = direction.normalized;
                float a = -transform.up.x * direction.y + transform.up.y * direction.x;
                if (Mathf.Abs(a) < 0.1f)
                {
                    a = 0;
                }
                else {
                    a = a / Mathf.Abs(a);
                }
                Debug.Log(a);

                //now lets take that rotational direction and figure out if we should move forward of backward
                if ((target.transform.position - transform.position).sqrMagnitude <= minimumDistance * minimumDistance){
                    //We are closer than we want, so lets back up
                    motor.Move(new Vector2(a, -1));
                }
                else if ((target.transform.position - transform.position).sqrMagnitude <= Mathf.Pow(stoppingDistance, 2)){
                    //we are near the minimum, but within our slack distance, so lets just stop moving;
                    motor.Move(new Vector2(a, 0));
                }
                else{
                    //We are not too close, lets move closer
                    motor.Move(new Vector2(a, 1));
                }

            }
            else{
                Debug.Log(gameObject.name + " is agro, but has no target assigned");
            }
        }

    }
}
