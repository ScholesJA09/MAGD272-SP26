using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(IMove))]
public class PlayerInputController : MonoBehaviour
{
    [Header("This script is used to control the player's movement and jumping.")]
    IMove motor;
    IJump jumpMotor;

    void Start(){
        motor = GetComponent<IMove>();
        jumpMotor = GetComponent<IJump>();
        if(motor is null){
            Debug.LogWarning("No Motor component found on " + gameObject.name);
        }
        if(jumpMotor is null){
            Debug.LogWarning("No JumpMotor component found on " + gameObject.name);
        }
    }

    void Update(){
        motor?.Move(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));

        if (jumpMotor != null) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                jumpMotor.Jump();
            }
        }
    }
}
