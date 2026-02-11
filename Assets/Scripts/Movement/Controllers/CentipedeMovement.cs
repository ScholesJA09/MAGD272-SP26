using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentipedeMovement : MonoBehaviour
{

    [SerializeField] Transform leftDetectorPoint;
    [SerializeField] Transform rightDetectorPoint;

    [SerializeField] float wallDetectionDistance;

    [SerializeField] LayerMask whatIsGround;

    [SerializeField] float dropTime;

    IMove motor;
    [SerializeField] bool moveRight = true;

    [SerializeField] bool checkForEdges = true;
    Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        motor = GetComponent<IMove>();
        direction = new Vector2((moveRight) ? 1 : -1, 0);
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    motor.Move(direction);
    //}

    public bool CheckWall()
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(leftDetectorPoint.position, -leftDetectorPoint.right, wallDetectionDistance, whatIsGround);
        RaycastHit2D hitRight = Physics2D.Raycast(rightDetectorPoint.position, rightDetectorPoint.right, wallDetectionDistance, whatIsGround);

        if (moveRight && hitRight || !moveRight && hitLeft)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void FixedUpdate()
    {
        motor.Move(direction);

        if (checkForEdges) { 
            if (CheckWall())
            {
                StartCoroutine(SwapDirections());
            }
        }
    }

    IEnumerator SwapDirections()
    {
        Debug.Log("swapping directions");
        checkForEdges = false;
        // move down
        Vector2 newDirection = new Vector2(0, -1);
        //direction = 

        direction = newDirection;

        yield return new WaitForSeconds(dropTime);
        moveRight = !moveRight;

        direction = new Vector2((moveRight) ? 1 : -1, 0);

        checkForEdges = true;

    }
}
