using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;


/// A faire
// Limiter vitesse de d�placement lors des attaques (Abandonn�)
//Melee (Fait)
//rifle (Fait)
//V�rifier le cooldown sur les attaques (Fait ?) 
/// 

/************************************************************************************************************************
Cr�ateur g�n�ral du script Player : Derek
L'utilisation de nombreux tutoriel � men� � la cr�ation de ce script
************************************************************************************************************************/
public class Player : MonoBehaviour
{

    // DECLARE REFERENCE VARIABLES
    private InputHandler _input; //Premi�re m�thode d'obtention des inputs du player
    private PlayerInput playerInput; //S�connde m�thode
    private CharacterController characterController; //Coeur du personnage
    private Animator animator; //Coeur des animations
    private RigBuilder rigBuilder; //Utilis� pour le inverse Kinematic lors de l'utilisation des armes � feu


    //Variables to store optimized setter/getter parameter IDs
    //Le ashing n'a pas �t� respect� tout au long du projet, par cause de manque de temps
    //L'utilisation du ashing �tait une nouvelle fonctionnalit� que j'ai appris et pas encore maitris� � 100%
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
    private bool isMeleePressed; //Obsol�te depuis l'impl�mentation de l'inventaire
    private bool isRiflePressed; //Idem
    private bool isDigginPressed;

    //JUMPING VARIABLES
    //OBSOL�TE D� AU BUG NON R�SOLU
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

    //Arme
    [SerializeField] private GameObject m4;
    [SerializeField] private GameObject Pelle;
    [SerializeField] private GameObject BaseballBat;

    //Vie
    [SerializeField] private int _maxHealth = 100;
    private int _health;
    private UI_Manager _uiManager;

    ItemData item;
    [SerializeField] Inventaire inventaire;



/************************************************************************************************************************
Cr�ateur : Derek
Fonction Awake qui va chercher nombreux parametre utilis� tout au long du programme
                                
                                                   ***  AWAKE ***
                   
************************************************************************************************************************/
    private void Awake()
    {
        //Input du joueur
        _input = GetComponent<InputHandler>();
        playerInput = new PlayerInput();

        characterController = GetComponent<CharacterController>();

        //IK
        rigBuilder = GetComponent<RigBuilder>();

        //UI
        _uiManager = FindObjectOfType<UI_Manager>().GetComponent<UI_Manager>();

        //L'initialisation de la vie ainsi permet de garder en m�moire la vie maximale plut�t que d'utiliser une seule variable
        _health = _maxHealth;

        //Initialise la gravit� de base
        //OBSOL�TE car impl�mentation manuelle de la gravit�
        //characterController.SimpleMove(Vector3.forward * 0); 
        

        //Coeur de l'animation
        animator = GetComponent<Animator>();

        //S'occupe du StringToHash
        hash();
        //S'occupe des inputCallbacks du PLayerInput (Input Action Asset)
        inputCallback();

        setupJumpVariables();
    }





/************************************************************************************************************************
Cr�ateur : Derek
Fonction hash est utilis� pour convertir les string en hash afin d'optimiser le code

                                               *** HASH AND SWITCH ***

************************************************************************************************************************/
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



/************************************************************************************************************************
Cr�ateur : Derek
Fonction hash est utilis� pour convertir les string en hash afin d'optimiser le code

                                                    *** INPUT ***

************************************************************************************************************************/
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

    private void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }

    /************************************************************************************************************************
    Cr�ateur : Derek
    CODE OBSOL�TE
    Ce code est laiss� dans le programme pour laisser des tr�ce de mon travail et aussi dans l'espoir de trouver le probl�me
    un jour.

                                                        *** JUMP ***

    ************************************************************************************************************************/
    private void setupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }




