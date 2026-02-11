using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class ProjectileMove : MonoBehaviour
{
    [Header("All settings (direction, speed, lifespan) are set by the attack shoot script")]
    
    Attack attackScript;
    private float speed;
    float life;
    Vector2 direction;
    private Direction flatDirection = Direction.none;
    bool dealtDamage = false;
    bool isNew = true;
    bool isLive;
    bool fromEnemy;
    private Rigidbody2D rb;
    bool destroyOtherProjectiles = false;
    private Transform shooter;

    void setColliderToTrigger()
    {
        GetComponent<Collider2D>().isTrigger = true;
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        setColliderToTrigger();
    }

    void setDirectionAndMove()
    {
        rb.linearVelocity = direction.normalized * speed;
    }
    void Update()
    {
        if (isLive)
        {
            life -= Time.deltaTime;
            if (life < 0)
            {
                isLive = false;
                direction = Vector2.zero;
                if (attackScript)
                {
                    attackScript.returnToPool(gameObject);
                    gameObject.SetActive(false);
                }
                else Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if no attack script is attached to projectile
        if(attackScript == null)
        {
            return;
        }
        
        // if projectile collides with self
        if (attackScript.transform.root == collision.transform.root)
        {
            print("collided with self");
            return;
        }
        
        // if projectile collides with another projectile
        if(collision.GetComponent<ProjectileMove>())
        {
            //ignore collision with other projectiles from the same shooter
            if(collision.GetComponent<ProjectileMove>().shooter == shooter)
            {
                print("Ignoring collision with projectile from same shooter");
                return;
            }
            
            if(destroyOtherProjectiles)
            {
                print("Disabling projectile");
                collision.GetComponent<ProjectileMove>().attackScript?.returnToPool(collision.gameObject);
                collision.gameObject.SetActive(false);
            }
            print("collided with another projectile");
            return;
        }

        if (collision.CompareTag("CheckPoint"))
        {
            // print("collided with checkpoint");
            return;
        }

        if (dealtDamage || !isLive) return;
        if ((fromEnemy && collision.GetComponent<PlayerHealth>()) || (!fromEnemy && collision.GetComponent<EnemyHealth>()))
        {
            print("Dealing damage to: " + collision.gameObject.name);
            dealtDamage = true;
            attackScript?.DealDamage(collision.GetComponent<Health>());
        }

        if (attackScript.transform.root != collision.transform.root )
        {
            // print($" collsion vs attack script object: {collision.transform.root.gameObject != attackScript.GetComponent<Collider2D>().transform.root.gameObject}");
            isLive = false;
            direction = Vector2.zero;
            // Debug.LogError($"Projectile hit something that wasn't the target. Returning to pool: {collision.gameObject.name}");
            if (attackScript) attackScript.returnToPool(gameObject);
            else Destroy(gameObject);
        }
    }

    public void setValues( Attack _attackscript, float _speed, float _lifespan, Vector2 _direction, bool _fromEnemy, bool _destroyOtherProjectiles = false, Transform _shooter = null)
    {
        life = _lifespan;
        speed = _speed;
        destroyOtherProjectiles = _destroyOtherProjectiles;
        isLive = true;
        dealtDamage = false;
        direction = _direction;
        shooter = _shooter;
        // print($"Setting values  - direction: {direction}");
        
        if (isNew)
        {
            isNew = false;
            attackScript = _attackscript;
            fromEnemy = _fromEnemy;
            setColliderToTrigger();
            gameObject.name = "Projectile: " + attackScript.ToString();
        }
        setDirectionAndMove();
    }

    public void disableReturn()
    {
        attackScript = null;
    }
}