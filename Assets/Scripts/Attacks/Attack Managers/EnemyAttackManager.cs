using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackManager : MonoBehaviour
{
    /// This manages cooldowns/damage for any attacks this enemy has
    /// currently enemies can only have ONE attack at a time (changable; requires more AI)

    // finds ONE attack on enemy; set on start (other attacks will be ignored)
    Attack enemyAttack;

    [Header("Disables all attacks on enemy")]
    public bool attacksEnabled = true;

    PlayerHealth player;

    // for pokes
    bool needsCasting = false;
    bool needsAllDirection = true;
    float attackRange;

    public void Start()
    {
        enemyAttack = GetComponent<Attack>();
        if (enemyAttack == null)
        {
            Debug.LogError("No attacks found on " + gameObject.name + "!" + gameObject.name + " is unable to attack.");
            this.enabled = false;
        }

        Invoke(nameof(setPlayerReferenceAndAttacks), 0.5f);
    }

    private void setPlayerReferenceAndAttacks()
    {
        PlayerHealth[] checkForOnePlayer = FindObjectsOfType<PlayerHealth>();
        if(checkForOnePlayer.Length > 1)
        {
            Debug.LogError("Multiple player health scripts found! Enemies will only target the first player found.");
            player = checkForOnePlayer[0];
        }
        else if(checkForOnePlayer.Length == 0)
        {
            Debug.LogError(gameObject.name + ": No players found. Enemy will not attack");
            attacksEnabled = false;
        }
        else
        {
            player = checkForOnePlayer[0];
        }
        enemyAttack.setPlayerRef(player);
        
        if (enemyAttack is PokeFourDirection e)
        {
            needsCasting = true;
            attackRange = e.attackRange;
            if (e is PokeTwoDirection) needsAllDirection = false;
        }
        if (enemyAttack is PokeAllDirection en)
        {
            needsCasting = needsAllDirection = true;
            attackRange = en.attackRange;
        }
        if(enemyAttack is SlashAllDirection es)
        {
            needsCasting = true;
            attackRange = es.attackRange;
        }

    }

    void Update()
    {
        if (!player)
        {
            setPlayerReferenceAndAttacks();
        }
        
        if (needsCasting)
        {
            if(checkDistance(attackRange, needsAllDirection) && !enemyAttack.attacking) StartCoroutine(enemyAttack.ExecuteAttack(enemyAttack.attackSpeed));
        }
        else
        {
            if (attacksEnabled && enemyAttack.enabled)
            {
                // execute attack
                if (!enemyAttack.attacking) StartCoroutine(enemyAttack.ExecuteAttack(enemyAttack.attackSpeed));
            }
        }

        if (GetComponent<ContactAttack>())
        {
            ContactAttack mine = GetComponent<ContactAttack>();
            if (!attacksEnabled && !mine.disable) mine.disable = true;
            else if (attacksEnabled && mine.disable) mine.disable = false;
        }
    }


    /// <summary>
    /// This method uses a CircleCast/RayCast to see if targets are in range
    /// </summary>
    /// <param name="maxRange">Max radius of search</param>
    /// <param name="allDirection">Look all directions or only horizontally?</param>
    bool checkDistance(float maxRange, bool allDirection)
    {
        if (!enemyAttack.attacking)
        {
            if (allDirection)
            {
                //CircleCast
                RaycastHit2D[] targets = Physics2D.CircleCastAll(enemyAttack.attackOffset.position, maxRange, enemyAttack.attackOffset.position);

                if (targets.Length > 0)
                {
                    foreach (RaycastHit2D hit in targets)
                    {
                        if (hit.collider.GetComponent<PlayerHealth>()) //return true;
                        {
                            Debug.Log("hit");
                            return true;
                        }
                    }
                }
            }
            else
            {
                //RayCast left and right

                RaycastHit2D[] targets = Physics2D.RaycastAll(new Vector2(enemyAttack.attackOffset.position.x - maxRange, enemyAttack.attackOffset.position.y), Vector2.right, maxRange * 2F);

                if (targets.Length > 0)
                {
                    foreach (RaycastHit2D hit in targets)
                    {
                        if (hit.collider.GetComponent<PlayerHealth>())
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        if (needsCasting) 
        {
            if (needsAllDirection) Gizmos.DrawWireSphere(enemyAttack.attackOffset.position, attackRange);
            else Gizmos.DrawLine(new Vector2(enemyAttack.attackOffset.position.x - attackRange, enemyAttack.attackOffset.position.y), new Vector2(enemyAttack.attackOffset.position.x + attackRange, enemyAttack.attackOffset.position.y));
        }
    }
}
