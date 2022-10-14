using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerShoot : MonoBehaviour
{
    private const int NormalShootDamage = 1;
    private const int ChargedShootDamage = 2;
    private const float NormalShootSpeed = 2;
    private const float ChargedShootSpeed = 4;
    
    [SerializeField] private Transform shootPosition;
    [SerializeField] private GameObject normalShootPrefab;
    [SerializeField] private GameObject chargedShootPrefab;
    
    [SerializeField] private float chargingCoolDown = 2;
    private float chargingTime;
    
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
                var shoot = Instantiate(chargedShootPrefab, shootPosition.position, Quaternion.identity).GetComponent<Shoot>();
                shoot.SetProperties(shootPosition.forward * ChargedShootSpeed, "Enemy", ChargedShootDamage);
            }
            else
            {
                var shoot = Instantiate(normalShootPrefab, shootPosition.position, Quaternion.identity).GetComponent<Shoot>();
                shoot.SetProperties(shootPosition.forward * NormalShootSpeed, "Enemy", NormalShootDamage);
            }
            chargingTime = 0;
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
