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
    
    protected override void LookToPlayer(Component player)
    {
        LookToPlayerHorizontally(player);
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
}
