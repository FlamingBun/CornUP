using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    private bool isWalking = false;
    public float moveSpeed;
    public float jumpPower;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;
    
    [Header("Look")] 
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;
    public bool canLook = true; // 인벤토리가 켜졌을 때 시선을 고정하고 커서가 나오게 하기 위한 bool 
    
    private Rigidbody _rigidbody;

    // events
    public event Action IdleAction;
    public event Action JumpAction;
    public event Action<bool> WalkAction;
    public event Action MoveAction;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    void LateUpdate()
    {
        if(canLook)
        {
            CameraLook();
        }
    }

    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        if (isWalking)
        {
            dir *= (moveSpeed / 2f);
        }
        else
        {
            dir *= moveSpeed;
        }


        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
    }

    void CameraLook()
    {
        // mouseDelta - 마우스를 좌우로 움직이는 양
        camCurXRot += mouseDelta.y * lookSensitivity; // Rotation을 x방향으로 움직일 때는 y축을 기준으로 움직이고 
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0); // y방향으로 움직일 때는 x 축을 기준으로 움직여야 한다.

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // 키가 한 번 눌렸을 때 : InputActionPhase.Performed
        // 키가 눌렸을 때 :
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
            MoveAction?.Invoke();
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
            IdleAction?.Invoke();
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {   
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
            JumpAction?.Invoke();
        }
    }

    public void OnWalk(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && !isWalking)
        {
            isWalking = true;
            WalkAction?.Invoke(true);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isWalking = false;
            WalkAction?.Invoke(false);
        }
    }

    bool IsGrounded()
    {
        // 4개의 Ray를 만든다.
        // 플레이어(transform)을 기준으로 앞뒤좌우 0.2씩 떨어뜨려서.
        // 0.01 정도 살짝 위로 올린다.
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            // groundLayerMask에 걸렸으면 true 반환 -- 땅이라는 의미
            if(Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }
    

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}
