using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthEnemy : Enemy
{
    private bool needHorizontalRotation;
    
    private new void Start()
    {
        base.Start();
        StartCoroutine(ChangeDirection());
    }
    
    private void Update()
    {
        Move();

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
    
    protected override void Move()
    {
        if (!isViewingPlayer)
        {
            var velocity = Rb.velocity;
            var moveDirection = depthFacing == DepthFacing.Inside ? -1 : 1;
            Rb.velocity = new Vector3(velocity.x, velocity.y, speed * moveDirection);
        }
        else
        {
            Rb.velocity = Vector3.zero;
        }
    }
    
    protected override void LookToPlayer(Component player)
    {
        var moveDirection = LookToPlayerHorizontally(player);
        
        if (moveDirection.z < 0 && depthFacing == DepthFacing.Out || moveDirection.z > 0 && depthFacing == DepthFacing.Inside)
        {
            RotateInDepth();
        }
    }
    
    protected override IEnumerator ChangeDirection()
    {
        while (true)
        {
            yield return new WaitForSeconds(coolDownChangeDirection);
            if (!isViewingPlayer)
            {
                RotateInDepth();
            }
        }
    }
    
    protected virtual void RotateInDepth()
    {
        depthFacing = depthFacing == DepthFacing.Inside ? DepthFacing.Out : DepthFacing.Inside;
    }
}
