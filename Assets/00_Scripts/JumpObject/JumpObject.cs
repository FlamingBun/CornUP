using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpObject : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float JumpForce;
    private Animator animator;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (targetLayer.value == (targetLayer.value | 1 << other.gameObject.layer))
        {
            other.gameObject.GetComponent<PlayerController>().SuperJump(JumpForce);
            animator.SetTrigger("Jump");
        }
    }
    
}
