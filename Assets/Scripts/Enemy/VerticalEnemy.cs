using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalEnemy : Enemy
{

    private bool needHorizontalRotation;
    private new void Start()
    {
        base.Start();
        StartCoroutine(ChangeDirection(1f));
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
            var moveDirection = verticalFacing == VerticalFacing.Down ? -1 : 1;
            Rb.velocity = new Vector3(velocity.x, speed * moveDirection, velocity.z);
        }
        else
        {
            Rb.velocity = Vector3.zero;
        }
    }
    
    protected override void LookToPlayer(Component player)
    {
        var moveDirection = (player.transform.position - transform.position).normalized;
        needHorizontalRotation = moveDirection.x < 0 && horizontalFacing == HorizontalFacing.Right ||
                                 moveDirection.x > 0 && horizontalFacing == HorizontalFacing.Left;
        
        if (needHorizontalRotation)
        {
            RotateInHorizontal();
        }
        
        if (moveDirection.y < 0 && verticalFacing == VerticalFacing.Up || moveDirection.y > 0 && verticalFacing == VerticalFacing.Down)
        {
            RotateInVertical();
        }
    }
    
    protected override IEnumerator ChangeDirection(float coolDownChangeDirection)
    {
        while (true)
        {
            yield return new WaitForSeconds(coolDownChangeDirection);
            if (!isViewingPlayer)
            {
                RotateInVertical();
            }
        }
    }
    
    protected new virtual void RotateInVertical()
    {
        verticalFacing = verticalFacing == VerticalFacing.Up ? VerticalFacing.Down : VerticalFacing.Up;
    }

    private void RotateInHorizontal()
    {
        gameObject.transform.Rotate(0.0f, 180.0f, 0.0f, Space.World);
        horizontalFacing = horizontalFacing == HorizontalFacing.Left ? HorizontalFacing.Right : HorizontalFacing.Left;
    }
}
