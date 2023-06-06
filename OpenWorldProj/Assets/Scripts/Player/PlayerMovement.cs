using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    // Components
    [SerializeField] CharacterController _characterController;
    [SerializeField] public PlayerController inputActions;
    [SerializeField] Animator _anim;
    [SerializeField] ObjectGen objectGen;
    [SerializeField] PlayerCombat combat;
    [SerializeField] PlayerManager playerManager;
    public CinemachineVirtualCamera followCam, aimCam;

    // Animation Hashes
    int isWalkingHash;
    int isRunningHash;
    int isJumpingHash;
    int jumpCountHash;
    int isShootingHash;
    int isAimingHash;

    // Movement Variables
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    Vector3 appliedMovement;
    bool isMovementPressed;
    bool isRunningPressed;
    bool runToggle;

    // Constants
    float rotationFactorPerFrame = 15f;
    public float runFactor; // Multiplier for running speed
    public float walkFactor; // Multiplier for walking speed
    int zero = 0;
    float gravity = -9.81f;
    float groundedGravity = -0.5f;

    // Jumping Variables
    bool isJumpedPressed = false;
    float initialJumpVelocity;
    public float maxJumpHeight = 4f;
    public float maxJumpTime = 0.75f;
    bool isJumping = false;
    bool isJumpAnimPlaying = false;
    int jumpCount = 0;
    Dictionary<int, float> initialJumpVelocities = new Dictionary<int, float>(); // Initial jump velocities for double jump
    Dictionary<int, float> jumpGravities = new Dictionary<int, float>(); // Gravities for double jump
    Coroutine currentJumpResetRoutine = null;

    // Camera Relative Movement
    private Transform cameraTransform;
    Vector3 forward;
    Vector3 right;
    Vector3 cameraRelativeMovement;

    // Attacking
    bool isAttackPressed = false;
    public bool isAttackPerforming = false;
    bool canAttack = true;

    // Aiming
    bool isAimingPressed = false;
    bool isAiming = false;
    Vector3 centerOfScreen;

    // Arrow - To remove later
    public GameObject arrowPrefab;
    public Transform spawnPoint;

    // 3D Models
    public GameObject weapon, sling, weaponDes, slingDes;

    // Healing
    bool isHealPressed;
    bool isHealing;

    //Inventario
    public Inventory inventory;
    public GameObject hand;
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();
        inputActions = new PlayerController();
        cameraTransform = Camera.main.transform;

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
        jumpCountHash = Animator.StringToHash("jumpCount");
        isAimingHash = Animator.StringToHash("isAiming");
        isShootingHash = Animator.StringToHash("isShooting");

        inputActions.Movement.Move.started += OnMovement;
        inputActions.Movement.Move.canceled += OnMovement;
        inputActions.Movement.Move.performed += OnMovement;
        inputActions.Movement.Run.started += OnRun;
        inputActions.Movement.Run.canceled += OnRun;
        inputActions.Movement.Jump.started += OnJump;
        inputActions.Movement.Jump.canceled += OnJump;
        inputActions.Movement.Attack.started += OnAttack;
        inputActions.Movement.Attack.canceled += OnAttack;
        inputActions.Movement.Aim.started += OnAim;
        inputActions.Movement.Aim.canceled += OnAim;
        inputActions.Movement.Heal.started += OnHeal;
        inputActions.Movement.Heal.canceled += OnHeal;


        SetUpJumpVariables();
    }

    void OnMovement(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x * walkFactor;
        currentMovement.z = currentMovementInput.y * walkFactor;
        currentRunMovement.x = currentMovementInput.x * runFactor;
        currentRunMovement.z = currentMovementInput.y * runFactor;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;

    }

    void OnRun(InputAction.CallbackContext context)
    {
        isRunningPressed = context.ReadValueAsButton();
        if(isRunningPressed)
        {
            runToggle = !runToggle;
        }
    }

    void OnJump(InputAction.CallbackContext context)
    {
        isJumpedPressed = context.ReadValueAsButton();
    }

    void OnAttack(InputAction.CallbackContext context)
    {
        isAttackPressed = context.ReadValueAsButton();
    }

    void OnAim(InputAction.CallbackContext context)
    {
        isAimingPressed = context.ReadValueAsButton();
    }
    void OnHeal(InputAction.CallbackContext context)
    {
        isHealPressed= context.ReadValueAsButton();
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

    void HandleWalkRunAnimation()
    {
        bool isWalking = _anim.GetBool(isWalkingHash);
        bool isRunning = _anim.GetBool(isRunningHash);

        if(isMovementPressed && !isWalking)
        {
            _anim.SetBool(isWalkingHash, true);
        }
        else if (!isMovementPressed && isWalking)
        {
            _anim.SetBool(isWalkingHash, false);
            _anim.SetBool(isRunningHash, false);
            runToggle = false;
        }

        if(isMovementPressed && runToggle)
        {
            _anim.SetBool(isRunningHash, true);
        }
        else if((!isMovementPressed && runToggle) || (isMovementPressed && !runToggle))
        {
            _anim.SetBool(isRunningHash, false);
            runToggle = false;
        }
        //if ((isMovementPressed && isRunningPressed) && !isRunning)
        //{
        //}
        //else if ((isMovementPressed && !isRunningPressed) && isRunning)
        //{
        //    _anim.SetBool(isRunningHash, false);
        //}
        //else if((!isMovementPressed && isRunningPressed) && isRunning)
        //{
        //    _anim.SetBool(isRunningHash, false);
        //}
        //else if((!isMovementPressed && !isRunningPressed) && isRunning)
        //{
        //    _anim.SetBool(isRunningHash, false);
        //}
    }

    void HandleRotation()
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = cameraRelativeMovement.x;
        positionToLookAt.y = zero;
        positionToLookAt.z = cameraRelativeMovement.z;
        Quaternion currentRotation = transform.rotation;
        if(isMovementPressed && !isAiming)
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
            appliedMovement.y = groundedGravity;
        }
        else if(isFalling)
        {
            float previousYVelocity = currentMovement.y;
            currentMovement.y = currentMovement.y + (jumpGravities[jumpCount] * fallMultiplier * Time.deltaTime);
            appliedMovement.y = Mathf.Max((previousYVelocity + currentMovement.y) * 0.5f, -20f);
        }
        else
        {
            float previousYVelocity = currentMovement.y;
            currentMovement.y = currentMovement.y + (jumpGravities[jumpCount] * Time.deltaTime);
            appliedMovement.y = (previousYVelocity + currentMovement.y) * 0.5f;
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
            currentMovement.y = initialJumpVelocities[jumpCount];
            appliedMovement.y = initialJumpVelocities[jumpCount];
        }
        else if (!isJumpedPressed && isJumping && _characterController.isGrounded)
        {
            isJumping = false;
        }
    }

    void HandleAttack()
    {
        if(isAttackPressed)
        {
            isAttackPressed = false;
            if(isAiming)
            {
                _anim.SetBool(isShootingHash, true);
                return;
            }
            else
            {
                combat.Attack();
            }
            
        }
        
    }

    void HandleAim()
    {
        if(isAimingPressed && _characterController.isGrounded)
        {
            
            isAiming = true;
            runToggle = false;
            sling.SetActive(true);
            //slingDes.SetActive(false);
            if(inputActions.Movement.Attack.enabled)
            {
                weapon.SetActive(false);
                weaponDes.SetActive(true);
            }

            _anim.SetBool(isRunningHash, false);
            _anim.SetBool(isAimingHash, true);
            followCam.gameObject.SetActive(false);
            aimCam.gameObject.SetActive(true);
            Quaternion targetRotation = Quaternion.LookRotation(cameraTransform.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
        else
        {
            if(isAiming)
            {
                transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, transform.eulerAngles.z);
            }
            sling.SetActive(false);
            //slingDes.SetActive(true);
            if(inputActions.Movement.Attack.enabled)
            {
                weapon.SetActive(true);
                weaponDes.SetActive(false);
            }
            isAiming = false;
            _anim.SetBool(isAimingHash, false);
            _anim.SetBool(isShootingHash, false);
            followCam.gameObject.SetActive(true);
            aimCam.gameObject.SetActive(false);
        }
    }

    public void Shoot()
    {
        //GameObject arrow = Instantiate(arrowPrefab, spawnPoint.position, Quaternion.identity);
        //arrow.GetComponent<Rigidbody>().AddForce(transform.forward * 25f, ForceMode.Impulse);
        RaycastHit hit;
        if(Physics.Raycast(cameraTransform.position, centerOfScreen, out hit, 15f ))
        {
            objectGen.OnShoot(spawnPoint.position, hit.point, true);
        }
        else
        {
            objectGen.OnShoot(spawnPoint.position, (aimCam.transform.forward * 100f), false);
        }
        canAttack = false;
        _anim.SetBool(isShootingHash, false);
        StartCoroutine(AttackCooldown());
        Debug.Log("Shoot");
    }

    public void HandleHeal()
    {
        if(isHealPressed && !isHealing)
        {
            isHealing = true;

        }
    }

    IEnumerator JumpResetRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        jumpCount = 0;
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        canAttack = true;
    }
    private void OnEnable()
    {
        inputActions.Movement.Enable();
    }

    private void OnDisable()
    {
        inputActions.Movement.Disable();

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
        if(_characterController.enabled && !isAttackPerforming)
            _characterController.Move(cameraRelativeMovement * Time.deltaTime);
    }

    private void Start()
    {
        weaponDes.SetActive(false);
        sling.SetActive(false);
        centerOfScreen = new Vector3(Screen.width / 2, Screen.height / 2, zero);
        inventory.ItemUsed += Inventory_ItemUsed;
    }

    private void Inventory_ItemUsed(object sender, InventoryEventsArgs e)
    {
        IInventoryItem item = e.Item;
        GameObject goItem = (item as MonoBehaviour).gameObject;
        goItem.SetActive(true);

        goItem.transform.parent = hand.transform;
        goItem.transform.position = hand.transform.position;

    }

    private void Update()
    {
        if (runToggle)
        {
            appliedMovement.x = currentRunMovement.x;
            appliedMovement.z = currentRunMovement.z;
        }
        else
        {
            appliedMovement.x = currentMovement.x;
            appliedMovement.z = currentMovement.z;
        }
        HandleCameraRelativeMovement(appliedMovement);
        //transform.forward = new Vector3(cameraRelativeMovement.x, 0f, cameraRelativeMovement.z);
        HandleWalkRunAnimation();
        HandleRotation();
        HandleGravity();
        HandleJump();
        HandleAim();
        HandleAttack();
    }

    public void DisableController()
    {
        _characterController.enabled = false;
    }
    public void EnableController()
    {
        _characterController.enabled = true;

    }
    public void SetAttackPerformingTrue()
    {
        isAttackPerforming = true;
    }
    public void SetAttackPerformingFalse()
    {
        isAttackPerforming = false;
    }
    private void OnApplicationFocus(bool focus)
    {
        if(focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
   
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        IInventoryItem item = hit.collider.GetComponent<IInventoryItem>();
        if(item != null)
        {
            inventory.AddItem(item);
        }
    }
}
