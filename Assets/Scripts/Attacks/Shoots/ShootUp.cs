using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootUp: ShootAllDirection
{
    override public IEnumerator ExecuteAttack(float attackTime)
    {
        if (myAmmo)
        {
            if (!myAmmo.CheckForAmmo(1)) yield break;
            else myAmmo.UpdateValue(Collectible_Type.Ammo, -1);
        }

        attacking = true;

        if (myAnim) myAnim.SetTrigger("Attack");

        // get angle
        Vector2 direction = transform.up;
        if (attackOffset) direction = attackOffset.up;
        // calculate how to rotate projectile relative to the 'direction'
        float rotation = Vector2.Angle(Vector2.up, direction);
        if (direction.y < 0) rotation = -rotation;

        Vector2 position = transform.position;
        if (attackOffset) position = attackOffset.position;

        // get projectile
        GameObject newProject = projectilePool.pullObject(position);
        if (newProject == null) newProject = Instantiate(projectile, position, Quaternion.identity);

        if (newProject.GetComponent<ProjectileMove>()) newProject.GetComponent<ProjectileMove>().setValues(this, projectileSpeed, liveTime, direction, isEnemy, destroyOtherProjectiles,  transform.root);
        else Debug.LogWarning("ProjectileMove component not found on " + projectile.name + ". This object will not move!");

        newProject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        yield return new WaitForSeconds(attackTime);
        attacking = false;
    }
}
