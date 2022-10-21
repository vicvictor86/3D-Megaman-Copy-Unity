using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSwitch : MonoBehaviour
{
    [SerializeField] private List<Weapon> playerWeapons;
    [SerializeField] public Weapon actualWeapon;
    private int currentWeaponIndex;
    private SkinnedMeshRenderer[] playerRenders;
    private static readonly int Color1 = Shader.PropertyToID("_Color");
    
    private void Start()
    {
        playerRenders = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        actualWeapon = playerWeapons[0];
        SelectWeapon();
    }

    private void Update()
    {
        var previousWeaponIndex = currentWeaponIndex;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetKeyDown(KeyCode.E))
        {
            currentWeaponIndex++;

            if (currentWeaponIndex >= playerWeapons.Count)
            {
                currentWeaponIndex = 0;
            }
        }
    
        if (Input.GetAxis("Mouse ScrollWheel") < 0f || Input.GetKeyDown(KeyCode.Q))
        {
            currentWeaponIndex--;

            if (currentWeaponIndex < 0)
            {
                currentWeaponIndex = playerWeapons.Count - 1;
            }
        }
    
        if (currentWeaponIndex != previousWeaponIndex)
        {
            SelectWeapon();
        }
    }
    
    private void SelectWeapon()
    {
        actualWeapon = playerWeapons[currentWeaponIndex];
        foreach (var item in playerRenders)
        {
            item.material.SetColor(Color1, actualWeapon.color);
        }
    }
}
