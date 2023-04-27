using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] CharacterController _characterController;
    PlayerController inputActions;
    Animator _anim;

    int isWalkingHash;
    int isRunningHash;

    float _horizontalInput, _verticalInput;
    Vector3 _playerInput;
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovemet;
    bool isMovementPressed;
    bool isRunningPressed;


    //Constants
    float rotationFactorPerFrame = 15f;
    public float runFactor;
    int zero = 0;
    float gravity = -9.81f;
    float groundedGravity = -0.5f;

    // Jumping Variables
    bool isJumpedPressed = false;
    float initialJumpVelocity;
    public float maxJumpHeight = 4f;
    public float maxJumpTime = 0.75f;
    bool isJumping = false;
    int isJumpingHash;
    int jumpCountHash;
    bool isJumpAnimPlaying = false;
    int jumpCount = 0;
    Dictionary<int, float> initialJumpVelocities = new Dictionary<int, float>();
    Dictionary<int, float> jumpGravities = new Dictionary<int, float>();
    Coroutine currentJumpResetRoutine = null;


    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();
        inputActions = new PlayerController();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
        jumpCountHash = Animator.StringToHash("jumpCount");

        inputActions.Movement.Move.started += OnMovement;
        inputActions.Movement.Move.canceled += OnMovement;
        inputActions.Movement.Move.performed += OnMovement;
        inputActions.Movement.Run.started += OnRun;
        inputActions.Movement.Run.canceled += OnRun;
        inputActions.Movement.Jump.started += OnJump;
        inputActions.Movement.Jump.canceled += OnJump;

        SetUpJumpVariables();
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

    void OnJump(InputAction.CallbackContext context)
    {
        isJumpedPressed = context.ReadValueAsButton();
    }

    void SetUpJumpVariables()
    {
        // Original Jump
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;

        // Second Jump
        float secondJumpGravity = (-2 * (maxJumpHeight + 2)) / Mathf.Pow(timeToApex * 1.25f, 2);
        float secondJumpInitialVelocity = (2 * (maxJumpHeight + 2)) / (timeToApex*1.25f);

        // Third Jump
        float thirdJumpGravity = (-2 * (maxJumpHeight + 4)) / Mathf.Pow(timeToApex * 1.5f, 2);
        float thirdJumpInitialVelocity = (2 * (maxJumpHeight + 4)) / (timeToApex * 1.5f);

        initialJumpVelocities.Add(1, initialJumpVelocity);
        initialJumpVelocities.Add(2, secondJumpInitialVelocity);
        initialJumpVelocities.Add(3, thirdJumpInitialVelocity);

        jumpGravities.Add(0, gravity);
        jumpGravities.Add(1, gravity);
        jumpGravities.Add(2, secondJumpGravity);
        jumpGravities.Add(3, thirdJumpGravity);

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
        else if ((isMovementPressed && !isRunningPressed) && isRunning)
        {
            _anim.SetBool("isRunning", false);
        }
        else if((!isMovementPressed && isRunningPressed) && isRunning)
        {
            _anim.SetBool("isRunning", false);
        }
    }

    void HandleRotation()
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = zero;
        positionToLookAt.z = currentMovement.z;
        Quaternion currentRotation = transform.rotation;
        if(isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }

    }
    
    void HandleGravity()
    {
        bool isFalling = currentMovement.y <= 0.0f || !isJumpedPressed;
        float fallMultiplier = 2f;
        if(_characterController.isGrounded)
        {
            if(isJumpAnimPlaying)
            {
                _anim.SetBool(isJumpingHash, false);
                isJumpAnimPlaying = false;
                currentJumpResetRoutine = StartCoroutine(JumpResetRoutine());
                if(jumpCount == 3)
                {
                    jumpCount = 0;
                    _anim.SetInteger(jumpCountHash, jumpCount);
                }
            }
            currentMovement.y = groundedGravity;
            currentRunMovemet.y = groundedGravity;
        }
        else if(isFalling)
        {
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (jumpGravities[jumpCount] * fallMultiplier * Time.deltaTime);
            float nextYVelocity = Mathf.Max((previousYVelocity + newYVelocity) * 0.5f, -20f);
            currentMovement.y = nextYVelocity;
            currentRunMovemet.y = nextYVelocity;
        }
        else
        {
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (jumpGravities[jumpCount] * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
            currentMovement.y = nextYVelocity;
            currentRunMovemet.y = nextYVelocity;
        }
    }

    void HandleJump()
    {
        if (!isJumping && _characterController.isGrounded && isJumpedPressed)
        {
            if(jumpCount < 3 && currentJumpResetRoutine != null)
            {
                StopCoroutine(currentJumpResetRoutine);
            }
            isJumping = true;
            _anim.SetBool(isJumpingHash, true);
            isJumpAnimPlaying = true;
            jumpCount += 1;
            _anim.SetInteger(jumpCountHash, jumpCount);
            currentMovement.y = initialJumpVelocities[jumpCount] * 0.5f;
            currentRunMovemet.y = initialJumpVelocities[jumpCount] * 0.5f;
        }
        else if (!isJumpedPressed && isJumping && _characterController.isGrounded)
        {
            isJumping = false;
        }
    }

    IEnumerator JumpResetRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        jumpCount = 0;
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
        HandleGravity();
        HandleJump();
    }

}
