using UnityEngine;
using System.Collections;

namespace AF
{

    public class Player : Character
    {

        InputActions inputActions;
        private Vector3 moveDirection;

        [Header("Movement")]
        public float walkSpeed = 6;
        public float runSpeed = 9;
        public float rotationSpeed = 8;

        [Header("Combat")]
        private int attackComboIndex = 0;

        [Header("Flags")]
        public bool isAttacking = false;
        public bool isParrying = false;
        public bool isSprinting = false;

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

            // UI
            inputActions.PlayerActions.MainMenu.performed += ctx => MainMenu.instance.Open();
        }

        private void Start()
        {
            base.Start();
        }



        protected void Update()
        {
            base.Update();

            isRolling = animator.GetBool("isRolling");
            isAttacking = animator.GetBool("isAttacking");
            isBlocking = animator.GetBool("isBlocking");
            isDead = animator.GetBool("isDead");
            isParrying = animator.GetBool("isParrying");

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
                    if (animator.GetFloat("movementSpeed") >= 0.95f)
                    {
                        animator.SetFloat("movementSpeed", 1f);
                    }
                    else
                    {
                        animator.SetFloat("movementSpeed", 1f, 0.05f, Time.fixedDeltaTime);
                    }
                }
            }
            else if (moveDirection.magnitude == 0)
            {
                // Handle issues with floating values on limit (causing footstep sounds to play overlapped)
                if (animator.GetFloat("movementSpeed") <= 0.05f)
                {
                    animator.SetFloat("movementSpeed", 0f);
                }
                else
                {
                    animator.SetFloat("movementSpeed", 0f, 0.05f, Time.fixedDeltaTime);
                }
            }
            else
            {
                // Handle issues with floating values on limit (causing footstep sounds to play overlapped)
                if (animator.GetFloat("movementSpeed") <= 0.55f || animator.GetFloat("movementSpeed") >= 0.45f)
                {
                    animator.SetFloat("movementSpeed", 0.5f);
                }
                else
                {
                    animator.SetFloat("movementSpeed", 0.5f, 0.05f, Time.fixedDeltaTime);
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

            Vector3 targetVector = new Vector3(moveDirection.x, 0, moveDirection.y);

            var rotation = Quaternion.LookRotation(targetVector);

            if (isBlocking)
            {
                if (isSprinting)
                {
                    StopGuard();
                }
                else
                {
                    // Lock on to target logic
                    Character closestCharacter = shield.GetComponent<Shield>().FindClosestCharacter(this.transform.position);

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
            if (IsNotAvailable())
            {
                return;
            }

            if (isAttacking)
            {
                return;
            }

            if (isBlocking) { 
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

            if (isBlocking)
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

            if (isSprinting)
            {
                return;
            }

            animator.SetBool("isBlocking", true);
            animator.SetBool("isParrying", true);

            combatAudiosource.PlayOneShot(startBlockingSfx);

            if (shield != null)
            {
                shield.SetActive(true);
            }
        }

        protected void StopGuard()
        {
            animator.SetBool("isBlocking", false);

            if (shield != null)
            {
                shield.SetActive(false);
            }
        }

        public override void TakeDamage(float amount, Character enemy)
        {
            if (IsNotAvailable())
            {
                return;
            }

            if (isRolling)
            {
                return;
            }

            // Check if player is facing enemy, other wise ignore shield
            bool playerFacingEnemy = Vector3.Angle(transform.forward * -1, enemy.transform.forward) <= 90f;
            if (isBlocking && playerFacingEnemy)
            {
                FaceTarget(enemy.transform, 100f);
                enemy.ApplyKnockback(this.transform.position);
                animator.Play("BlockHit");
                return;
            }

            ObjectPooler.instance.SpawnFromPool("Blood", enemy.weaponHitbox.transform.position, Quaternion.identity, 1f);
            health -= amount;

            if (health <= 0)
            {

                Die();
            }
            else
            {
                animator.Play("TakeDa m age");
                PlayDamage();
            }
        }
        #endregion


        public void Die()
        {
            animator.Play("Die");
        }

    }
}
