using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTwoDirectionPlatformer : ShootFourDirection
{
    override public IEnumerator ExecuteAttack(float attackTime)
    {
        print("shooting");
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
                if (!newProject)
                {
                    newProject = Instantiate(projectile, position, Quaternion.identity);
                }

                if (newProject.GetComponent<ProjectileMove>())
                    newProject.GetComponent<ProjectileMove>().setValues(this, projectileSpeed, liveTime,
                        getVector2FromAngle(i), isEnemy);
                else
                    Debug.LogWarning("ProjectileMove component not found on " + projectile.name +
                                     ". This object will not move!");

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
                    direction = directionFromVector2(false, new Vector2(transform.rotation.x, transform.rotation.y));
                }
                else
                {
                    // follows enemy facing direction
                    direction = directionFromVector2(false, directionFacing);
                }
            }

            StartCoroutine(Shoot(position, direction, 0));
        }


        yield return new WaitForSeconds(attackTime);
        attacking = false;
    }

    private IEnumerator Shoot(Vector2 position, Direction direction, float delay)
    {
        print("shooting");
        yield return new WaitForSeconds(delay);
        // get projectile
        GameObject newProject = projectilePool.pullObject(position);
        if (!newProject) newProject = Instantiate(projectile, position, transform.rotation);

        if (newProject.GetComponent<ProjectileMove>())
            newProject.GetComponent<ProjectileMove>().setValues(this, projectileSpeed, liveTime,
                getVector2FromDirection(direction), isEnemy, destroyOtherProjectiles, transform.root);
        else
            Debug.LogWarning(
                "ProjectileMove component not found on " + newProject.name + ". This object will not move!");

        if (direction == Direction.left)
        {
            newProject.transform.localRotation = transform.rotation;
        }
        else
        {
            newProject.transform.localRotation = transform.rotation;
        }

        //
        // *** Uncomment for Spread shot, be sure to change projectile layer properties
        if(!spreadshot) yield break;
        
        print("spread shot");
        
        for (int i = 0; i < numSpreadShots; i++)
        {
            GameObject newSpreadshot = projectilePool.pullObject(position);
            if (!newSpreadshot)
            {
                newSpreadshot = Instantiate(projectile, position, transform.rotation);
            }

            // set values for spread shot
            if (newSpreadshot.GetComponent<ProjectileMove>())
            {
                var spreadUpOrDown = i % 2 == 0 ? Vector2.up : Vector2.down; 
                var spreadAngle = (spreadWidth == SpreadWidth.tight ? .1f : .3f) / (Mathf.Floor(i/2f) + 1);
                print("angle: " + spreadAngle);
                var spreadDirection = getVector2FromDirection(direction) + spreadUpOrDown * (spreadAngle);
                print("spreadDirection: " + spreadDirection);
                newSpreadshot.GetComponent<ProjectileMove>().setValues(this, projectileSpeed, liveTime,
                    spreadDirection, isEnemy, destroyOtherProjectiles, transform.root);
            }
            else
            {
                Debug.LogWarning(
                    "ProjectileMove component not found on " + projectile.name + ". This object will not move!");
            }

            if (direction == Direction.left)
            {
                newSpreadshot.transform.localRotation = transform.rotation;
            }
            else
            {
                newSpreadshot.transform.localRotation = transform.rotation;
            }

            newSpreadshot.transform.localRotation = transform.rotation * Quaternion.Euler(new Vector3(0, 0, 12));
        }
    }
}