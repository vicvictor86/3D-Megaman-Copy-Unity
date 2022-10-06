using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerShoot : MonoBehaviour
{

    private int damage = 0;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private GameObject shootPrefab;
    [SerializeField] private float chargingCoolDown = 2;
    private float chargingTime = 0;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
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
            shoot.Move(shootPosition);
        }
    }
}
