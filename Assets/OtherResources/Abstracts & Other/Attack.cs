using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour, IAttack<Health>
{
    // This attacks are generic for players or enemies. Players need PlayerAttackManager and Enemies need EnemyAttackManager

    [Header("Damage attack amount")]
    public int damage = 1;

    [Header("How much time does the attack take?")][Tooltip("Melee: Attack will hit halfway between this time\nShoot: Cooldown time before able to shoot again")]
    public float attackSpeed = 0.5F;

    [Header("How much time before the attack can be used again?")]
    public float cooldown = 0.5F;
    
    [Header("What key/mouse button activates this attack?")][Tooltip("Only applies to GameObjects with a PlayerAttackManager component")]
    public KeyCode attackInput = KeyCode.Mouse0;

    [Header("Used if attacks aren't where you want them.")]
    [Tooltip("You can use an empty object that's a child of this GameObject. If this is empty, attacks will trigger at (0, 0) on the player")]
    public Transform attackOffset;

    public bool attacking { get; protected set; }
    protected bool isEnemy { get; set; }

    protected PlayerHealth playerRef;   // needed for enemy targetting

    protected Vector2 directionFacing;

    public Animator myAnim;

    virtual public void DealDamage(Health target)
    {
        target.TakeDamage(damage);
    }

    virtual public IEnumerator ExecuteAttack(float attackTime)
    {
        attacking = true;
        if (myAnim)
        {
            myAnim.SetTrigger("Attack");
        }
        Debug.Log(gameObject.name + " is attacking!");
        yield return new WaitForSeconds(attackTime);
        Debug.Log(gameObject.name + " is done attacking.");
        attacking = false;
    }

    virtual public void SetDirection(Vector2 direction)
    {
        if(direction != Vector2.zero) directionFacing = direction;
    }

    protected bool RaycastForDamage(Vector2 direction, float range, LayerMask attackLayer)
    {
        RaycastHit2D[] targets = Physics2D.RaycastAll(attackOffset.transform.position, direction, range, attackLayer);

        if (targets.Length > 0)
        {
            foreach (RaycastHit2D hit in targets)
            {
                if (hit.collider.GetComponent<Health>())
                {
                    DealDamage(hit.collider.GetComponent<Health>());
                }
                else Debug.LogWarning(hit.collider.name + " has been hit. Does it need a health component?");
            }

            return true;
        }
        else return false;
    }

    virtual protected void Awake()
    {
        directionFacing = Vector2.right;
        myAnim = GetComponent<Animator>();

        if (damage <= 0)
        {
            Debug.LogWarning(gameObject.name + "'s damage is too low! Defaulting to 1...", gameObject);
            damage = 1;
        }

        if (attackSpeed <= 0)
        {
            Debug.LogWarning(gameObject.name + "'s attack duration is too small! Defaulting to 0.5...", gameObject);
            attackSpeed = 0.5F;
        }

        if(attackInput == KeyCode.None && GetComponent<PlayerAttackManager>())
        {
            Debug.LogError("No input set for " + this.name + ", disabling attack.", gameObject);
            this.enabled = false;
        }
        if (GetComponent<EnemyAttackManager>())
        {
            // Debug.Log(gameObject.name + " is an enemy.", gameObject);
            isEnemy = true;
        }
        if (!attackOffset)
        {
            attackOffset = transform;
        }
    }

    virtual public void returnToPool(GameObject obj)
    {
        Debug.Log("This shouldn't ever be called. Needed for inheritance.");
    }

    public void setPlayerRef(PlayerHealth p)
    {
        if (p) playerRef = p;
    }
}

public enum Direction   // this is for me to make sense of my attacks. it can be changed when movement is implemented
{
    up,
    down,
    left,
    right,
    none
}
