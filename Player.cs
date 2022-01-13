using UnityEngine;
using System.Collections;

namespace AF
{

    public class Player : MonoBehaviour
    {

        InputActions inputActions;

        [SerializeField] private Vector3 moveDirection;
        [SerializeField] private Vector3 lastMoveDirection;

        protected Animator animator => GetComponent<Animator>();

        [Header("Movement")]
        public float walkSpeed = 6;
        public float runSpeed = 9;
        public float rotationSpeed = 8;

        [Header("Combat")]
        public GameObject shieldGameObject;
        private int attackComboIndex = 0;

        [Header("Stats")]
        public int health = 100;
        public int stamina = 60;

        [Header("Flags")]
        public bool isSprinting = false;
        public bool isRolling = false;
        public bool isAttacking = false;
        public bool isGuarding = false;

        void OnEnable()
        {
            inputActions.Enable();
        }

        void Awake()
        {
            if (inputActions == null)
            {
                inputActions = new InputActions();
            }

            // Movement Input
            inputActions.PlayerActions.Movement.performed += ctx => moveDirection = ctx.ReadValue<Vector2>();
            inputActions.PlayerActions.Movement.canceled += ctx =>
            {
                lastMoveDirection = moveDirection;
                moveDirection = Vector3.zero;
            };

            inputActions.PlayerActions.Sprint.performed += ctx => isSprinting = true;
            inputActions.PlayerActions.Sprint.canceled += ctx => isSprinting = false;

            inputActions.PlayerActions.Roll.performed += ctx => HandleRoll();

            // Combat Input
            inputActions.PlayerActions.Attack.performed += ctx => HandleAttack();

            inputActions.PlayerActions.Guard.performed += ctx => Guard();
            inputActions.PlayerActions.Guard.canceled += ctx => StopGuard();
        }

        private void Start()
        {
            if (shieldGameObject != null)
            {
                shieldGameObject.SetActive(false);
            }
        }

        protected void Update()
        {
            isRolling = animator.GetBool("isRolling");
            isAttacking = animator.GetBool("isAttacking");
            isGuarding = animator.GetBool("isGuarding");
        }

        private void FixedUpdate()
        {
            HandleMovement();
        }

        #region Movement

        void HandleMovement()
        {
            if (isAttacking || isRolling)
            {
                return;
            }

            Vector3 targetVector = moveDirection.magnitude != 0
                ? new Vector3(moveDirection.x, 0, moveDirection.y)
                : new Vector3(lastMoveDirection.x, 0, lastMoveDirection.y);

            var rotation = Quaternion.LookRotation(targetVector);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed);


            if (moveDirection.magnitude == 0)
            {
                animator.SetFloat("movementSpeed", 0f, .05f, Time.deltaTime);
                return;
            }

            var speed = (isSprinting ? runSpeed : walkSpeed) * Time.deltaTime;
            var targetPosition = transform.position + targetVector * speed;
            transform.position = targetPosition;

            if (isSprinting)
            {
                animator.SetFloat("movementSpeed", 1f, .05f, Time.deltaTime);
                return;
            }

            animator.SetFloat("movementSpeed", 0.5f, .05f, Time.deltaTime);
        }

        protected void HandleRoll()
        {
            if (isAttacking)
            {
                return;
            }

            animator.CrossFade("Roll", 0.05f);
        }
        #endregion

        #region Combat
        protected void HandleAttack()
        {
            if (isAttacking)
            {
                return;
            }

            if (isGuarding)
            {
                StopGuard();
            }

            if (attackComboIndex > 2)
            {
                attackComboIndex = 0;
            }

            if (attackComboIndex == 0)
            {
                animator.CrossFade("Attack1", 0.05f);
            }
            else if (attackComboIndex == 1)
            {
                animator.CrossFade("Attack2", 0.05f);
            }
            else
            {
                animator.CrossFade("Attack3", 0.05f);
            }

            attackComboIndex++;
        }

        protected void Guard()
        {
            animator.SetBool("isGuarding", true);

            if (shieldGameObject != null)
            {
                shieldGameObject.SetActive(true);
            }
        }
        protected void StopGuard()
        {
            animator.SetBool("isGuarding", false);

            if (shieldGameObject != null)
            {
                shieldGameObject.SetActive(false);
            }
        }
        #endregion

    }
}
