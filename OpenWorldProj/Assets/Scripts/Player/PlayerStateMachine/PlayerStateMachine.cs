using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] CharacterController _characterController;
    PlayerController inputActions;
    Animator _anim;

    int isWalkingHash;
    int isRunningHash;

    float _horizontalInput, _verticalInput;
    Vector3 _playerInput;
    Vector2 currentMovementInput;
    Vector3 _currentMovement;
    Vector3 currentRunMovement;
    Vector3 _appliedMovement;
    bool isMovementPressed;
    bool isRunningPressed;


    //Constants
    float rotationFactorPerFrame = 15f;
    public float runFactor;
    int zero = 0;
    float gravity = -9.81f;
    float _groundedGravity = -0.5f;

    // Jumping Variables
    bool _isJumpedPressed = false;
    float initialJumpVelocity;
    public float maxJumpHeight = 4f;
    public float maxJumpTime = 0.75f;
    bool _isJumping = false;
    int _isJumpingHash;
    int _jumpCountHash;
    bool _requireNewJumpPress = false;
    int _jumpCount = 0;
    Dictionary<int, float> _initialJumpVelocities = new Dictionary<int, float>();
    Dictionary<int, float> _jumpGravities = new Dictionary<int, float>();
    Coroutine _currentJumpResetRoutine = null;


    // Camera Relative Movement
    Vector3 forward;
    Vector3 right;
    Vector3 cameraRelativeMovement;

    // State Variables
    PlayerBaseState _currentState;
    PlayerStateFactory _states;

    // getters and setters
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public CharacterController CharacterController { get { return _characterController; } }
    public Animator Animator { get { return _anim; } }
    public Coroutine CurrentJumpResetRoutine { get { return _currentJumpResetRoutine; } set { _currentJumpResetRoutine = value; } }
    public Dictionary<int, float> InitialJumpVelocites { get { return _initialJumpVelocities; } }
    public Dictionary<int, float> JumpGravities { get { return _jumpGravities; } }

    public int JumpCount { get { return _jumpCount; } set { _jumpCount = value; } }
    public int IsJumpingHash { get { return _isJumpingHash; } }
    public int JumpCountHash { get { return _jumpCountHash; } }
    public bool RequireNewJumpPress { get { return _requireNewJumpPress; } set { _requireNewJumpPress = value; } }
    public bool IsJumping { set { _isJumping = value; } }
    public bool IsJumpedPressed {get { return _isJumpedPressed; } } 
    public float GroundedGravity { get { return _groundedGravity; } }
    public float CurrentMovementY { get { return _currentMovement.y; } set { _currentMovement.y = value; } }
    public float AppliedMovementY { get { return _appliedMovement.y; } set { _appliedMovement.y = value; } }




    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();
        inputActions = new PlayerController();
        
        // Set Up State
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        _isJumpingHash = Animator.StringToHash("isJumping");
        _jumpCountHash = Animator.StringToHash("jumpCount");

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
        _currentMovement.x = currentMovementInput.x * 2;
        _currentMovement.z = currentMovementInput.y * 2;
        currentRunMovement.x = currentMovementInput.x * runFactor;
        currentRunMovement.z = currentMovementInput.y * runFactor;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;

    }

    void OnRun(InputAction.CallbackContext context)
    {
        isRunningPressed = context.ReadValueAsButton();
    }

    void OnJump(InputAction.CallbackContext context)
    {
        _isJumpedPressed = context.ReadValueAsButton();
        _requireNewJumpPress = false;
    }

    void SetUpJumpVariables()
    {
        // Original Jump
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;

        // Second Jump
        float secondJumpGravity = (-2 * (maxJumpHeight + 2)) / Mathf.Pow(timeToApex * 1.25f, 2);
        float secondJumpInitialVelocity = (2 * (maxJumpHeight + 2)) / (timeToApex * 1.25f);

        // Third Jump
        float thirdJumpGravity = (-2 * (maxJumpHeight + 4)) / Mathf.Pow(timeToApex * 1.5f, 2);
        float thirdJumpInitialVelocity = (2 * (maxJumpHeight + 4)) / (timeToApex * 1.5f);

        _initialJumpVelocities.Add(1, initialJumpVelocity);
        _initialJumpVelocities.Add(2, secondJumpInitialVelocity);
        _initialJumpVelocities.Add(3, thirdJumpInitialVelocity);

        _jumpGravities.Add(0, gravity);
        _jumpGravities.Add(1, gravity);
        _jumpGravities.Add(2, secondJumpGravity);
        _jumpGravities.Add(3, thirdJumpGravity);

    }
    void HandleRotation()
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = cameraRelativeMovement.x;
        positionToLookAt.y = zero;
        positionToLookAt.z = cameraRelativeMovement.z;
        Quaternion currentRotation = transform.rotation;
        if (isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }

    }

    void HandleCameraRelativeMovement(Vector3 moveVector)
    {
        forward = Camera.main.transform.forward;
        right = Camera.main.transform.right;
        forward.y = 0f;
        right.y = 0f;
        forward = forward.normalized;
        right = right.normalized;

        Vector3 forwardRelativeMovement = moveVector.z * forward;
        Vector3 rightRelativeMovement = moveVector.x * right;
        cameraRelativeMovement = forwardRelativeMovement + rightRelativeMovement;

        cameraRelativeMovement.y = moveVector.y;
        _characterController.Move(cameraRelativeMovement * Time.deltaTime);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (isRunningPressed)
        //{
        //    appliedMovement.x = currentRunMovement.x;
        //    appliedMovement.z = currentRunMovement.z;
        //}
        //else
        //{
        //    appliedMovement.x = currentMovement.x;
        //    appliedMovement.z = currentMovement.z;
        //}
        HandleRotation();
        _currentState.UpdateState();
        HandleCameraRelativeMovement(_appliedMovement);



    }

    private void OnEnable()
    {
        inputActions.Movement.Enable();
    }

    private void OnDisable()
    {
        inputActions.Movement.Disable();

    }
}
