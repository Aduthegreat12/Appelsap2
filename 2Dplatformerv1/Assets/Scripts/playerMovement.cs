using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;
using UnityEngine.EventSystems;
public class playerMovement : MonoBehaviour
{
    public Vector2 input;
    private float output;
    private float startingGravity;
    private float idleClimbSpeed;
    public InputValue inputJump;
    private Rigidbody2D myRigidBody;
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 0.02f;
    [SerializeField] float climbSpeed = 5f;
    Animator myAnimator;
    Collider2D myCapsuleCollider;
    private bool isJumpingTrue;
    private bool touching;
    private bool isPlayerClimbing;
    private bool isPlayerJumping;
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        startingGravity = myRigidBody.gravityScale;
    }

    void Update()
    {
        Run();
        FlipSprite();
        climbLadder();
        output = input.y * climbSpeed;
        Debug.Log(output);
    }

    void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
    }
    void OnJump(InputValue value2)
    {
        inputJump = value2;
        if(value2.isPressed && myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && !myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRigidBody.velocity += new Vector2(0f, jumpSpeed);
            isPlayerJumping = !myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
            myAnimator.SetBool("isJumping", isPlayerJumping);
        }
         
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2((input.x) * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
        bool isPlayerMoving = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", isPlayerMoving);

    }
    void FlipSprite()
    {
        bool isPlayerMoving = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (isPlayerMoving == true)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }
    void climbLadder()
    {
        isPlayerClimbing = !(myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) && myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"));
        myAnimator.SetBool("isClimbing", isPlayerClimbing);
        if ((myRigidBody.velocity.y == 0) && myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            idleClimbSpeed = 0f;
        }
        else
        {
            idleClimbSpeed = 1f;
        }
        myAnimator.speed = idleClimbSpeed;
        Debug.Log(myRigidBody.velocity.y);

        if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, climbSpeed * input.y);
            myRigidBody.velocity = climbVelocity;
            myRigidBody.gravityScale = 0f;
        }
        else
        {
            myRigidBody.gravityScale = startingGravity;
            return;
        }
    
    } 
}
 