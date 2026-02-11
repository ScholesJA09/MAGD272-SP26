using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Utility;

public class SlashAllDirection : Attack
{
    [Header("Use this script for a slash attack that hits in all directions. \n" +
            "Use Slash - Weapon Anchor prefab for visuals.")]
    
    [Header("How far can the attack hit?")]
    public float attackRange = 1;

    [Header("Layers that can be hit")]
    // public LayerMask attackLayer;

    [Header("Have a Gameobject that visually shows the poke?")][Tooltip("To use this, add the WeaponAnchor PREFAB to your object. The sprites are changeable.")]
    public GameObject weaponAnchor;

    [Header("How many degrees does the slash cover?")]
    public int slashDegrees = 360;
    
    [Header("Only enable damage while attacking?")]
    public bool onlyDamageWhileAttacking = false;

    public override  IEnumerator ExecuteAttack(float attackTime)
    {
        print("SLASH");
        attacking = true;
        // enableWeaponCollider(true);
        if (myAnim)
        {
            myAnim.SetTrigger("Attack");
        }

        Vector2 direction;
        if (!isEnemy)
        {
            var mouse = Input.mousePosition;
            direction = (Camera.main.ScreenToWorldPoint(mouse) - attackOffset.position);
            direction.Normalize();
        }
        else
        {
            // how enemy sets direction
            direction = (playerRef.transform.position - attackOffset.position);
            direction.Normalize();
        }
        
        Debug.DrawRay(attackOffset.position, direction * attackRange, Color.red, attackTime * 2);

        if (weaponAnchor)
        {
            float rotation = Vector2.Angle(Vector2.right, direction);
            if (direction.y < 0) rotation = -rotation;
            weaponAnchor.transform.localRotation = Quaternion.Euler(0, 0, rotation);

            weaponAnchor.GetComponentInChildren<ContactAttack>().damage = damage;
            weaponAnchor.transform.GetComponent<Animator>().SetTrigger("Attack");
        }
        
        // if ray cast hits, do damage
        // if (!ShootRaycast(direction, attackRange, attackLayer))
        // {
        //     Debug.Log(gameObject.name + " missed their attack!: " + direction + " - " + attackRange);
        // }
        // enableWeaponCollider(false);
        yield return new WaitForSeconds(cooldown);
        attacking = false;
    }

    override protected void Awake()
    {
        base.Awake();

        if (attackRange <= 0)
        {
            Debug.LogWarning(gameObject.name + "'s attack range is too small! Defaulting to 1...");
            attackRange = 1;
        }

        if (weaponAnchor)
        {
            if(attackSpeed != 1)
            {
                //int newFrameRate = Mathf.RoundToInt((1F / attackSpeed) * 8F);
                //weaponAnchor.transform.GetComponentInChildren<Animator>().runtimeAnimatorController.animationClips[1].frameRate = newFrameRate;
                weaponAnchor.transform.GetComponentInChildren<Animator>().speed = (1F * 1.5f) / (attackSpeed );
            }
        }

        if (onlyDamageWhileAttacking)
        {
            enableWeaponCollider(false);
        }
    }
    
    public void enableWeaponCollider(bool enable)
    {
        weaponAnchor.GetComponentInChildren<Collider2D>().isTrigger = true;
        weaponAnchor.GetComponentInChildren<Collider2D>().enabled = enable;
        print(weaponAnchor.GetComponentInChildren<Collider2D>().enabled);
    }

    private void Update()
    {
        if (!playerRef)
        {
            playerRef = Utility.Utility.FindPlayer().GetComponent<PlayerHealth>();
        }
        
        if (!attacking && weaponAnchor)
        {
            Vector2 direction;
            if(!isEnemy) direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - attackOffset.position);
            else direction = (playerRef.transform.position - attackOffset.position);
            direction.Normalize();

            float rotation = Vector2.Angle(Vector2.right, direction);
            if (direction.y < 0) rotation = -rotation;

            weaponAnchor.transform.localRotation = Quaternion.Euler(0, 0, rotation);
        }
    }
}
