using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeForward : PokeAllDirection
{
    override public IEnumerator ExecuteAttack(float attackTime)
    {
        attacking = true;

        if (myAnim) myAnim.SetTrigger("Attack");
        if (weaponAnchor) weaponAnchor.transform.GetComponentInChildren<Animator>().SetTrigger("Attack");

        Vector2 direction = transform.up;
        Debug.DrawRay(attackOffset.position, direction * attackRange, Color.red, attackTime);

        yield return new WaitForSeconds(attackTime / 2);
        if (!RaycastForDamage(direction, attackRange, attackLayer)) Debug.Log(gameObject.name + " missed their attack!");

        yield return new WaitForSeconds(attackTime / 2);
        attacking = false;
    }

    private void Update()
    {
        //nothing
    }
}
