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
    public float lookSensitivity;
    public bool canLook = true; // 인벤토리가 켜졌을 때 시선을 고정하고 커서가 나오게 하기 위한 bool 
    
    private float minXLook;
    private float maxXLook;
    private bool isThirdPersonMode;
    
    private float camCurXRot;
    private Vector2 mouseDelta;

    [Header("Third Person Mode")]
    public float third_minXLook;
    public float third_maxXLook;
    public Vector3 third_position;
    
    [Header("First Person Mode")]
    public float first_minXLook;
    public float first_maxXLook;
    public Vector3 first_position;
    
    private Rigidbody _rigidbody;
    private Player player;
    private MovingPlatform movingPlatform;
    
    // events
    public event Action IdleAction;
    public event Action JumpAction;
    public event Action<bool> WalkAction;
    public event Action MoveAction;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        player = GetComponent<Player>();
        isThirdPersonMode = false;
        minXLook = first_minXLook;
        maxXLook = first_maxXLook;
        cameraContainer.localPosition = first_position;
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

    void Update()
    {
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
        if (context.phase == InputActionPhase.Started && IsGrounded() && player.stamina.Value>=10)
        {   
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
            player.stamina.Subtract(10);
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
    
    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            GameManager.Instance.UIManager.OpenUI(UIKey.InventoryUI);
            Debug.Log("inventory");
            ToggleCursor();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            GameManager.Instance.UIManager.CloseUI();
            Debug.Log("Close inventory");
            ToggleCursor();
        }
    }

    public void OnCameraSwap(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (isThirdPersonMode)
            {
                minXLook = first_minXLook;
                maxXLook = first_maxXLook;
                cameraContainer.localPosition = first_position;
                isThirdPersonMode = false;
            }
            else
            {
                minXLook = third_minXLook;
                maxXLook = third_maxXLook;
                cameraContainer.localPosition = third_position;
                isThirdPersonMode = true;
            }
        }
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.1f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.1f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.1f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.1f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if(Physics.Raycast(rays[i], 0.3f, groundLayerMask))
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

    public void SuperJump(float jumpForce)
    {
        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        JumpAction?.Invoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            movingPlatform = collision.gameObject.GetComponent<MovingPlatform>();
            return;
        }
    }
    
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            movingPlatform = null;
            return;
        }
    }
}
