using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalEnemy : Enemy
{
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
            var moveDirection = facing == Facing.Left ? -1 : 1;
            Rb.velocity = new Vector3(speed * moveDirection, velocity.y, velocity.z);
        }
    }
    
    protected override void LookToPlayer(Component player)
    {
        var moveDirection = (player.transform.position - transform.position).normalized;
        if (moveDirection.x < 0 && facing == Facing.Right || moveDirection.x > 0 && facing == Facing.Left)
        {
            RotateEnemy();
        }
    }
    
    protected override IEnumerator ChangeDirection(float coolDownChangeDirection)
    {
        while (true)
        {
            yield return new WaitForSeconds(coolDownChangeDirection);
            if (!isViewingPlayer)
            {
                RotateEnemy();
            }
        }
    }
    
    protected new virtual void RotateEnemy()
    {
        gameObject.transform.Rotate(0.0f, 180.0f, 0.0f, Space.World);
        facing = facing == Facing.Left ? Facing.Right : Facing.Left;
    }
}
