using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPatroller : MonoBehaviour
{

    IMove motor;
    [SerializeField] Transform[] patrolPath;
    [SerializeField] bool loop = false;

    int patrolIndex = 0;
    int direction = 1;


    void Start(){
        motor = GetComponent<IMove>();
    }

    void FixedUpdate(){

        if (patrolPath.Length < 2) {
            return;
        }

        Vector2 difference = new Vector2(patrolPath[patrolIndex].position.x - transform.position.x, patrolPath[patrolIndex].position.y - transform.position.y);
        if (difference.sqrMagnitude <= .1f) {
            SetNewWayPoint();
        }

        motor.Move(new Vector2(patrolPath[patrolIndex].position.x, patrolPath[patrolIndex].position.y));

    }

    void SetNewWayPoint() {
        patrolIndex += direction;

        if (patrolIndex == patrolPath.Length && loop){
            patrolIndex = 0;
        }

        if (patrolIndex == patrolPath.Length && !loop){
            patrolIndex = patrolPath.Length - 2;
            direction = -1;
        }
        else if (patrolIndex < 0) {
            patrolIndex = 1;
            direction = 1;
        }
    }
}
