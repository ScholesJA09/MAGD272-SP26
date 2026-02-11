using UnityEngine;

public class MovingEnemy : MonoBehaviour
{
    public float defaultMoveSpeed = 1;
    private float moveSpeed = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = defaultMoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSpeed(float speed)
    {
        moveSpeed = speed;
    }
}
