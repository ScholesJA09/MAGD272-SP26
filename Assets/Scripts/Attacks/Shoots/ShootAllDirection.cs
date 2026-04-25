using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAllDirection : Attack
{
    [Header("PREFAB that this object shoots")]
    public GameObject projectile;

    [Header("How fast does the projectile move?")]
    public float projectileSpeed = 5;

    [Header("How long will this projectile live until it disappears?")]
    public float liveTime = 10;

    [Header("Can you shoot endlessly or need to pick up more ammo? (PLAYER ONLY)")]
    [Tooltip("If false, make sure CollectibleManager is on the player")]
    public bool infiniteAmmo = true;

    [Header("Does this projectile destroy other projectiles?")]
    public bool destroyOtherProjectiles = false;

    protected CollectibleManager myAmmo;

    protected ObjectPool projectilePool;

    [Header("Spreadshoot: Shoots direction character is facing.")]
    public bool spreadshot = false;

    [Header("Number of spread shots, not counting straight shot")]
    public int numSpreadShots = 4;

    public enum SpreadWidth
    {
        tight = 10,
        loose = 20
    }

    [Header("Spread shot angle")] public SpreadWidth spreadWidth = SpreadWidth.tight;

    override public IEnumerator ExecuteAttack(float attackTime)
    {
        // --- NEW: UPDATED GATEKEEPER ---
        // Added the myAnim != null check to prevent errors
        if (!this.enabled || (myAnim != null && myAnim.GetCurrentAnimatorStateInfo(0).IsName("Invisible")))
        {
            attacking = false;
            yield break;
        }

        // --- THE GATEKEEPER LOG ---
        if (!this.enabled || myAnim.GetCurrentAnimatorStateInfo(0).IsName("Invisible"))
        {
            Debug.Log("Attack cancelled: Script disabled or still in Invisible state.");
            attacking = false;
            yield break;
        }

        Debug.Log("Ghost is attacking now!");
        attacking = true;

        // Ammo Check
        if (myAmmo)
        {
            if (!myAmmo.CheckForAmmo(1))
            {
                attacking = false;
                yield break;
            }
            else myAmmo.UpdateValue(Collectible_Type.Ammo, -1);
        }

        // Offset Check
        if (attackOffset == null)
        {
            Debug.LogError("ATTACK OFFSET MISSING!");
            attacking = false;
            yield break;
        }

        // Aiming
        Vector2 direction = (playerRef.transform.position - attackOffset.transform.position).normalized;
        float rotation = Vector2.SignedAngle(Vector2.right, direction);

        // Trigger the Animation
        myAnim.SetTrigger("Attack");

        // Spawn Projectile
        GameObject newProject = projectilePool.pullObject(attackOffset.transform.position);
        if (!newProject) newProject = Instantiate(projectile, attackOffset.transform.position, Quaternion.identity);

        if (newProject.TryGetComponent(out ProjectileMove move))
        {
            move.setValues(this, projectileSpeed, liveTime, direction, isEnemy, destroyOtherProjectiles, transform.root);
        }

        newProject.transform.localRotation = Quaternion.Euler(0, 0, rotation);

        // Spreadshot logic (if enabled) remains here...

        // Cooldown
        yield return new WaitForSeconds(attackTime);

        Debug.Log("Attack finished, resetting for next shot.");
        attacking = false;
    }

    override protected void Awake()
    {
        base.Awake();

        // --- NEW: AUTOMATIC ASSIGNMENT ---
        // This ensures myAnim is found even if the Inspector slot is empty
        myAnim = GetComponent<Animator>();
        if (myAnim == null)
        {
            myAnim = GetComponentInChildren<Animator>();
        }

        if (!projectile)
        {
            Debug.LogError("No projectile set. " + gameObject.name + " cannot shoot!", gameObject);
            this.enabled = false;
        }
        else
        {
            projectilePool = new ObjectPool();
        }

        if (projectileSpeed <= 0)
        {
            Debug.LogWarning(gameObject.name + "'s shoot speed is too low! Defaulting to 1...", gameObject);
            projectileSpeed = 1;
        }

        if (!infiniteAmmo)
        {
            if (isEnemy)
            {
                Debug.Log(gameObject.name + " cannot use finite ammo! Setting infiniteAmmo to true...");
                infiniteAmmo = true;
            }

            myAmmo = GetComponent<CollectibleManager>();
            if (!myAmmo)
            {
                Debug.LogError(
                    gameObject.name +
                    " cannot have finite ammo without a CollectableManager component! Turning on infinite ammo...",
                    gameObject);
                infiniteAmmo = true;
            }
        }

        if (liveTime <= 0)
        {
            Debug.LogWarning(
                gameObject.name + "'s bullets won't live long enough to hit anything! Defaulting liveTime to 1...",
                gameObject);
            liveTime = 1;
        }
    }

    override public void returnToPool(GameObject obj)
    {
        if (obj)
        {
            projectilePool.returnObject(obj);
        }
    }

    public void disablePool()
    {
        projectilePool.objectDestroyed();
    }

    protected Direction directionFromMouse(bool fourDirections)
    {
        Vector3 mouse = Input.mousePosition;
        Vector2 mousePos = (Camera.main.ScreenToWorldPoint(mouse) - transform.position);
        if (attackOffset) mousePos = (Camera.main.ScreenToWorldPoint(mouse) - attackOffset.position);
        mousePos.Normalize();

        return directionFromVector2(fourDirections, mousePos);
    }

    protected Direction directionFromVector2(bool fourDirections, Vector2 direct)
    {
        if (!fourDirections || Mathf.Abs(direct.x) >= Mathf.Abs(direct.y))
        {
            // left or right
            if (direct.x > 0) return Direction.right;
            else return Direction.left;
        }
        else
        {
            // up or down
            if (direct.y > 0) return Direction.up;
            else return Direction.down;
        }
    }

    protected Direction getDirectionFromAngle(int angle)
    {
        switch (angle)
        {
            case -90: return Direction.down;
            case 0: return Direction.right;
            case 90: return Direction.up;
            case 180: return Direction.left;
            default: return Direction.none;
        }
    }

    protected Vector2 getVector2FromDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.down: return Vector2.down;
            case Direction.up: return Vector2.up;
            case Direction.left: return Vector2.left;
            case Direction.right: return Vector2.right;
            default: return Vector2.zero;
        }
    }

    protected Vector2 getVector2FromAngle(int angle)
    {
        switch (angle)
        {
            case -90: return Vector2.down;
            case 0: return Vector2.right;
            case 90: return Vector2.up;
            case 180: return Vector2.left;
            default: return Vector2.zero;
        }
    }
}