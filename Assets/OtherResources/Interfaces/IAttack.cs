using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack<T>
{
    IEnumerator ExecuteAttack(float attackTime);

    void DealDamage(T target);

    void SetDirection(Vector2 direction);
}
