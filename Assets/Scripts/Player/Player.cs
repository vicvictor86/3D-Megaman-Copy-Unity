using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Player Properties")]
    [SerializeField] private int life = 2;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    private Rigidbody rb;
    
    [Header("Ground Collision")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float sizeGroundCheck = 1.0f;

    [Header("ColdDowns")]
    [SerializeField] private float coldDownWallJumping = 1;
    [SerializeField] private float flickFrequency = 0.25f;
    private bool canWallJump = true;
    
    private SkinnedMeshRenderer[] playerRenders;
    private static readonly int Color1 = Shader.PropertyToID("_Color");

    [SerializeField] private float coldDownInvincibility = 1;
    private bool isInvincible;
    
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        playerRenders = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        rb.velocity = new Vector3(horizontalInput * playerSpeed, rb.velocity.y, verticalInput * playerSpeed);

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
    
    private void OnCollisionStay(Collision collision)
    {
        if (!IsGrounded() && collision.contacts[0].normal.y < 0.1f)
        {
            if (Input.GetButton("Jump") && canWallJump)
            {
                Debug.DrawRay(collision.contacts[0].point, collision.contacts[0].normal, Color.red, 1.25f);
                rb.AddForce(new Vector3(collision.contacts[0].normal.x * 8, 0, 0), ForceMode.Impulse);
                rb.velocity = new Vector3(collision.contacts[0].normal.x * 8, jumpHeight, rb.velocity.z);
                canWallJump = false;
                Invoke(nameof(SetWallJumpTrue), coldDownWallJumping);
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
