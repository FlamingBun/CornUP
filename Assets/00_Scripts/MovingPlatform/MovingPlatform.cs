using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Vector3 originPosition;
    [SerializeField] private  MoveDirection moveDirection;
    [SerializeField] private float range;
    [SerializeField] private float moveSpeed;
    private Vector3 targetPosition;
    private bool isOpposite;
    private bool isArrive;

    private void Start()
    {
        originPosition= transform.position;
        isOpposite = false;
        isArrive = false;
        SetTargetPosition();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        float distance = Vector3.Distance(transform.position, targetPosition);
        if (distance <= 0.1f)
        {
            isArrive = true;
            isOpposite = !isOpposite;
        }

        if (!isArrive) return;
        SetTargetPosition();
    }

    private void SetTargetPosition()
    {
        float tempRange;
        
        tempRange = isOpposite?-range:range;

        switch (moveDirection)
        {
            case MoveDirection.MoveX:
                targetPosition = new Vector3(originPosition.x + tempRange, originPosition.y, originPosition.z);
                break;
            case MoveDirection.MoveY:
                targetPosition = new Vector3(originPosition.x, originPosition.y+ tempRange, originPosition.z);
                break;
            case MoveDirection.MoveZ:
                targetPosition = new Vector3(originPosition.x, originPosition.y, originPosition.z+ tempRange);
                break;
        }
    }
}