/************************************************************************************************************************
Cr�ateur : Derek

                                                     *** Callbacks ***

************************************************************************************************************************/
    

    //handler function to set the player input values
    //Input des mouvements
    private void onMovementInput (InputAction.CallbackContext context)
    {
        //Utilisation de context.ReadValue qui est un peu de la triche haha
        currentMovementInput = context.ReadValue<Vector2>();
        //x
        currentMovement.x = currentMovementInput.x;
        currentRunMovement.x = currentMovementInput.x * RunSpeed;
        //y
        currentMovement.z = currentMovementInput.y;
        currentRunMovement.z = currentMovementInput.y * RunSpeed;
        //Global
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    //Inputs de la course
    private void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    //Inputs du jump
    //OBSOL�TE
    private void onJump(InputAction.CallbackContext context)
    {
        isJumpingPressed= context.ReadValueAsButton();
    }

    //Attaque de melee
    private void onAttack(InputAction.CallbackContext context)
    {
        isAttackingPressed = context.ReadValueAsButton();
    }

    
    //Changement d'arme
    //Obsol�te d� � l'impl�mentation de l'inventaire
    private void onMeleeSwitch(InputAction.CallbackContext context)
    {
        isMeleePressed= context.ReadValueAsButton();
    }
    //Changement d'arme
    //Obsol�te d� � l'impl�mentation de l'inventaire
    private void onRifleSwitch(InputAction.CallbackContext context)
    {
        isRiflePressed = context.ReadValueAsButton();
    }
    

    //Utilisation de la pelle pour pelleter
    //J'ai cr�� c'ette partie du pelletage mais F�lix est l'auteur du reste du concept
    private void onDiggin(InputAction.CallbackContext context)
    {
        isDigginPressed = context.ReadValueAsButton();
    }
   


/************************************************************************************************************************
Cr�ateur : Derek
Coeur du fonctionnement du personnage
                                                   *** UPDATES ***

************************************************************************************************************************/
    // Update is called once per frame
    void Update()
    {

        //Script cr�� par Yoan qui permet au joueur de se tourner vers la souris
        PlayerRotation();

        //Animations du personnage
        handleAnimation();

        //handleRotation(); //Version obsol�te par l'existance de la version de Yoan

        //Course du personnage
        handleRun();

        //Changement du blend tree lorsqu'un changement occure
        handleBlendTree();

        //Gestion de la gravit� du personnage
        handleGravity();

        //Gestion du saut du personnage
        //Le personnage ne peut pas sauter vers l'avant et le probl�me est introuvable...
        //handleJump(); //Obsol�te mais reste une tr�ce de mon travail



    }


/************************************************************************************************************************
Cr�ateur : Yoan
Gestion de certaine fonctionnalit� de la cam�ra et de la souris

                                                  *** FIXEDUPDATES ***

************************************************************************************************************************/
    private void FixedUpdate()
    {
        Quaternion rotation = Quaternion.Lerp(transform.rotation, _lookAtRotation, _smoothCoef);
        transform.rotation = rotation;
    }





/************************************************************************************************************************
Cr�ateur : Derek
Gestion du changement de blend tree lorsque le personnage change d'arme

                                                    *** BLEND TREE ***

************************************************************************************************************************/


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



/************************************************************************************************************************
Cr�ateur : Derek
Gestion des animations du personnage

                                                  *** ANIMATION ***

************************************************************************************************************************/
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

        // get key input from player
        bool Alpha1Pressed = Input.GetKey(KeyCode.Alpha1);
        bool Alpha2Pressed = Input.GetKey(KeyCode.Alpha2);
        bool Alpha3Pressed = Input.GetKey(KeyCode.Alpha3);
        bool Alpha4Pressed = Input.GetKey(KeyCode.Alpha4);
        bool Alpha5Pressed = Input.GetKey(KeyCode.Alpha5);
        bool Alpha6Pressed = Input.GetKey(KeyCode.Alpha6);
        bool Alpha7Pressed = Input.GetKey(KeyCode.Alpha7);
        bool Alpha8Pressed = Input.GetKey(KeyCode.Alpha8);
        bool Alpha9Pressed = Input.GetKey(KeyCode.Alpha9);

        //  get currently equiped item
        item = inventaire.GetComponent<Inventaire>().getItemCurrentlySelected();


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
        //OBSOL�TE
        //Probl�me du saut toujours pas trouv�
        /*
        if (isJumpingPressed)
        {
            animator.SetBool(isJumpingHash, true);
        }
        else if (!isJumpingPressed)
        {
            animator.SetBool(isJumpingHash, false);
        }
        */                                                         //Le commentaire le plus important ever
        //#####                                                  C'est probablement ca que tu cherche Derek

        //                          *** CHANGEMENT D'ARME, DE BLEND TREE, D'ANIMATION, ETC.. ***

        //Blend Tree Setter
        //Start
        if ((isMelee == false) && (isRifle == false))
        {
            animator.SetBool(isMeleeHash, true);
            Pelle.SetActive(true);
            m4.SetActive(false);
            BaseballBat.SetActive(false);
            rigBuilder.enabled = false;
        }

        //RUntime
        if (item == null || item.nom == "Pelle")
        {
            rigBuilder.enabled = false;
            m4.SetActive(false);
            Pelle.SetActive(true);
            BaseballBat.SetActive(false);
            animator.SetBool(isRifleHash, false);
            animator.SetBool(isMeleeHash, true);
        }
        else if (item.nom == "B�ton de Baseball")
        {
            rigBuilder.enabled = false;
            m4.SetActive(false);
            Pelle.SetActive(false);
            BaseballBat.SetActive(true);
            animator.SetBool(isRifleHash, false);
            animator.SetBool(isMeleeHash, true);
        }
        else if (item.nom == "m4")
        {
            animator.SetBool(isMeleeHash, false);
            animator.SetBool(isRifleHash, true);
            BaseballBat.SetActive(false);
            Pelle.SetActive(false);
            m4.SetActive(true);
            rigBuilder.enabled = true;
        }
        
        //Diggin
        //Regarde si la pelle est active, sinon le joueur peut pelleter avec toute les armes.
        if(isDigginPressed && !isAttacking && Pelle.activeSelf == true)
        {
            animator.SetBool(isDigginHash, true);
        }
        if (!isDigginPressed)
        {
            animator.SetBool(isDigginHash, false);
        }
    }
