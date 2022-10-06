using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int life = 2;
    [SerializeField] private int damage = 1;
    [SerializeField] private float rangeVision = 1;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private GameObject shootPrefab;
    private Vector3 sphereCenter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var position = transform.position;
        sphereCenter = new Vector3(position.x, position.y, position.z);
        var insideVision = Physics.OverlapSphere(sphereCenter, rangeVision);

        foreach (var entity in insideVision)
        {
            if (entity.CompareTag("Player"))
            {
                Shoot();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(sphereCenter, rangeVision);
    }

    private void Shoot()
    {
        var shoot = Instantiate(shootPrefab, shootPosition.position, Quaternion.identity).GetComponent<Shoot>();
        shoot.Move(shootPosition);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Player>().TakeDamage(damage);
            Debug.Log("DANO NO PLAYER");
        }
    }
}
