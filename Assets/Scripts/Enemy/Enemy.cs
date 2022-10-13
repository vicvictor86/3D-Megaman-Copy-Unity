using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private enum Facing
    {
        Left,
        Right
    } 
    
    [SerializeField] private int life = 2;
    [SerializeField] private int damage = 1;
    
    [SerializeField] private float rangeVision = 1;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private GameObject shootPrefab;
    [SerializeField] private float coldDownFire = 2;
    private Facing facing = Facing.Right;
    
    private Vector3 sphereCenter;
    private float fireTime = 0;
    private bool firstShoot = true;

    private Rigidbody rb;
    
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {

        var insideVision = ObjectsInEnemyVision();

        fireTime += Time.deltaTime;
        foreach (var entity in insideVision)
        {
            if (entity.CompareTag("Player"))
            {
                LookToPlayer(entity);

                if (fireTime >= coldDownFire || firstShoot)
                {
                    Shoot(entity);
                    firstShoot = false;
                }
            }
        }
    }

    private IEnumerable<Collider> ObjectsInEnemyVision()
    {
        var position = transform.position;
        sphereCenter = new Vector3(position.x, position.y, position.z);
        return Physics.OverlapSphere(sphereCenter, rangeVision);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(sphereCenter, rangeVision);
    }

    private void LookToPlayer(Component player)
    {
        var moveDirection = (player.transform.position - transform.position).normalized;
        if (moveDirection.x < 0 && facing == Facing.Right || moveDirection.x > 0 && facing == Facing.Left)
        {
            gameObject.transform.Rotate(0.0f, 180.0f, 0.0f, Space.World);
            facing = facing == Facing.Left ? Facing.Right : Facing.Left;
        }
    }
    
    private void Shoot(Component target)
    {
        var positionShoot = shootPosition.position;
        var moveDirection = (target.transform.position - positionShoot).normalized;
        
        var shoot = Instantiate(shootPrefab, positionShoot, Quaternion.identity).GetComponent<Shoot>();
        shoot.SetProperties(moveDirection, "Player", damage);
        
        fireTime = 0;
    }

    public int TakeDamage(int damageTaken)
    {
        life -= damageTaken;
        
        if (life <= 0)
        {
            Debug.Log("Enemy died");
        }
        
        return life;
    }
}