/************************************************************************************************************************
Cr�ateur : Derek
Groupement de fonction Lock et Velocity

                                                    *** VELOCITY ***

************************************************************************************************************************/
    // handles reset and locking of velocity
    private void lockOrResetVelocity(bool forwardPressed, bool leftPressed, bool rightPressed, bool backPressed, bool runPressed, float currentMaxVelocity)
    {
        // handle the reset function
        handleReset(forwardPressed, leftPressed, rightPressed, backPressed);

        // handdle the lock function
        handleLock(forwardPressed, leftPressed, rightPressed, backPressed, runPressed, currentMaxVelocity);

    }
/************************************************************************************************************************
Cr�ateur : Derek
Reset de la vitesse du personnage lors de l'arr�t du mouvement

                                                    *** RESET ***

************************************************************************************************************************/
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

/************************************************************************************************************************
Cr�ateur : Derek
Impl�mentation d'un mouvement plus r�aliste
Bloque le mouvement du joueur pour qu'il ne d�passe pas une certaine vitesse
Impl�menation n�scessaire puisque nous utilison une acc�l�ration et dec�l�ration pour le mouvement
Sans cette fonction le personnage acc�l�retait infin�ment

                                                    *** LOCK ***

************************************************************************************************************************/
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

/************************************************************************************************************************
Cr�ateur : Derek
Groupement des fonctions d'acc et de decc

                                            *** ACC�L�RATION ET DEC�L�RATION ***

************************************************************************************************************************/
    // handles acceleration and deceleration
    private void changeVelocity(bool forwardPressed, bool leftPressed, bool rightPressed, bool backPressed, bool runPressed, float currentMaxVelocity)
    {
        //handle acceleration function
        handleAcceleration(forwardPressed, leftPressed, rightPressed, backPressed, currentMaxVelocity);

        // handle deceleration function
        handleDeceleration(forwardPressed, leftPressed, rightPressed, backPressed, runPressed);

    }

/************************************************************************************************************************
Cr�ateur : Derek
Implicit
                                                    *** ACC�L�RATION ***

************************************************************************************************************************/
    private void handleAcceleration(bool forwardPressed, bool leftPressed, bool rightPressed, bool backPressed, float currentMaxVelocity)
    {
        // if player presses forward, increase velocity in z direction
        if (forwardPressed && velocityZ < currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * acceleration;
            //Debug.Log("W");
        }

        // increase velocity in left direction
        if (leftPressed && velocityX > -currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * acceleration;
            //Debug.Log("A");
        }

        //increase velocity in right direction
        if (rightPressed && velocityX < currentMaxVelocity)
        {
            velocityX += Time.deltaTime * acceleration;
            //Debug.Log("D");
        }

        //increase velocity in back direction
        if (backPressed && velocityZ > -currentMaxVelocity)
        {
            velocityZ -= Time.deltaTime * acceleration;
            //Debug.Log("S");
        }
    }

/************************************************************************************************************************
Cr�ateur : Derek
Gestion de la decc lors de la course et de la marche
Gestion de la decc selon les directions X Y du perso

                                                    *** Deceleration ***

************************************************************************************************************************/

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

/************************************************************************************************************************
Cr�ateur : Derek
OBSOL�TE
                                                    *** JUMP ***

************************************************************************************************************************/

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
/************************************************************************************************************************
Cr�ateur : Derek
Gestion de la gravit� du personnage
Ce script a �t� cr�� en parall�le avec le saut.
Le saut �tant obsol�te n'emp�che pas l'utilisation de ce script.
J'ai donc d�cid� de garder ce script.

                                                    *** GRAVITY (2013) ***

************************************************************************************************************************/
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
/************************************************************************************************************************
Cr�ateur : Yoan
Gestion de la rotation du joueur vers la cam�ra

                                                 *** ROTATION / CAM�RA ***

************************************************************************************************************************/
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
/************************************************************************************************************************
Cr�ateur : Derek
Gestion de la course du personnage

                                                    *** RUN ***

************************************************************************************************************************/
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
/************************************************************************************************************************
Cr�ateur : Derek
OBSOL�TE
                                                    *** ROTATION / CAM�RA ***

************************************************************************************************************************/
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
/************************************************************************************************************************
Cr�ateur : Yoan
Gestion de la vie du personnage
                                                 *** VIE DU PERSONNAGE ***

************************************************************************************************************************/
    public void PlayerDamage(int degats)
    {
        _health -= degats;
        _uiManager.UpdateHealth(_health, _maxHealth);
        if(_health <= 0)
        {
            Physics.IgnoreLayerCollision(12, 12);
            animator.SetBool("isDead", true);
            _uiManager.joueurMort();
        }
    }
}
