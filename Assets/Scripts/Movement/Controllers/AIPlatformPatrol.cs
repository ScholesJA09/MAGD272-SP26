using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlatformPatrol : MonoBehaviour{

    IMove motor;
    IJump jumpMotor;

    [SerializeField] int direction = 1;

    public float edgeWaitTime = 1f;

    float edgeTimer = .25f;
    bool checkForEdges = true;

    void Start(){
        motor = GetComponent<IMove>();
        jumpMotor = GetComponent<IJump>();
    }

    void FixedUpdate(){
        motor.Move(new Vector2(direction, 0));

        if (checkForEdges && (jumpMotor.CheckEdge() || jumpMotor.CheckWall())) {
            StartCoroutine(SwapDirections());
        }
    }

    IEnumerator SwapDirections() {
        checkForEdges = false;
        int newDirection = -direction;
        direction = 0;

        yield return new WaitForSeconds(edgeWaitTime);

        direction = newDirection;

        yield return new WaitForSeconds(edgeTimer);

        checkForEdges = true;
    }
}
