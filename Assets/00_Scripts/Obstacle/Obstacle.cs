using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private float Damage;
    
    private void OnTriggerEnter(Collider other)
    {
        if (targetLayer.value == (targetLayer.value | 1 << other.gameObject.layer))
        {
            other.gameObject.GetComponent<Player>().TakeDamage(Damage);
            other.gameObject.transform.position = spawnPosition;
            // 배경 깜빡
        }
    }
}
