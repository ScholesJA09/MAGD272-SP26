using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMover : MonoBehaviour
{
    IMove motor;

    public Vector2 direction;
    public float speed = 1f;

    void Start(){
        motor = GetComponent<IMove>();
    }

    void FixedUpdate(){

        motor.Move(direction.normalized);

    }
}
