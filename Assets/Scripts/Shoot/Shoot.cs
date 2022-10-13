using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private string target;
    
    private const float LivingColdDown = 3;
    private float livingTime;
    
    private Rigidbody rgBody;
    private Vector3 shootDirection;

    private int damage;
    
    private void Start()
    {
        rgBody = gameObject.GetComponent<Rigidbody>();
        rgBody.velocity = shootDirection;
        
    }

    private void Update()
    {
        TimeLiving();
    }

    public void SetProperties(Vector3 updateShootDirection, string updateTarget, int updateDamage)
    {
        shootDirection = updateShootDirection;
        target = updateTarget;
        damage = updateDamage;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(target))
        {
            if (target == "Player")
            {
                other.GetComponent<Player>().TakeDamage(damage);
            }
            else if (target == "Enemy")
            {
                other.GetComponent<Enemy>().TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }

    private void TimeLiving()
    {
        livingTime += Time.deltaTime;
        if (livingTime >= LivingColdDown)
        {
            Destroy(gameObject);
        }
    }
}
