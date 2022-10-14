using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected enum HorizontalFacing
    {
        Left,
        Right,
    }

    protected enum VerticalFacing
    {
        Up,
        Down
    }
    
    [SerializeField] protected int life = 2;
    [SerializeField] protected int damage = 1;
    [SerializeField] protected float speed = 1;
    
    [SerializeField] protected float rangeVision = 1;
    [SerializeField] protected Transform shootPosition;
    [SerializeField] protected GameObject shootPrefab;
    [SerializeField] protected float coolDownFire = 2;
    [SerializeField] protected float shootSpeed = 1;
    
    protected HorizontalFacing horizontalFacing = HorizontalFacing.Right;
    protected VerticalFacing verticalFacing = VerticalFacing.Down;
    protected bool isViewingPlayer;
    
    protected Vector3 sphereCenter;
    protected float fireTime;
    protected bool firstShoot = true;

    protected Rigidbody Rb;
    
    protected void Start()
    {
        Rb = gameObject.GetComponent<Rigidbody>();
    }

    protected virtual void SearchPlayer()
    {
    }

    protected virtual void Move()
    {
    }
    
    protected virtual IEnumerable<Collider> ObjectsInEnemyVision()
    {
        return null;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(sphereCenter, rangeVision);
    }

    protected virtual void LookToPlayer(Component player)
    {
    }

    protected virtual void RotateEnemy()
    {
    }
    
    protected virtual void Shoot(Component target)
    {
        var positionShoot = shootPosition.position;
        var moveDirection = (target.transform.position - positionShoot).normalized;
        
        var shoot = Instantiate(shootPrefab, positionShoot, Quaternion.identity).GetComponent<Shoot>();
        shoot.SetProperties(moveDirection * shootSpeed, "Player", damage);
        
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

    protected virtual IEnumerator ChangeDirection(float coolDownChangeDirection)
    {
        return null;
    }
}
