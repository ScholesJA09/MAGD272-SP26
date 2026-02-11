using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(IMove))]
public class CustomPlayerInputController : MonoBehaviour
{

    public string horizontalMappingName;
    public string verticalMappingName;

    IMove motor;
    IJump jumpMotor;

    void Start(){
        motor = GetComponent<IMove>();
        jumpMotor = GetComponent<IJump>();
    }

    void Update(){
        if (motor != null) {
            motor.Move(new Vector2(Input.GetAxisRaw(horizontalMappingName), Input.GetAxisRaw(verticalMappingName)));
        }

        if (jumpMotor != null) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                jumpMotor.Jump();
            }
        }
        
    }
}
