using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;


/// A faire
// Limiter vitesse de déplacement lors des attaques
//Melee
//rifle
//Vérifier le cooldown sur les attaques
/// 
public class Player : MonoBehaviour
{
    // DECLARE REFERENCE VARIABLES
    private InputHandler _input;
    private PlayerInput playerInput;
    private CharacterController characterController;
    private Animator animator;


    //Variables to store optimized setter/getter parameter IDs
    private int isWalkingHash;
    private int isRunningHash;
    private int isJumpingHash;
    private int isAttackingHash;
    private int velocityZHash;
    private int velocityXHash;
    private int isMeleeHash;
    private int isRifleHash;
    private int isDigginHash;



    // VARIABLES TO STORE PLAYER INPUT VALUES
    private Vector2 currentMovementInput;
    private Vector3 currentMovement;
    private Vector3 currentRunMovement;
    private bool isMovementPressed;
    private bool isRunPressed;
    private bool isAttackingPressed;
    private bool isMeleePressed;
    private bool isRiflePressed;
    private bool isDigginPressed;

    //JUMPING VARIABLES
    [SerializeField] bool isJumpingPressed = false;
    private float initialJumpVelocity;
    private float maxJumpHeight = 1.0f;
    private float maxJumpTime = 0.75f;
    private bool isJumping = false;

    // VARIABLES FOR THE ATTACK
    [SerializeField] private float cooldown = 1f; //seconds
    [SerializeField] private float cooldownAR = 0.1f;
    [SerializeField] private float lastAttack = -9999f;

    // VARIABLES FOR THE GRAVITY
    private float groundedGravity = -.05f;
    private float gravity = -9.8f;

    //Acceleration et movement avec animations
    private float velocityZ = 0.0f;
    private float velocityX = 0.0f;
    [SerializeField] public float acceleration;
    [SerializeField] float deceleration;
    [SerializeField] float decelerationMax;
    [SerializeField] float decelerationMin;
    [SerializeField] private float maxVelocity = 1.0f;
    private float minMaxWalkingVelocity = 0.5f;
    private float minMaxRunVelocity = 2.0f;
    [SerializeField] float currentMaxVelocity;

    //Rotation
    [SerializeField] private bool RotateTowardMouse;

    //Movement sans animation
    [SerializeField] private float MovementSpeed;
    [SerializeField] private float RunSpeed;
    [SerializeField] private float RotationSpeed;

    //Camera
    [SerializeField]
    private Camera Camera;
    private float _smoothCoef = 0.2f;
    private Quaternion _lookAtRotation;

    

    private void Awake()
    {
        _input = GetComponent<InputHandler>();
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        //Initialise la gravité
        characterController.SimpleMove(Vector3.forward * 0);

        animator = GetComponent<Animator>();

        //S'occupe du StringToHash
        hash();
        //S'occupe des inputCallbacks du PLayerInput (Input Action Asset)
        inputCallback();

        setupJumpVariables();

    }
    //#########################################################################################
    //HASH AND SWITCH
    private void hash()
    {
        //Hash
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
        isAttackingHash = Animator.StringToHash("isAttacking");
        velocityZHash = Animator.StringToHash("Velocity Z");
        velocityXHash = Animator.StringToHash("Velocity X");
        isMeleeHash = Animator.StringToHash("isMelee");
        isRifleHash = Animator.StringToHash("isRifle");
        isDigginHash = Animator.StringToHash("isDiggin");
    }

