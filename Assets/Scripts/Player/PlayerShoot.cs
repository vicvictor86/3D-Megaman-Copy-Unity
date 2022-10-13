using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerShoot : MonoBehaviour
{
    private int damage = 1;
    private float speed = 2;
    
    [SerializeField] private Transform shootPosition;
    [SerializeField] private GameObject shootPrefab;
    
    [SerializeField] private float chargingCoolDown = 2;
    private float chargingTime;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(KeyCode.J))
        {
            chargingTime += Time.deltaTime;
        }
        
        if (Input.GetKeyUp(KeyCode.J))
        {
            if (chargingTime >= chargingCoolDown)
            {
                chargingTime = 0;
            }
            
            var shoot = Instantiate(shootPrefab, shootPosition.position, Quaternion.identity).GetComponent<Shoot>();
            shoot.SetProperties(shootPosition.forward * speed, "Enemy", damage);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }
}
