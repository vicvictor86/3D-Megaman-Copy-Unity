using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalEnemy : Enemy
{
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
            var moveDirection = horizontalFacing == HorizontalFacing.Left ? -1 : 1;
            Rb.velocity = new Vector3(speed * moveDirection, velocity.y, velocity.z);
        }
    }
    
    protected override void LookToPlayer(Component player)
    {
        LookToPlayerHorizontally(player);
    }
    
    protected override IEnumerator ChangeDirection()
    {
        while (true)
        {
            yield return new WaitForSeconds(coolDownChangeDirection);
            if (!isViewingPlayer)
            {
                RotateInHorizontal();
            }
        }
    }
}
