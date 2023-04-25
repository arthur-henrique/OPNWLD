using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    float _horizontalInput, _verticalInput;
    Vector3 _playerInput;
    [SerializeField] CharacterController _characterController;
    PlayerController inputActions;
    Animator _anim;
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovemet;
    bool isMovementPressed;
    bool isRunningPressed;
    float rotationFactorPerFrame = 15f;

    public float runFactor;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();
        inputActions = new PlayerController();

        inputActions.Movement.Move.started += OnMovement;
        inputActions.Movement.Move.canceled += OnMovement;
        inputActions.Movement.Move.performed += OnMovement;
        inputActions.Movement.Run.started += OnRun;
        inputActions.Movement.Run.canceled += OnRun;
    }

    void OnMovement(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        currentRunMovemet.x = currentMovementInput.x * runFactor;
        currentRunMovemet.z = currentMovementInput.y * runFactor;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    void OnRun(InputAction.CallbackContext context)
    {
        isRunningPressed = context.ReadValueAsButton();
    }

    void HandleAnimation()
    {
        bool isWalking = _anim.GetBool("isWalking");
        bool isRunning = _anim.GetBool("isRunning");

        if(isMovementPressed && !isWalking)
        {
            _anim.SetBool("isWalking", true);
        }

        else if (!isMovementPressed && isWalking)
        {
            _anim.SetBool("isWalking", false);
        }

        if((isMovementPressed && isRunningPressed) && !isRunning)
        {
            _anim.SetBool("isRunning", true);
        }
        else if((!isMovementPressed && !isRunningPressed) && isRunning)
        {
            _anim.SetBool("isRunning", false);

        }
    }

    void HandleRotation()
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;
        Quaternion currentRotation = transform.rotation;
        if(isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }

    }
    private void OnEnable()
    {
        inputActions.Movement.Enable();
    }

    private void OnDisable()
    {
        inputActions.Movement.Disable();

    }

    private void Update()
    {
        if(isRunningPressed)
        {
            _characterController.Move(currentRunMovemet * Time.deltaTime);
        }
        else
        {
            _characterController.Move(currentMovement * Time.deltaTime);
        }
        HandleAnimation();
        HandleRotation();
    }

}
