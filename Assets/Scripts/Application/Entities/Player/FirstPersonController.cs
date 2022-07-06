using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    public PlayerInput playerInput;
    public Camera playerCamera;
    public Player player;
    public CharacterController characterController;

    public float jumpPower = 10f;
    public float gravity = 2.0f;
    public float crouchSpeedReduction = 0.5f;
    public float crouchHeight = 0.75f;
    public float minLookY = -60f;
    public float maxLookY = 60f;
    public float cameraSensitivity = 1f;
    public MovementState movementState = MovementState.Idle;

    [HideInInspector]
    public bool canMove = true;
    [HideInInspector]
    public Vector3 moveDirection = Vector3.zero;
    [HideInInspector]
    public Vector3 lookDirection = Vector3.zero;
    [HideInInspector]
    public Vector3 originalScale;
    [HideInInspector]
    public bool isCrouching = false;
    [HideInInspector]
    public Vector2 look;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        originalScale = transform.localScale;

        playerInput = GetComponent<PlayerInput>();
        playerInput.currentActionMap.FindAction("Jump").performed += Jump;
        playerInput.currentActionMap.FindAction("Crouch").performed += Crouch;
        playerInput.currentActionMap.FindAction("MovementHorizontal").performed += e => UpdateMoveHorizontal(e);
        playerInput.currentActionMap.FindAction("MovementHorizontal").canceled += e => moveDirection.x = 0;
        playerInput.currentActionMap.FindAction("MovementVertical").performed += e => UpdateMoveVertical(e);
        playerInput.currentActionMap.FindAction("MovementVertical").canceled += e => moveDirection.z = 0;
        playerInput.currentActionMap.FindAction("Look").performed += e => UpdateLook(e);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }

    void Update()
    {
        Look();
        Move();
    }


    public void Jump(CallbackContext context)
    {
        if (!characterController.isGrounded || !canMove)
        {
            return;
        }

        if (isCrouching)
        {
            Crouch(context);
        }

        moveDirection.y = jumpPower;
    }

    public void Crouch(CallbackContext context)
    {
        if (!characterController.isGrounded)
        {
            return;
        }

        if (isCrouching is false)
        {
            transform.localScale = new Vector3(originalScale.x, crouchHeight, originalScale.z);
            isCrouching = true;
        }
        else
        {
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
            isCrouching = false;
        }
    }

    private void UpdateMovementState()
    {
        var running = Keyboard.current.leftShiftKey.isPressed;

        if (running)
        {
            movementState = MovementState.Running;
        }
        else if (moveDirection == Vector3.zero)
        {
            movementState = MovementState.Idle;
        }
        else if (!characterController.isGrounded)
        {
            movementState = MovementState.Jumping;
        }
        else
        {
            movementState = MovementState.Walking;
        }
    }

    public void Look()
    {
        look.y = Mathf.Clamp(look.y, minLookY, maxLookY);

        var smoothedLook = look;

        transform.eulerAngles = new Vector3(smoothedLook.y, smoothedLook.x, 0);
        playerCamera.transform.eulerAngles = new Vector3(smoothedLook.y, smoothedLook.x, 0);
    }

    private void UpdateLook(CallbackContext context)
    {
        look.x += context.ReadValue<Vector2>().x;
        look.y -= context.ReadValue<Vector2>().y;
    }

    public void Move()
    {
        UpdateMovementState();

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
            characterController.Move(moveDirection * Time.deltaTime);
        } else
        {
            var forward = playerCamera.transform.forward;
            var right = playerCamera.transform.right;

            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();
            
            var desiredMoveDirection = forward * moveDirection.z + right * moveDirection.x;
            desiredMoveDirection.y = moveDirection.y;

            var modifier = movementState.GetMovementModifier() * (isCrouching ? crouchSpeedReduction : 1);
            characterController.Move(5 * modifier * Time.deltaTime * desiredMoveDirection);
        }
    }

    private void UpdateMoveVertical(CallbackContext context)
    {
        moveDirection.z = context.ReadValue<Vector2>().y;
    }

    private void UpdateMoveHorizontal(CallbackContext context)
    {
        moveDirection.x = context.ReadValue<Vector2>().x;
    }
}