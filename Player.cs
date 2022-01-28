using UnityEngine;
using System.Collections;

namespace AF
{

    public class Player : MonoBehaviour
    {

        InputActions inputActions;

        private Vector3 moveDirection;

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
        public float health = 100;
        public float stamina = 60;
        public float attackPower = 30;

        [Header("Flags")]
        public bool isSprinting = false;
        public bool isRolling = false;
        public bool isAttacking = false;
        public bool isGuarding = false;
        public bool isParrying = false;
        public bool isDead = false;

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
            inputActions.PlayerActions.Movement.canceled += ctx => moveDirection = Vector3.zero;

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
            isDead = animator.GetBool("isDead");

            if (IsNotAvailable())
            {
                return;
            }


            HandleMovement();
        }


        #region Movement

        void HandleMovement()
        {

            if (isAttacking || isRolling)
            {
                return;
            }

            Vector3 targetVector = new Vector3(moveDirection.x, 0, moveDirection.y);

            var rotation = Quaternion.LookRotation(targetVector);

            if (isGuarding)
            {
                if (isSprinting)
                {
                    StopGuard();
                }
                else
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

            }

            if (moveDirection.magnitude != 0)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed);
            }

            if (moveDirection.magnitude == 0)
            {
                animator.SetFloat("movementSpeed", 0f, 0.05f, Time.deltaTime);
                return;
            }

            var speed = (isSprinting ? runSpeed : walkSpeed) * Time.deltaTime;
            var targetPosition = transform.position + targetVector * speed;
            transform.position = targetPosition;

            if (isSprinting)
            {
                animator.SetFloat("movementSpeed", 1f, 0.05f, Time.deltaTime);
                return;
            }

            animator.SetFloat("movementSpeed", 0.5f, 0.05f, Time.deltaTime);
        }

        protected void HandleRoll()
        {
            if (IsNotAvailable())
            {
                return;
            }

            if (isAttacking)
            {
                return;
            }

            if (isGuarding) { 
                StopGuard();
            }

            Vector3 targetVector = new Vector3(moveDirection.x, 0, moveDirection.y);
            var rotation = Quaternion.LookRotation(targetVector);

            if (moveDirection.magnitude != 0) { 
                transform.rotation = rotation;
            }

            animator.CrossFade("Roll", 0.05f);
        }
        #endregion

        #region Combat
        protected void HandleAttack()
        {
            if (IsNotAvailable())
            {
                return;
            }

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
            if (IsNotAvailable())
            {
                return;
            }

            animator.SetBool("isGuarding", true);

            if (shieldGameObject != null)
            {
                shieldGameObject.SetActive(true);
            }

            isParrying = true;
        }
        protected void StopGuard()
        {
            animator.SetBool("isGuarding", false);

            if (shieldGameObject != null)
            {
                shieldGameObject.SetActive(false);
            }

            isParrying = false;
        }

        public void TakeDamage(float amount, EnemyMelee enemy)
        {
            if (IsNotAvailable())
            {
                return;
            }

            if (isRolling)
            {
                return;
            }

            if (isGuarding)
            {
                enemy.ApplyKnockback(this.transform.position);
                animator.Play("BlockHit");
                return;
            }

            animator.Play("TakeDamage");
            ObjectPooler.instance.SpawnFromPool("Blood", enemy.weaponHitbox.transform.position, Quaternion.identity, 1f);

            health -= amount;

            if (health <= 0)
            {

                Die();
            }
        }
        #endregion


        public void Die()
        {
            animator.Play("Die");
        }

        #region Physics
        public void ApplyKnockback(Vector3 enemyPosition)
        {
            Vector3 _moveDirection = transform.position - enemyPosition;
            _moveDirection.y = 0;
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


        public bool IsNotAvailable()
        {
            return isDead || ParrySystem.instance.parryingOngoing;
        }
    }
}
