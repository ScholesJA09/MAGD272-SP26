using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMove
{
    [field: SerializeField]
    public float speed { get; set; }
    void Move(Vector2 direction);

    void UpdateAnimations(float horizontal, float vertical);
}
