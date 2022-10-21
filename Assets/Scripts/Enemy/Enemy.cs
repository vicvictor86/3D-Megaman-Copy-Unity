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

    protected enum DepthFacing
    {
        Inside,
        Out
    }

    [Header("Basic Properties")] [SerializeField]
    protected int life = 2;

    [SerializeField] protected int damage = 1;
    [SerializeField] protected float speed = 1;
    [SerializeField] protected float shootSpeed = 1;
    [SerializeField] protected int armor = 0;
    protected bool canTakeDamage = true;

    [Header("Specific Properties")] [SerializeField]
    protected float rangeVision = 1;

    [SerializeField] protected float coolDownFire = 2;
    [SerializeField] protected float coolDownChangeDirection = 1;
    [SerializeField] protected Transform shootPosition;
    [SerializeField] protected GameObject shootPrefab;

    protected HorizontalFacing horizontalFacing = HorizontalFacing.Right;
    protected VerticalFacing verticalFacing = VerticalFacing.Down;
    protected DepthFacing depthFacing = DepthFacing.Inside;
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
        var position = transform.position;
        sphereCenter = new Vector3(position.x, position.y, position.z);
        return Physics.OverlapSphere(sphereCenter, rangeVision);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(sphereCenter, rangeVision);
    }

    protected virtual void LookToPlayer(Component player)
    {
    }

    protected Vector3 LookToPlayerHorizontally(Component player)
    {
        var moveDirection = (player.transform.position - transform.position).normalized;
        var needHorizontalRotation = moveDirection.x < 0 && horizontalFacing == HorizontalFacing.Right ||
                                     moveDirection.x > 0 && horizontalFacing == HorizontalFacing.Left;

        if (needHorizontalRotation)
        {
            RotateInHorizontal();
        }

        return moveDirection;
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
        if (!canTakeDamage) return 0;

        var damageMitigated = damageTaken - armor;
        damageMitigated = damageMitigated < 0 ? 0 : damageMitigated;
        life -= damageMitigated;

        if (life <= 0)
        {
            Debug.Log("Enemy died");
        }

        return life;
    }

    protected void RotateInHorizontal()
    {
        gameObject.transform.Rotate(0.0f, 180.0f, 0.0f, Space.World);
        horizontalFacing = horizontalFacing == HorizontalFacing.Left ? HorizontalFacing.Right : HorizontalFacing.Left;
    }

    protected virtual IEnumerator ChangeDirection()
    {
        return null;
    }
}