using System.Collections;
using UnityEngine;

public class ShootFourDirection : ShootAllDirection
{
    [Header("Shoots towards the player?")]
    [Tooltip(
        "Player:\ntrue = Shoots the direction player is facing\nfalse = shoots direction closest to mouse\nEnemy:\ntrue = Will always shoot direction of player\nfalse = Shoots direction they are facing")]
    public bool FollowsPlayer = false;

    [Header("Shoot all directions or only one?")]
    [Tooltip(
        "If ShootFourDirection, turning this true will shoot four directions when triggered\nIf ShootTwoDirection, turning this true will shoot two directions when triggered")]
    public bool ShootsAllDirections = false;

    override public IEnumerator ExecuteAttack(float attackTime)
    {
        if (myAmmo)
        {
            if (ShootsAllDirections)
            {
                if (!myAmmo.CheckForAmmo(4)) yield break;
                else myAmmo.UpdateValue(Collectible_Type.Ammo, -4);
            }
            else
            {
                if (!myAmmo.CheckForAmmo(1)) yield break;
                else myAmmo.UpdateValue(Collectible_Type.Ammo, -1);
            }
        }

        attacking = true;

        if (myAnim) myAnim.SetTrigger("Attack");

        // shoot all directions
        if (ShootsAllDirections)
        {
            Vector2 position = transform.position;
            if (attackOffset) position = attackOffset.position;

            for (int i = -90; i <= 180; i = i + 90) // angles: -90, 0, 90, 180
            {
                GameObject newProject = projectilePool.pullObject(position);
                if (!newProject) newProject = Instantiate(projectile, position, Quaternion.identity);

                if (newProject.GetComponent<ProjectileMove>())
                    newProject.GetComponent<ProjectileMove>().setValues(this, projectileSpeed, liveTime,
                        getVector2FromAngle(i), isEnemy, destroyOtherProjectiles, transform.root);
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
                //follows down
                direction = !FollowsPlayer
                    ? directionFromMouse(true)
                    :
                    //player direction  
                    directionFromVector2(true, directionFacing);
            }
            else
            {
                // goes towards player
                direction = FollowsPlayer
                    ? directionFromVector2(true, playerRef.transform.position - new Vector3(position.x, position.y, 0))
                    :
                    // follows enemy facing direction
                    directionFromVector2(true, directionFacing);
            }

            float rotation = 0;
            if (direction == Direction.right) rotation = 0;
            else if (direction == Direction.left) rotation = 180;
            else if (direction == Direction.up) rotation = 90;
            else if (direction == Direction.down) rotation = -90;

            // get projectile
            GameObject newProject = projectilePool.pullObject(position);
            if (newProject)
            {
                newProject = Instantiate(projectile, position, Quaternion.identity);
            }

            if (newProject.GetComponent<ProjectileMove>())
            {
                newProject.GetComponent<ProjectileMove>().setValues(this, projectileSpeed, liveTime,
                    getVector2FromDirection(direction), isEnemy);
            }
            else
            {
                Debug.LogWarning("ProjectileMove component not found on " + projectile.name +
                                 ". This object will not move!");
            }

            newProject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, rotation));
        }

        yield return new WaitForSeconds(attackTime);
        attacking = false;
    }

}