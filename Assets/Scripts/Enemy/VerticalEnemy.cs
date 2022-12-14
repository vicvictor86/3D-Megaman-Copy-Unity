using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalEnemy : Enemy
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
        var moveDirection = LookToPlayerHorizontally(player);
        
        if (moveDirection.y < 0 && verticalFacing == VerticalFacing.Up || moveDirection.y > 0 && verticalFacing == VerticalFacing.Down)
        {
            RotateInVertical();
        }
    }
    
    protected override IEnumerator ChangeDirection()
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
    
    protected virtual void RotateInVertical()
    {
        verticalFacing = verticalFacing == VerticalFacing.Up ? VerticalFacing.Down : VerticalFacing.Up;
    }
}
