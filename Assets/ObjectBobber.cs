using UnityEngine;

public class ObjectBobber : MonoBehaviour
{
    public float speed = 2f;      // How fast it bobs
    public float height = 0.5f;   // How far up and down it goes

    private Vector3 startPos;

    void Start()
    {
        // Record the starting position so we bob around it
        startPos = transform.position;
    }

    void Update()
    {
        // Calculate the new Y position using a Sine wave
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * height;

        // Apply the position
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}