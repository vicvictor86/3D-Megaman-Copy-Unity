using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private int life = 2;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float sizeGroundCheck = 1.0f;
    
    private Rigidbody rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
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
        life -= damage;

        if (life <= 0)
        {
            Debug.Log("Player died");
        }
        
        return life;
    }
}
