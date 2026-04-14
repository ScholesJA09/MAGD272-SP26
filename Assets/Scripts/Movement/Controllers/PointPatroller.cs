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

    void FixedUpdate()
    {
        if (patrolPath.Length < 2) return;

        // 1. Calculate the direction vector (Target - Current)
        Vector2 targetPos = patrolPath[patrolIndex].position;
        Vector2 currentPos = transform.position;
        Vector2 moveDirection = targetPos - currentPos;

        // 2. Check if we are close enough to the point
        // Note: I increased this to .2f for better physics reliability
        if (moveDirection.sqrMagnitude <= .2f)
        {
            SetNewWayPoint();
        }

        // 3. PASS THE DIRECTION, NOT THE POSITION
        motor.Move(moveDirection);
    }

    void SetNewWayPoint()
    {
        patrolIndex += direction;

        // Case 1: We reached the end of the list and we WANT to loop (P1 -> P2 -> P1)
        if (patrolIndex >= patrolPath.Length && loop)
        {
            patrolIndex = 0;
        }
        // Case 2: We reached the end of the list and we want to PING-PONG (Go backwards)
        else if (patrolIndex >= patrolPath.Length && !loop)
        {
            patrolIndex = patrolPath.Length - 2;
            direction = -1;
        }
        // Case 3: We reached the start of the list while going backwards
        else if (patrolIndex < 0)
        {
            patrolIndex = 1;
            direction = 1;
        }
    }
}
