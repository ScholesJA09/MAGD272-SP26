using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTwoDirection : ShootFourDirection
{
    override public IEnumerator ExecuteAttack(float attackTime)
    {
        if (myAmmo)
        {
            if (ShootsAllDirections)
            {
                if (!myAmmo.CheckForAmmo(2)) yield break;
                else myAmmo.UpdateValue(Collectible_Type.Ammo, -2);
            }
            else
            {
                if (!myAmmo.CheckForAmmo(1)) yield break;
                else myAmmo.UpdateValue(Collectible_Type.Ammo, -1);
            }
        }

        attacking = true;

        if (myAnim) myAnim.SetTrigger("Attack");

        if (ShootsAllDirections)
        {
            Vector2 position = transform.position;
            if (attackOffset) position = attackOffset.position;

            for (int i = 0; i <= 180; i = i + 180) // angles: 0, 180
            {
                GameObject newProject = projectilePool.pullObject(position);
                if (newProject == null) newProject = Instantiate(projectile, position, Quaternion.identity);

                if (newProject.GetComponent<ProjectileMove>()) newProject.GetComponent<ProjectileMove>().setValues(this, projectileSpeed, liveTime, getVector2FromAngle(i), isEnemy);
                else Debug.LogWarning("ProjectileMove component not found on " + projectile.name + ". This object will not move!");

                newProject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, i));
            }
        }

        else
        {
            Vector2 position = transform.position;
            if (attackOffset) position = attackOffset.position;

            // get angle
            Direction direction;
            if (!isEnemy)
            {
                if (!FollowsPlayer)
                {
                    direction = directionFromMouse(false);
                }
                else
                {
                    //player direction
                    direction = directionFromVector2(false, directionFacing);
                }
            }
            else
            {
                if (FollowsPlayer)
                {
                    // goes towards player
                    direction = directionFromVector2(false, playerRef.transform.position - new Vector3(position.x, position.y, 0));
                }
                else
                {
                    // follows enemy facing direction
                    direction = directionFromVector2(false, directionFacing);
                }
            }

            StartCoroutine(NewMethod(position, direction, 0));
        }


        yield return new WaitForSeconds(attackTime);
        attacking = false;
    }

    private IEnumerator NewMethod(Vector2 position, Direction direction, float delay)
    {
        yield return new WaitForSeconds(delay);
        // get projectile
        GameObject newProject = projectilePool.pullObject(position);
        if (newProject == null) newProject = Instantiate(projectile, position, Quaternion.identity);

        if (newProject.GetComponent<ProjectileMove>()) newProject.GetComponent<ProjectileMove>().setValues(this, projectileSpeed, liveTime, getVector2FromDirection(direction), isEnemy, destroyOtherProjectiles, transform.root);
        else Debug.LogWarning("ProjectileMove component not found on " + projectile.name + ". This object will not move!");

        if (direction == Direction.left) newProject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
        else newProject.transform.localRotation = Quaternion.identity;
    }
}
