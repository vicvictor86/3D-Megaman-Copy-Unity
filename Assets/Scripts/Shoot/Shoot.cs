using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{

    [SerializeField] private float speed = 0;
    private const float LivingColdDown = 3;
    private float livingTime = 0;
    private Rigidbody rgBody;
    private Transform objectTransform;
    
    // Start is called before the first frame update
    private void Start()
    {
        rgBody = gameObject.GetComponent<Rigidbody>();
        rgBody.velocity = objectTransform.forward * 2;
    }

    private void Update()
    {
        TimeLiving();
    }

    public void Move(Transform updateObjectTransform)
    {
        objectTransform = updateObjectTransform;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(1);
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
