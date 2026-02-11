using UnityEngine;

public class TowerEnemyAttackManager : MonoBehaviour
{
    /// This manages cooldowns/damage for any attacks this enemy has
    /// currently enemies can only have ONE attack at a time (changeable; requires more AI)

    // finds ONE attack on enemy; set on start (other attacks will be ignored)
    Attack attackScript;

    [Header("Disables all attacks on enemy")]
    public bool attacksEnabled = true;

    PlayerHealth player;

    // for pokes
    private bool needsCasting;
    private bool needsAllDirection = true;
    private float attackRange;

    void Start()
    {
        attackScript = GetComponent<Attack>();

        if (attackScript == null)
        {
            Debug.LogError("No attacks found on " + gameObject.name + "! " + gameObject.name + " is unable to attack. Disabling script.");
            this.enabled = false;
        }

        PlayerHealth[] checkForOnePlayer = FindObjectsOfType<PlayerHealth>();
        if (checkForOnePlayer.Length > 1)
        {
            Debug.LogError("Multiple PlayerHealth scripts found! Enemies will only target the first player found.");
            player = checkForOnePlayer[0];
        }
      
        attackScript?.setPlayerRef(player);

        if (attackScript is PokeFourDirection e)
        {
            needsCasting = true;
            attackRange = e.attackRange;
            if (e is PokeTwoDirection) needsAllDirection = false;
        }
        else if (attackScript is PokeAllDirection en)
        {
            needsCasting = needsAllDirection = true;
            attackRange = en.attackRange;
        }

        Debug.Log("Need Casting: " + needsCasting + " - needsAllDirection: " + needsAllDirection);
    }

    private void Update()
    {
        if (needsCasting)
        {
            if (checkDistance(attackRange, needsAllDirection) && !attackScript.attacking) StartCoroutine(attackScript.ExecuteAttack(attackScript.attackSpeed));
        }
        else
        {
            if (attacksEnabled && attackScript.enabled)
            {
                // fight stuff
                if (!attackScript.attacking) StartCoroutine(attackScript.ExecuteAttack(attackScript.attackSpeed));
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
        if (!attackScript.attacking)
        {
            if (allDirection)
            {
                //CircleCast
                RaycastHit2D[] targets = Physics2D.CircleCastAll(attackScript.attackOffset.position, maxRange, attackScript.attackOffset.position);

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

                RaycastHit2D[] targets = Physics2D.RaycastAll(new Vector2(attackScript.attackOffset.position.x - maxRange, attackScript.attackOffset.position.y), Vector2.right, maxRange * 2F);

                if (targets.Length > 0)
                {
                    foreach (RaycastHit2D hit in targets)
                    {
                        if (hit.collider.GetComponent<PlayerHealth>()) return true;
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
            if (needsAllDirection) Gizmos.DrawWireSphere(attackScript.attackOffset.position, attackRange);
            else Gizmos.DrawLine(new Vector2(attackScript.attackOffset.position.x - attackRange, attackScript.attackOffset.position.y), new Vector2(attackScript.attackOffset.position.x + attackRange, attackScript.attackOffset.position.y));
        }
    }
}
