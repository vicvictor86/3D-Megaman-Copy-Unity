using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    [SerializeField] private int swordDamage = 1;
    [SerializeField] private Transform centerDamageCircle;
    [SerializeField] private float hitBoxCircleRange;

    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            var objectsHits = HitSword();

            foreach (var hit in objectsHits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    hit.GetComponent<Enemy>().TakeDamage(swordDamage);
                }
            }
        }
    }
    
    private IEnumerable<Collider> HitSword()
    {
        return Physics.OverlapSphere(centerDamageCircle.position, hitBoxCircleRange);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(centerDamageCircle.position, hitBoxCircleRange);
    }
}