    private void inputCallback()
    {
        // set the player input callbacks
        // Movement Input
        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;
        //Run INPUT
        playerInput.CharacterControls.Run.started += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;
        playerInput.CharacterControls.Run.performed += onRun;
        //JUMP INPUT
        playerInput.CharacterControls.Jump.started += onJump;
        playerInput.CharacterControls.Jump.canceled += onJump;
        playerInput.CharacterControls.Jump.performed += onJump;
        //ATTACK INPUT
        playerInput.CharacterControls.Attack.started += onAttack;
        playerInput.CharacterControls.Attack.canceled += onAttack;
        playerInput.CharacterControls.Attack.performed += onAttack;
        //MELEE SWITCH
        playerInput.CharacterControls.MeleeSwitch.started += onMeleeSwitch;
        playerInput.CharacterControls.MeleeSwitch.canceled += onMeleeSwitch;
        playerInput.CharacterControls.MeleeSwitch.performed += onMeleeSwitch;
        //RIFLE SWITCH
        playerInput.CharacterControls.RifleSwitch.started += onRifleSwitch;
        playerInput.CharacterControls.RifleSwitch.canceled += onRifleSwitch;
        playerInput.CharacterControls.RifleSwitch.performed += onRifleSwitch;
        //DIGGIN INPUT
        playerInput.CharacterControls.Diggin.started += onDiggin;
        playerInput.CharacterControls.Diggin.canceled += onDiggin;
        playerInput.CharacterControls.Diggin.performed += onDiggin;
    }
    //####################################################################################
    private void setupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    //###########################################################################################
    //handle all callback
    private void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    //handler function to set the player input values
    private void onMovementInput (InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        currentRunMovement.x = currentMovementInput.x * RunSpeed;
        currentRunMovement.z = currentMovementInput.y * RunSpeed;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    private void onJump(InputAction.CallbackContext context)
    {
        isJumpingPressed= context.ReadValueAsButton();
    }

    private void onAttack(InputAction.CallbackContext context)
    {
        isAttackingPressed = context.ReadValueAsButton();
    }

    private void onMeleeSwitch(InputAction.CallbackContext context)
    {
        isMeleePressed= context.ReadValueAsButton();
    }
    private void onRifleSwitch(InputAction.CallbackContext context)
    {
        isRiflePressed = context.ReadValueAsButton();
    }
    private void onDiggin(InputAction.CallbackContext context)
    {
        isDigginPressed = context.ReadValueAsButton();
    }
    //###########################################################################################

    // Update is called once per frame
    void Update()
    {
        PlayerRotation();
        handleAnimation();
        //handleRotation();
        handleRun();
        handleBlendTree();

        if (isRunPressed)
        {
            characterController.Move(currentRunMovement * Time.deltaTime);
        }
        else
        {
            characterController.Move(currentMovement * Time.deltaTime);
        }

        handleGravity();
        //Le personnage ne peut pas sauter vers l'avant et le problème est introuvable...
        //handleJump();


    }
    private void FixedUpdate()
    {
        Quaternion rotation = Quaternion.Lerp(transform.rotation, _lookAtRotation, _smoothCoef);
        transform.rotation = rotation;
    }

    //########################################################################################################################################################
    //Handle all animation

    private void handleBlendTree()
    {
        //input will be true if the player is pressing on the passed in key parameter
        // get key input from player
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool backPressed = Input.GetKey(KeyCode.S);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);

        // set current maxVelocity
        currentMaxVelocity = runPressed ? minMaxRunVelocity : minMaxWalkingVelocity;

        //handle changes in velocity
        changeVelocity(forwardPressed, leftPressed, rightPressed, backPressed, runPressed, currentMaxVelocity);
        //handle the lock or resest
        lockOrResetVelocity(forwardPressed, leftPressed, rightPressed, backPressed, runPressed, currentMaxVelocity);

        // set the parameters to our local variable values
        animator.SetFloat(velocityZHash, velocityZ);
        animator.SetFloat(velocityXHash, velocityX);

    }
    private void handleAnimation()
    {
        //get parameter values from animator
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);
        bool isJumping = animator.GetBool(isJumpingHash);
        bool isAttacking = animator.GetBool(isAttackingHash);
        bool isMelee = animator.GetBool(isMeleeHash);
        bool isRifle = animator.GetBool(isRifleHash);
        bool isDiggin = animator.GetBool(isDigginHash);

