using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class JumpMotor : MonoBehaviour, IJump
{
    Rigidbody2D rb;

    [Tooltip("How high can I jump")]
    public float jumpHeight = 4f;
    [Tooltip("How does gravity effect me while I am accending")]
    public float upGravityScale = 2f;
    [Tooltip("How does gravity effect me while I am falling")]
    public float downGravityScale = 3f;

    public AudioClip jumpSound;
    [Range(0f,1f)]
    public float soundVolume = 1f;

    [Header("Multiple Jump Options")]
    [Tooltip("The number of jumps I can execute in one go")]
    [SerializeField]
    int numberOfJumps = 1;
    [Tooltip("After the first jump, my following jumps will jump to a height scaled by this number")]
    [SerializeField]
    float secondaryJumpHeightMultiplier = .5f;


    [Header("Ground Detection Options")]
    [Tooltip("Left point which we use to check below us for ground and to the side for a wall.")]
    [SerializeField] Transform leftDetectorPoint;

    [Tooltip("Right point which we use to check below us for ground and to the side for a wall.")]
    [SerializeField] Transform rightDetectorPoint;

    [SerializeField] float groundDetectionDistance = .5f;
    [SerializeField] float wallDetectionDistance = .25f;

    [Tooltip("I will try to run and jump on anything in these layers as if it was ground")]
    [SerializeField] LayerMask whatIsGround;

    

    int jumpCount = 0;

    void Start(){
        rb = GetComponent<Rigidbody2D>();

        if (leftDetectorPoint == null) {
            Debug.LogError(gameObject.name + "'s jump script is missing a left detector point.");
        }

        if (rightDetectorPoint == null){
            Debug.LogError(gameObject.name + "'s jump script is missing a right detector point.");
        }

    }

    void FixedUpdate(){
        UpdateGravityScale();
    }

    public bool Jump(){
        if (CheckGround()){
            jumpCount = numberOfJumps - 1;
        }else if (jumpCount > 0) {
            jumpCount--;
        }
        else{
            return false;
        }

        float h;
        if (jumpCount == numberOfJumps - 1)
        {
            h = jumpHeight;
        }
        else {
            h = jumpHeight * secondaryJumpHeightMultiplier;
        }

        float jumpVel = Mathf.Sqrt(2 * upGravityScale * Mathf.Abs(Physics.gravity.y) * h);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVel);

        if (jumpSound != null && AudioManager.audioManager != null) {
            //play sound
            AudioManager.audioManager.playAudio(jumpSound, soundVolume);
        }

        return true;
    }

    void UpdateGravityScale() {
        if (rb.linearVelocity.y < -.01f){
            rb.gravityScale = downGravityScale;
        }
        else {
            rb.gravityScale = upGravityScale;
        }
    }

    #region PhysicsDetection

    /// <summary>
    /// Check to see if there is any ground below at all
    /// </summary>
    /// <returns> returns if there is any ground </returns>
    public bool CheckGround() {
        RaycastHit2D hitLeft = Physics2D.Raycast(leftDetectorPoint.position, -leftDetectorPoint.up, groundDetectionDistance, whatIsGround);
        RaycastHit2D hitRight = Physics2D.Raycast(rightDetectorPoint.position, -rightDetectorPoint.up, groundDetectionDistance, whatIsGround);

        if (hitLeft.collider || hitRight.collider)
        {
            //Debug.Log(hitLeft.collider + " " + hitRight.collider);
            return true;
        }
        else {
            return false;
        }
    }

    /// <summary>
    /// Check to see if there is a lack of ground infront or behind us
    /// </summary>
    /// <returns> returns whether there is a lack of ground on either side,
    /// returns false if there is no ground at all </returns>
    public bool CheckEdge() {
        RaycastHit2D hitLeft = Physics2D.Raycast(leftDetectorPoint.position, -leftDetectorPoint.up, groundDetectionDistance, whatIsGround);
        RaycastHit2D hitRight = Physics2D.Raycast(rightDetectorPoint.position, -rightDetectorPoint.up, groundDetectionDistance, whatIsGround);

        if ((!hitLeft && hitRight) || (hitLeft && !hitRight)){
            return true;
        }
        else{
            return false;
        }
    }

    public bool CheckWall() {
        RaycastHit2D hitLeft = Physics2D.Raycast(leftDetectorPoint.position, -leftDetectorPoint.right, wallDetectionDistance, whatIsGround);
        RaycastHit2D hitRight = Physics2D.Raycast(rightDetectorPoint.position, rightDetectorPoint.right, wallDetectionDistance, whatIsGround);

        if (hitLeft || hitRight){
            return true;
        }
        else{
            return false;
        }
    }

    #endregion

}
