using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private readonly int isMoving = Animator.StringToHash("IsMoving");
    private readonly int isWalking = Animator.StringToHash("IsWalking");

    private Animator animator;
    private PlayerController playerController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        playerController.IdleAction += OnIdle;
        playerController.JumpAction += OnJump;
        playerController.WalkAction += OnWalk;
        playerController.MoveAction += OnMove;
    }

    private void OnDisable()
    {
        playerController.IdleAction -= OnIdle;
        playerController.JumpAction -= OnJump;
        playerController.WalkAction -= OnWalk;
        playerController.MoveAction -= OnMove;
    }

    private void OnIdle()
    {
        animator.SetBool(isWalking, false);
        animator.SetBool(isMoving, false);
    }
    
    private void OnJump()
    {
        animator.SetTrigger("Jump");
    }

    private void OnWalk(bool value)
    {
        animator.SetBool(isWalking, value);
    }

    private void OnMove()
    {
        animator.SetBool(isMoving, true);
    }
}
