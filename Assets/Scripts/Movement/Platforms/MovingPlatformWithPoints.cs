using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformWithPoints : MonoBehaviour
{
    Queue<Vector2> realPoints = new Queue<Vector2>();
    [Header("How many points would you like this platform to travel between?")]
    public List<Vector2> points;

    [Header("How fast would you like the platform to go?")]
    public float speed;

    private float maxSpeed; //variable to store speed if move on touch is true
    private Vector3 posA;

    public bool moveOnTouch = false;

    public Vector2 respawn;
    // Start is called before the first frame update
    void Start()
    {
        respawn = gameObject.transform.position;

        maxSpeed = speed;

        if (moveOnTouch)
        {
            speed = 0;
        }
        foreach (Vector2 p in points)
        {
            realPoints.Enqueue(p);
        }

        StartCoroutine(movePlatform());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        move();
    }
    public IEnumerator movePlatform()
    {
        while (true)
        {
            posA = realPoints.Dequeue();
            realPoints.Enqueue(posA);
            yield return new WaitUntil(isDest);
        }
    }
    public void move()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.localPosition, posA, speed * Time.deltaTime);
    }
    public bool isDest()
    {
        return posA == gameObject.transform.position;
    }
  /*  private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IMove>() != null)
        {
            collision.transform.SetParent(gameObject.transform);

        }
        if (moveOnTouch)
        {
            speed = maxSpeed;
        }

    }*/
  /*  private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<IMove>() != null)
        {
            collision.transform.SetParent(null);
        }
        if (moveOnTouch)
        {
            speed = 0;
        }

    }*/

    private void OnDrawGizmos()
    {
        if(points != null)
        {
            Gizmos.color = Color.black;
            foreach(Vector3 p in points)
            {
                Gizmos.DrawSphere(p, 0.5f);
            }
        }
    }

    public void resetPosition()
    {
        gameObject.transform.position = respawn;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<IMove>() != null)
        {
            collision.transform.SetParent(gameObject.transform);

        }
        if (moveOnTouch)
        {
            speed = maxSpeed;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<IMove>() != null)
        {
            collision.transform.SetParent(null);
        }
        if (moveOnTouch)
        {
            speed = 0;
        }
    }
}
