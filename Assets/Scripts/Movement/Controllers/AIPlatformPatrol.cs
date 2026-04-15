using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlatformPatrol : MonoBehaviour{

    IMove motor;
    IJump jumpMotor;

    SpriteRenderer sr;

    public float shootInterval = 2f;
    float shootTimer;
    ShootAllDirection gun;

    [SerializeField] int direction = 1;

    public float edgeWaitTime = 1f;

    float edgeTimer = .25f;
    bool checkForEdges = true;

    void Start()
    {
        motor = GetComponent<IMove>();
        jumpMotor = GetComponent<IJump>();
        sr = GetComponent<SpriteRenderer>();

        gun = GetComponent<ShootAllDirection>();
        shootTimer = shootInterval;
    }

    void Update() // Use Update for timers
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0)
        {
            // This "pulls the trigger" on your gun script
            StartCoroutine(gun.ExecuteAttack(0.5f));
            shootTimer = shootInterval;
        }
    }

    void FixedUpdate(){
        motor.Move(new Vector2(direction, 0));

        if (checkForEdges && (jumpMotor.CheckEdge() || jumpMotor.CheckWall())) {
            StartCoroutine(SwapDirections());
        }
    }

    void FlipSprite()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction; // ensures correct sign
        transform.localScale = scale;
    }

    IEnumerator SwapDirections()
    {
        checkForEdges = false;
        int newDirection = -direction;
        direction = 0;

        yield return new WaitForSeconds(edgeWaitTime);

        direction = newDirection;

        sr.flipX = (direction > 0); // 👈 THIS LINE

        yield return new WaitForSeconds(edgeTimer);

        checkForEdges = true;
    }
}
