using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableInput : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerInputController input = collision.GetComponent<PlayerInputController>();
        
        if(input != null)
        {
            //input.gameObject.GetComponent<IMove>().Move(Vector2.zero);
            //Destroy(input);
            input.gameObject.GetComponent<IMove>().Move(Vector2.zero);
            input.enabled = false;
        }
    }
}
