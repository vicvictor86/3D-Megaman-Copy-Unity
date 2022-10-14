using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperEnemy : Enemy
{
    [SerializeField] private float jumpHeight = 1;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float sizeGroundCheck = 1;
    [SerializeField] private LayerMask ground;
    
    private new void Start()
    {
        base.Start();
        StartCoroutine(LoopingJump(1f));
    }
    
    private void Update()
    {
        fireTime += Time.deltaTime;
        
        SearchPlayer();
    }
    
    protected override void SearchPlayer()
    {
        var insideVision = ObjectsInEnemyVision();
        isViewingPlayer = false;
        foreach (var entity in insideVision)
        {
            if (entity.CompareTag("Player"))
            {
                isViewingPlayer = true;
                LookToPlayer(entity);

                if (fireTime >= coolDownFire || firstShoot)
                {
                    Shoot(entity);
                    firstShoot = false;
                }
            }
        }
    }

    protected override IEnumerable<Collider> ObjectsInEnemyVision()
    {
        var position = transform.position;
        sphereCenter = new Vector3(position.x, position.y, position.z);
        return Physics.OverlapSphere(sphereCenter, rangeVision);
    }

    protected override void LookToPlayer(Component player)
    {
        var moveDirection = (player.transform.position - transform.position).normalized;
        if (moveDirection.x < 0 && horizontalFacing == HorizontalFacing.Right || moveDirection.x > 0 && horizontalFacing == HorizontalFacing.Left)
        {
            RotateEnemy();
        }
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            var velocity = Rb.velocity;
            velocity = new Vector3(velocity.x, jumpHeight, velocity.z);
            Rb.velocity = velocity;
        }
    }
    
    private IEnumerator LoopingJump(float coolDownJump)
    {
        while (true)
        {
            yield return new WaitForSeconds(coolDownJump);
            if (!isViewingPlayer)
            {
                Jump();
            }
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
    
    protected new virtual void RotateEnemy()
    {
        gameObject.transform.Rotate(0.0f, 180.0f, 0.0f, Space.World);
        horizontalFacing = horizontalFacing == HorizontalFacing.Left ? HorizontalFacing.Right : HorizontalFacing.Left;
    }
}
