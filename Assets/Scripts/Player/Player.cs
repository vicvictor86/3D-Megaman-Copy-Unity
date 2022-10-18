using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private enum Facing
    {
        Left,
        Right
    }

    [Header("Player Properties")]
    [SerializeField] private int life = 2;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float wallSlidingSpeed = 1.0f;
    private Rigidbody rb;
    
    [Header("Collisions")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float sizeGroundCheck = 1.0f;
    [SerializeField] private Transform inFront;

    [Header("ColdDowns")]
    [SerializeField] private float coldDownWallJumping = 1;
    [SerializeField] private float flickFrequency = 0.25f;
    private bool canWallJump = true;
    
    private SkinnedMeshRenderer[] playerRenders;
    private static readonly int Color1 = Shader.PropertyToID("_Color");

    [SerializeField] private float coldDownInvincibility = 1;
    private bool isInvincible;

    private Facing actualFacing = Facing.Right;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        playerRenders = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    private void Update()
    {
        Move();
    }

    private void FixedUpdate()
    {
        CheckInFromWallColliding();
    }

    private void ChangeDirection()
    {
        if (rb.velocity.x > 0 && actualFacing == Facing.Left || rb.velocity.x < 0 && actualFacing == Facing.Right)
        {
            gameObject.transform.Rotate(0.0f, 180.0f, 0.0f, Space.World);
            actualFacing = actualFacing == Facing.Left ? Facing.Right : Facing.Left;
        }
    }
    
    private void Move()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");
        
        rb.velocity = new Vector3(horizontalInput * playerSpeed, rb.velocity.y, verticalInput * playerSpeed);
        
        ChangeDirection();

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, sizeGroundCheck);
    }

    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, sizeGroundCheck, ground);
    }
    
    private void Jump()
    {
        var velocity = rb.velocity;
        velocity = new Vector3(velocity.x, jumpHeight, velocity.z);
        rb.velocity = velocity;
    }

    public int TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            life -= damage;
            isInvincible = true;
        }

        StartCoroutine(FlickInvincibility());
        Invoke(nameof(DisableInvincibility), coldDownInvincibility);
        
        if (life <= 0)
        {
            Debug.Log("Player died");
        }
        
        return life;
    }

    private IEnumerator FlickInvincibility()
    {
        while (isInvincible)
        {
            foreach (var item in playerRenders)
            {
                item.enabled = !item.enabled;
            }
            yield return new WaitForSeconds(flickFrequency);
        }
    }

    private void CheckInFromWallColliding()
    {
        RaycastHit hit;
        var inFrontHit = Physics.Raycast(inFront.position, inFront.TransformDirection(Vector3.forward), out hit, 0.1f, ground);
        if (!IsGrounded() && inFrontHit && hit.normal.y < 0.1f)
        {
            if (Input.GetButton("Jump") && canWallJump)
            {
                rb.AddForce(new Vector3(hit.normal.x * 8, 0, 0), ForceMode.Impulse);
                rb.velocity = new Vector3(hit.normal.x * 8, jumpHeight, rb.velocity.z);
                canWallJump = false;
                Invoke(nameof(SetWallJumpTrue), coldDownWallJumping);
            }

            var inputDirection = Input.GetAxis("Horizontal"); 
            var playerGoingToWall = inputDirection >= 0 && hit.normal.x < 0 || inputDirection < 0 && hit.normal.x >= 0; 
            
            if (Input.GetButton("Horizontal") && playerGoingToWall && !Input.GetButton("Jump"))
            {
                var velocity = rb.velocity;
                rb.velocity = new Vector3(velocity.x, Mathf.Clamp(velocity.y, -wallSlidingSpeed, float.MaxValue), velocity.z);
            }
        }
    }

    private void SetWallJumpTrue()
    {
        canWallJump = true;
    }

    private void DisableInvincibility()
    {
        isInvincible = false;
        foreach (var item in playerRenders)
        {
            item.enabled = true;
        }
    }
}
