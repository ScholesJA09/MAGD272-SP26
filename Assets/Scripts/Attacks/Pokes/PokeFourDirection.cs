using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeFourDirection : PokeAllDirection
{
    [Header("Attacks face where the mouse is or where the character is facing?")][Tooltip("Does not apply to enemy; they will attack towards player")]
    public bool FollowsMouse = false;

    public override IEnumerator ExecuteAttack(float attackTime)
    {
        attacking = true;
        print("my anim: " + myAnim);

        if (myAnim) myAnim.SetTrigger("Attack");

        Vector2 direction;
        if (FollowsMouse) direction = directionFromMouse(true);
        else
        {
            if (isEnemy)
            {
                while(!playerRef)
                {
                    playerRef = Utility.Utility.FindPlayer().GetComponent<PlayerHealth>();
                    Debug.LogWarning("Player reference not found for " + gameObject.name + "! Cannot attack.");
                    // attacking = false;
                    yield return new WaitForSeconds(1.5f);
                }
                Vector2 value = playerRef.transform.position - attackOffset.position;
                // print("Value: " + value);
                value.Normalize();
                direction = directionFromVector2(true, value);
            }
            else
            {
                direction = directionFromVector2(true, directionFacing);
            }
        }

        if (weaponAnchor)
        {
            print("weaponAnchor rotation: " + direction);
            float rotation = Vector2.Angle(Vector2.right, direction);
            if (direction.y < 0) rotation = -rotation;
            weaponAnchor.transform.localRotation = Quaternion.Euler(0, 0, rotation);

            weaponAnchor.transform.GetComponentInChildren<Animator>().SetTrigger("Attack");
        }

        Debug.DrawRay(attackOffset.position, direction * attackRange, Color.red, attackTime);

        yield return new WaitForSeconds(attackTime / 2);
        // if ray cast hits, do damage
        if (!RaycastForDamage(direction, attackRange, attackLayer))
        {
            Debug.Log(gameObject.name + " missed their attack!");
        }

        // cooldown
        yield return new WaitForSeconds(cooldown);
        attacking = false;
    }

    protected Vector2 directionFromMouse(bool fourDirections)
    {
        /// Takes mouse position and returns direction closest to it
        /// fourDirections = true;      returns Vector2.left/right/up/down
        /// fourDirections = false;     returns Vector2.left/right

        Vector3 mouse = Input.mousePosition;
        Vector2 mousePos = (Camera.main.ScreenToWorldPoint(mouse) - attackOffset.position);
        mousePos.Normalize();

        return directionFromVector2(fourDirections, mousePos);
    }

    protected Vector2 directionFromVector2(bool fourDirections, Vector2 direct)
    {
        if (!fourDirections || Mathf.Abs(direct.x) >= Mathf.Abs(direct.y))
        {
            // left or right
            if (direct.x > 0) return Vector2.right;
            else return Vector2.left;
        }
        else
        {
            // up or down
            if (direct.y > 0) return Vector2.up;
            else return Vector2.down;
        }
    }

    override protected void Awake()
    {
        base.Awake();

        if (GetComponent<EnemyAttackManager>() && FollowsMouse)
        {
            Debug.LogWarning("Enemies cannot use the Mouse to attack! Disabling FollowsMouse for " + gameObject.name + ".");
            FollowsMouse = false;
        }
    }

    private void Update()
    {
        if (!attacking && weaponAnchor)
        {
            if (FollowsMouse || isEnemy)
            {
                Vector2 direction;
                if (!isEnemy)
                {
                    direction = directionFromVector2(true, Camera.main.ScreenToWorldPoint(Input.mousePosition) - attackOffset.position);
                }
                else
                {
                    if (!playerRef)
                    {
                        playerRef = Utility.Utility.FindPlayerHealth();
                        Debug.LogWarning("Player reference not found for " + gameObject.name + "! Cannot attack.");
                        // return;
                    }
                    direction = directionFromVector2(true, playerRef.transform.position - attackOffset.position);
                }
                direction.Normalize();

                float rotation = Vector2.Angle(Vector2.right, direction);
                if (direction.y < 0) rotation = -rotation;

                weaponAnchor.transform.localRotation = Quaternion.Euler(0, 0, rotation);
            }
            else
            {
                // takes direction from player direction
                Vector2 direction = directionFromVector2(true, directionFacing);

                float rotation = Vector2.Angle(Vector2.right, direction);
                if (direction.y < 0) rotation = -rotation;

                weaponAnchor.transform.localRotation = Quaternion.Euler(0, 0, rotation);
            }
        }
    }
}