        //####
        //Mouvement
        // start walking if movement pressed is true and not already walking
        if (isMovementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }
        // start wlaking if isMovementPresssed is false and not alreadt walking
        else if (!isMovementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }
        // run if movement and run pressed are true and not currently running
        if ((isMovementPressed && isRunPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }
        // stop running if movement or run pressed are false and currently running
        else if ((!isMovementPressed || !isRunPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }
        //#####
        //Attack
        // attack if mouse left is true
        if (isAttackingPressed && !isAttacking && (isRifle == false))
        {
            if (Time.time >= lastAttack + cooldown)
            {
                animator.SetBool(isAttackingHash, true);
                lastAttack = Time.time;
            }

        }
        else if (isAttackingPressed && !isAttacking && (isRifle == true))
        {
            if (Time.time >= lastAttack + cooldownAR)
            {
                animator.SetBool(isAttackingHash, true);
                lastAttack = Time.time;
            }
        }
            // attack if mouse left is false
            if (!isAttackingPressed && isAttacking)
        {
            animator.SetBool(isAttackingHash, false);
        }
        //#####
        //Saut
        //Problème du saut toujours pas trouvé
        /*
        if (isJumpingPressed)
        {
            animator.SetBool(isJumpingHash, true);
        }
        else if (!isJumpingPressed)
        {
            animator.SetBool(isJumpingHash, false);
        }
        */
        //#####
        //Blend Tree Setter
        if ((isMelee == false) && (isRifle == false))
        {
            animator.SetBool(isMeleeHash, true);
        }
        if (isMeleePressed && (isRifle == true))
        {
            animator.SetBool(isRifleHash, false);
            animator.SetBool(isMeleeHash, true);
        }
        if (isRiflePressed && (isMelee == true))
        {
            animator.SetBool(isMeleeHash, false);
            animator.SetBool(isRifleHash, true);
        }
        //####
        //Diggin
        if(isDigginPressed && !isAttacking)
        {
            animator.SetBool(isDigginHash, true);
        }
        if (!isDigginPressed)
        {
            animator.SetBool(isDigginHash, false);
        }
    }

    //########################################################################################################################################################
    // handles reset and locking of velocity
    private void lockOrResetVelocity(bool forwardPressed, bool leftPressed, bool rightPressed, bool backPressed, bool runPressed, float currentMaxVelocity)
    {
        // handle the reset function
        handleReset(forwardPressed, leftPressed, rightPressed, backPressed);

        // handdle the lock function
        handleLock(forwardPressed, leftPressed, rightPressed, backPressed, runPressed, currentMaxVelocity);

    }
    private void handleReset(bool forwardPressed, bool leftPressed, bool rightPressed, bool backPressed)
    {
        
        // reset velocityZ
        if (!backPressed && !forwardPressed && velocityZ != 0.0f && (velocityZ > -0.05f && velocityZ < 0.05f))
        {
            velocityZ = 0.0f;
        }


        // reset velocityX
        if (!leftPressed && !rightPressed && velocityX != 0.0f && (velocityX > -0.05f && velocityX < 0.05f))
        {
            velocityX = 0.0f;
        }
    }

    private void handleLock(bool forwardPressed, bool leftPressed, bool rightPressed, bool backPressed, bool runPressed, float currentMaxVelocity)
    {
        // lock forward
        if (forwardPressed && runPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ = currentMaxVelocity;
        }
        // decelerate to the maximum walk velocity
        else if (forwardPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ -= Time.deltaTime * deceleration;
            // round to the currentMaXVelocity if within offset
            if (velocityZ > currentMaxVelocity && velocityZ < (currentMaxVelocity + 0.05f))
            {
                velocityZ = currentMaxVelocity;
            }
        }
        else if (forwardPressed && velocityZ < currentMaxVelocity && velocityZ > (currentMaxVelocity - 0.05f))
        {
            velocityZ = currentMaxVelocity;
        }



        // lock backWard
        if (backPressed && runPressed && velocityZ < -currentMaxVelocity)
        {
            velocityZ = -currentMaxVelocity;
        }
        // decelerate to the maximum walk velocity
        else if (backPressed && velocityZ < -currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * deceleration;
            // round to the currentMaXVelocity if within offset
            if (velocityZ < -currentMaxVelocity && velocityZ > (-currentMaxVelocity - 0.05f))
            {
                velocityZ = -currentMaxVelocity;
            }
        }
        else if (backPressed && velocityZ > -currentMaxVelocity && velocityZ < (-currentMaxVelocity + 0.05f))
        {
            velocityZ = -currentMaxVelocity;
        }


        // lock right
        if (rightPressed && runPressed && velocityX > currentMaxVelocity)
        {
            velocityX = currentMaxVelocity;
        }
        // decelerate to the maximum walk velocity
        else if (rightPressed && velocityX > currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * deceleration;
            // round to the currentMaXVelocity if within offset
            if (velocityX > currentMaxVelocity && velocityX < (currentMaxVelocity + 0.05f))
            {
                velocityX = currentMaxVelocity;
            }
        }
        else if (rightPressed && velocityX < currentMaxVelocity && velocityX > (currentMaxVelocity - 0.05f))
        {
            velocityX = currentMaxVelocity;
        }



        // lock left
        if (leftPressed && runPressed && velocityX < -currentMaxVelocity)
        {
            velocityX = -currentMaxVelocity;
        }
        // decelerate to the maximum walk velocity
        else if (leftPressed && velocityX < -currentMaxVelocity)
        {
            velocityX += Time.deltaTime * deceleration;
            // round to the currentMaXVelocity if within offset
            if (velocityX < -currentMaxVelocity && velocityX > (-currentMaxVelocity - 0.05f))
            {
                velocityX = -currentMaxVelocity;
            }
        }
        // round to the currentMaXVelocity if within offset
        else if (leftPressed && velocityX > -currentMaxVelocity && velocityX < (-currentMaxVelocity + 0.05f))
        {
            velocityX = -currentMaxVelocity;
        }
    }

    // handles acceleration and deceleration
    private void changeVelocity(bool forwardPressed, bool leftPressed, bool rightPressed, bool backPressed, bool runPressed, float currentMaxVelocity)
    {
        //handle acceleration function
        handleAcceleration(forwardPressed, leftPressed, rightPressed, backPressed, currentMaxVelocity);

        // handle deceleration function
        handleDeceleration(forwardPressed, leftPressed, rightPressed, backPressed, runPressed);

    }
    private void handleAcceleration(bool forwardPressed, bool leftPressed, bool rightPressed, bool backPressed, float currentMaxVelocity)
    {
        // if player presses forward, increase velocity in z direction
        if (forwardPressed && velocityZ < currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * acceleration;
            Debug.Log("W");
        }

        // increase velocity in left direction
        if (leftPressed && velocityX > -currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * acceleration;
            Debug.Log("A");
        }

        //increase velocity in right direction
        if (rightPressed && velocityX < currentMaxVelocity)
        {
            velocityX += Time.deltaTime * acceleration;
            Debug.Log("D");
        }

        //increase velocity in back direction
        if (backPressed && velocityZ > -currentMaxVelocity)
        {
            velocityZ -= Time.deltaTime * acceleration;
            Debug.Log("S");
        }
    }
    private void handleDeceleration(bool forwardPressed, bool leftPressed, bool rightPressed, bool backPressed, bool runPressed)
    {
        // set the deceleration for if run is pressed or not
        //if he walk
        if (!runPressed && (velocityZ > 0.05f || velocityX > 0.05f))
        {
            deceleration = decelerationMin;
        }
        //if he run
        if (runPressed && (velocityZ > 0.05f || velocityX > 0.05f))
        {
            deceleration = decelerationMax;
        }

        // increase velocityX if left is not pressed and velocityX < 0
        if (!leftPressed && velocityX < 0.0f)
        {
            velocityX += Time.deltaTime * deceleration;
        }

        // increase velocityZ if back is not pressed and velocityZ < 0
        if (!backPressed && velocityZ < 0.0f)
        {
            velocityZ += Time.deltaTime * deceleration;
        }

        // decrease velocityZ if forward is not pressed and velocityZ > 0
        if (!forwardPressed && velocityZ > 0.0f)
        {
            velocityZ -= Time.deltaTime * deceleration;
        }

        // decrease velocityX if right is not pressed and velocityX > 0
        if (!rightPressed && velocityX > 0.0f)
        {
            velocityX -= Time.deltaTime * deceleration;
        }
    }

    //########################################################################################################################################################
    private void handleJump()
    {
        if (!isJumping && characterController.isGrounded && isJumpingPressed)
        {
            isJumping = true;
            currentMovement.y = initialJumpVelocity * 0.5f;
            currentRunMovement.y = initialJumpVelocity * 0.5f;
        }
        else if (!isJumpingPressed && isJumping && characterController.isGrounded) { 
            isJumping = false;
        }
    }

    private void handleGravity()
    {
        bool isFalling = currentMovement.y <= 0.0f || !isJumpingPressed;
        float fallMultiplier = 2.0f;
        //Apply proper gravity depending on if the character is grounded or not
        if (characterController.isGrounded)
        {
            currentMovement.y = groundedGravity;
            currentRunMovement.y = groundedGravity;
        }
        else if (isFalling) {
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (gravity * fallMultiplier * Time.deltaTime);
            float nextYVelocity = Mathf.Max((previousYVelocity + newYVelocity) * 0.5f, -20.0f);
            currentMovement.y = nextYVelocity;
            currentRunMovement.y = nextYVelocity;
        } 
        else
        {
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (gravity * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
            currentMovement.y = nextYVelocity;
            currentRunMovement.y = nextYVelocity;
        }
    }

    private void PlayerRotation()
    {
        Plane groundPlane = new Plane(Vector3.up, -transform.position.y);
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitDistance;

        if (groundPlane.Raycast(mouseRay, out hitDistance))
        {
            Vector3 lookAtPostion = mouseRay.GetPoint(hitDistance);
            _lookAtRotation = Quaternion.LookRotation(lookAtPostion - transform.position, Vector3.up);
        }

    }
    
    private void handleRun()
    {
        if(isRunPressed)
        {
            characterController.Move(currentRunMovement* Time.deltaTime);
        } else
        {
            characterController.Move(currentMovement * Time.deltaTime);
        }
    }
    private void handleRotation()
    {
        var targetVector = new Vector3(_input.InputVector.x, 0, _input.InputVector.y);
        var movementVector = MoveTowardTarget(targetVector);

        if (!RotateTowardMouse)
        {
            RotateTowardMovementVector(movementVector);
        }
        if (RotateTowardMouse)
        {
            RotateFromMouseVector();
        }
    }
    private void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }

    private void RotateFromMouseVector()
    {
        Ray ray = Camera.ScreenPointToRay(_input.MousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f))
        {
            var target = hitInfo.point;
            target.y = transform.position.y;
            transform.LookAt(target);
        }
    }

    private Vector3 MoveTowardTarget(Vector3 targetVector)
    {
        var speed = MovementSpeed * Time.deltaTime;
        // transform.Translate(targetVector * (MovementSpeed * Time.deltaTime)); Demonstrate why this doesn't work
        //transform.Translate(targetVector * (MovementSpeed * Time.deltaTime), Camera.gameObject.transform);

        targetVector = Quaternion.Euler(0, Camera.gameObject.transform.rotation.eulerAngles.y, 0) * targetVector;
        var targetPosition = transform.position + targetVector * speed;
        transform.position = targetPosition;
        return targetVector;
    }

    private void RotateTowardMovementVector(Vector3 movementDirection)
    {
        if (movementDirection.magnitude == 0) { return; }
        var rotation = Quaternion.LookRotation(movementDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, RotationSpeed);
    }
}
