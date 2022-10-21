using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public enum PlayerWeapons
    {
        Default,
        Fire,
        Ice
    }
    
    public new string name;
    public int damage;
    public float shootSpeed;
    public Color color;
    public PlayerWeapons typeWeapon;
}
