using UnityEngine;
using System.Collections;

namespace AF
{

    public class Player : MonoBehaviour
    {

        InputActions inputActions;

        private Vector3 moveDirection;
        private Vector3 lastMoveDirection;

        protected Animator animator => GetComponent<Animator>();
        protected Rigidbody rigidbody => GetComponent<Rigidbody>();

        [Header("Movement")]
        public float walkSpeed = 6;
        public float runSpeed = 9;
        public float rotationSpeed = 8;

        [Header("Combat")]
        public GameObject weaponGameObject;
        Hitbox weaponHitbox;
        public GameObject shieldGameObject;
        private int attackComboIndex = 0;
        public float impactOnHittinEnemyShield = 50000f;

        [Header("Stats")]
        public int health = 100;
        public int stamina = 60;

        [Header("Flags")]
        public bool isSprinting = false;
        public bool isRolling = false;
        public bool isAttacking = false;
        public bool isGuarding = false;

        public LayerMask characterLayer;


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

            if (weaponGameObject != null)
            {
                weaponHitbox = weaponGameObject.GetComponent<Hitbox>();
            }
        }

        protected void Update()
        {
            isRolling = animator.GetBool("isRolling");
            isAttacking = animator.GetBool("isAttacking");
            isGuarding = animator.GetBool("isGuarding");

            HandleMovement();
        }

        private void FixedUpdate()
        {
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

            if (isGuarding)
            {
                // Lock on to target logic
                Character closestCharacter = shieldGameObject.GetComponent<Shield>().FindClosestCharacter(this.transform.position);

                if (closestCharacter != null)
                {
                    var lookPos = closestCharacter.transform.position - transform.position;
                    lookPos.y = 0;
                    rotation = Quaternion.LookRotation(lookPos);
                }
            }

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

            if (isGuarding) { 
                StopGuard();
            }

            Vector3 targetVector = moveDirection.magnitude != 0
                ? new Vector3(moveDirection.x, 0, moveDirection.y)
                : new Vector3(lastMoveDirection.x, 0, lastMoveDirection.y);

            var rotation = Quaternion.LookRotation(targetVector);
            transform.rotation = rotation;

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

        public void TakeDamage()
        {

        }
        #endregion

        #region Physics
        public void ApplyKnockback(Vector3 enemyPosition)
        {
            Vector3 _moveDirection = transform.position - enemyPosition;
            rigidbody.AddForce(_moveDirection.normalized * impactOnHittinEnemyShield);
        }
        #endregion


        #region
        /// <summary>
        /// Animation Event Function. Handles the hitbox activation
        /// </summary>
        public void ActivateHitbox()
        {
            weaponHitbox.Enable();
        }
        public void DeactivateHitbox()
        {
            weaponHitbox.Disable();
        }
        #endregion
    }
}
