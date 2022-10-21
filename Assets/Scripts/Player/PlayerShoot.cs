using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerShoot : MonoBehaviour
{
    private const int NormalShootDamage = 1;
    private const float NormalShootSpeed = 2;
    private PlayerWeaponSwitch playerWeaponSwitch;

    [SerializeField] private Transform shootPosition;
    [SerializeField] private GameObject normalShootPrefab;
    [SerializeField] private GameObject chargedShootPrefab;
    
    [SerializeField] private float chargingCoolDown = 2;
    private float chargingTime;

    private void Start()
    {
        playerWeaponSwitch = gameObject.GetComponent<PlayerWeaponSwitch>();
    }

    private void Update()
    {
        var currentWeapon = playerWeaponSwitch.actualWeapon;
        if (Input.GetKey(KeyCode.J))
        {
            chargingTime += Time.deltaTime;
        }
        
        if (Input.GetKeyUp(KeyCode.J))
        {
            if (chargingTime >= chargingCoolDown)
            {
                var shoot = Instantiate(chargedShootPrefab, shootPosition.position, Quaternion.identity).GetComponent<Shoot>();
                shoot.SetProperties(shootPosition.forward * currentWeapon.shootSpeed, "Enemy", currentWeapon.damage);
            }
            else
            {
                var shoot = Instantiate(normalShootPrefab, shootPosition.position, Quaternion.identity).GetComponent<Shoot>();
                shoot.SetProperties(shootPosition.forward * NormalShootSpeed, "Enemy", NormalShootDamage);
            }
            chargingTime = 0;
        }
    }
}
