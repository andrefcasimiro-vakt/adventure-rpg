using UnityEngine;
using System.Collections;

namespace AF
{

    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Animator))]
    public class Player : Character
    {
        public readonly int hashMovementSpeed = Animator.StringToHash("movementSpeed");


        [Header("Movement")]
        public float walkSpeed = 6;
        public float runSpeed = 9;
        public float rotationSpeed = 8;
        Vector3 moveDirection;

        [Header("Flags")]
        public bool isAttacking = false;
        public bool isParrying = false;
        public bool isSprinting = false;
        public bool isBlocking = false;
        public bool isRolling = false;
        public bool isDead = false;

        [Header("References")]
        public Transform headTransform;

        [HideInInspector] public Animator animator => GetComponent<Animator>();
        [HideInInspector] public Rigidbody rigidbody => GetComponent<Rigidbody>();
        [HideInInspector] public CapsuleCollider capsuleCollider => GetComponent<CapsuleCollider>();

        InputActions inputActions;

        [HideInInspector] public PlayerCombatManager playerCombatManager => GetComponent<PlayerCombatManager>();

        void OnEnable()
        {
            inputActions.Enable();
        }

        void Awake()
        {
            base.Awake();

            if (inputActions == null)
            {
                inputActions = new InputActions();
            }

            // Movement Input
            inputActions.PlayerActions.Movement.performed += ctx => moveDirection = ctx.ReadValue<Vector2>();
            inputActions.PlayerActions.Movement.canceled += ctx => moveDirection = Vector3.zero;

            inputActions.PlayerActions.Sprint.performed += ctx => isSprinting = true;
            inputActions.PlayerActions.Sprint.canceled += ctx => isSprinting = false;

            inputActions.PlayerActions.Roll.performed += ctx => HandleRoll();

            // Combat Input
            inputActions.PlayerActions.Attack.performed += ctx => playerCombatManager.HandleAttack();

            inputActions.PlayerActions.Guard.performed += ctx => playerCombatManager.Guard();
            inputActions.PlayerActions.Guard.canceled += ctx => playerCombatManager.StopGuard();

            // UI
            inputActions.PlayerActions.MainMenu.performed += ctx => MainMenu.instance.Open();
        }

        private void Start()
        {
            if (inventory.shield != null)
            {
                inventory.shield.SetActive(false);
            }
        }



        protected void Update()
        {
            isBlocking = animator.GetBool("isBlocking");
            isRolling = animator.GetBool("isRolling");
            isAttacking = animator.GetBool("isAttacking");
            isDead = animator.GetBool("isDead");
            isParrying = animator.GetBool("isParrying");

            if (inventory.shield != null)
            {
                inventory.shield.SetActive(isBlocking);
            }
        }


        protected void FixedUpdate()
        {
            if (IsNotAvailable())
            {
                return;
            }

            HandleMovement();

            if (isSprinting)
            {
                if (moveDirection.magnitude <= 0)
                {
                    isSprinting = false;
                }
                else
                {
                    // Handle issues with floating values on limit (causing footstep sounds to play overlapped)
                    if (animator.GetFloat(hashMovementSpeed) >= 0.95f)
                    {
                        animator.SetFloat(hashMovementSpeed, 1f);
                    }
                    else
                    {
                        animator.SetFloat(hashMovementSpeed, 1f, 0.05f, Time.fixedDeltaTime);
                    }
                }
            }
            else if (moveDirection.magnitude == 0)
            {
                // Handle issues with floating values on limit (causing footstep sounds to play overlapped)
                if (animator.GetFloat(hashMovementSpeed) <= 0.05f)
                {
                    animator.SetFloat(hashMovementSpeed, 0f);
                }
                else
                {
                    animator.SetFloat(hashMovementSpeed, 0f, 0.05f, Time.fixedDeltaTime);
                }
            }
            else
            {
                // Handle issues with floating values on limit (causing footstep sounds to play overlapped)
                if (animator.GetFloat(hashMovementSpeed) <= 0.55f || animator.GetFloat(hashMovementSpeed) >= 0.45f)
                {
                    animator.SetFloat(hashMovementSpeed, 0.5f);
                }
                else
                {
                    animator.SetFloat(hashMovementSpeed, 0.5f, 0.05f, Time.fixedDeltaTime);
                }
            }
        }


        #region Movement

        void HandleMovement()
        {

            if (isAttacking || isRolling)
            {
                return;
            }

            Vector3 targetVector = GetMoveDirection();

            var rotation = Quaternion.LookRotation(targetVector);

            if (isBlocking)
            {
                if (isSprinting)
                {
                    playerCombatManager.StopGuard();
                }
                else
                {
                    // Lock on to target logic
                    Character closestCharacter = inventory.shield.GetComponent<Shield>().FindClosestCharacter(this.transform.position);

                    if (closestCharacter != null)
                    {
                        var lookPos = closestCharacter.transform.position - transform.position;
                        lookPos.y = 0;
                        rotation = Quaternion.LookRotation(lookPos);
                    }
                }

            }

            if (moveDirection.magnitude != 0)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed);
            }

            var speed = (isSprinting ? runSpeed : walkSpeed) * Time.fixedDeltaTime;
            var targetPosition = transform.position + targetVector * speed;
            transform.position = targetPosition;
        }

        protected void HandleRoll()
        {
            if (IsNotAvailable() || isAttacking)
            {
                return;
            }

            if (isBlocking)
            { 
                playerCombatManager.StopGuard();
            }

            var rotation = Quaternion.LookRotation(GetMoveDirection());

            if (moveDirection.magnitude != 0) { 
                transform.rotation = rotation;
            }

            animator.CrossFade("Roll", 0.05f);
        }
        #endregion

        
        public Vector3 GetMoveDirection()
        {
            bool cameraInverted = Camera.main.transform.forward.z <= 0;

            return cameraInverted
                ? new Vector3(moveDirection.x * -1, 0, moveDirection.y * -1)
                : new Vector3(moveDirection.x, 0, moveDirection.y);
        }

        public bool IsNotAvailable()
        {
            return isDead || MainMenu.instance.isOpen;
        }

    }
}
