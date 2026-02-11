using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    Queue<Transform> realPoints = new Queue<Transform>();

    [Header("How many points would you like this platform to travel between? Drag them in.")]
    public Transform[] points;

    [Header("How fast would you like the platform to go?")]
    public float speed;

    private float maxSpeed; //variable to store speed if move on touch is true
    private Transform posA;

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
        foreach (Transform p in points)
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
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.localPosition, posA.position, speed * Time.deltaTime);
    }
    public bool isDest()
    {
        return posA.position == gameObject.transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IMove>() != null)
        {
            collision.transform.SetParent(gameObject.transform);
        }
        if (moveOnTouch)
        {
            speed = maxSpeed;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<IMove>() != null)
        {
            collision.transform.SetParent(null);
          
        }
        if (moveOnTouch)
        {
            speed = 0;
        }

    }
    public void resetPosition()
    {
        gameObject.transform.position = respawn;
    }
}
