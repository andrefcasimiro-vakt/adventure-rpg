using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AF {

    public static class PlayerAnimatorParameters {
        public static string movementSpeed = "movementSpeed";
        public static string isRolling = "isRolling";
    }

    public static class PlayerAnimationClips
    {
        public static string Roll = "Roll";
    }

    public class Player : MonoBehaviour
    {
        Animator animator;

        public InputActions inputActions;

        [Header("Movement")]
        public int walkSpeed = 3;
        public int runSpeed = 5;
        public int rotationSpeed = 8;

        [SerializeField] private Vector3 moveDirection;

        [Header("Flags")]
        public bool isSprinting = false;
        public bool isRolling = false;

        void Awake() {
            if (inputActions == null) {
                inputActions = new InputActions();
            }

            inputActions.PlayerActions.Movement.performed += ctx => moveDirection = ctx.ReadValue<Vector2>();
            inputActions.PlayerActions.Movement.canceled += ctx => moveDirection = Vector2.zero;

            inputActions.PlayerActions.Sprint.performed += ctx => isSprinting = true;
            inputActions.PlayerActions.Sprint.canceled += ctx => isSprinting = false;

            inputActions.PlayerActions.Roll.performed += ctx => HandleRoll();
        }

        void Start() {
            animator = GetComponent<Animator>();
        }

        void OnEnable() {
            inputActions.Enable();
        }

        void Update() {
            isRolling = animator.GetBool(PlayerAnimatorParameters.isRolling);

            HandleMovement();
        }

        /// <summary>
        /// Handles the player movement
        /// </summary>
        void HandleMovement() {
            if (moveDirection.magnitude == 0) {
                animator.SetFloat(PlayerAnimatorParameters.movementSpeed, 0f, .05f, Time.deltaTime);
                return;
            }

            Vector3 targetVector = new Vector3(moveDirection.x, 0, moveDirection.y);
            var speed = (isSprinting ? runSpeed : walkSpeed) * Time.deltaTime;

            //  targetVector = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * targetVector;
            var targetPosition = transform.position + targetVector * speed;
             transform.position = targetPosition;

             var rotation = Quaternion.LookRotation(targetVector);
             transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed);

             if (isSprinting) {
                animator.SetFloat(PlayerAnimatorParameters.movementSpeed, 1f, .05f, Time.deltaTime);
                return;
             }

            animator.SetFloat(PlayerAnimatorParameters.movementSpeed, 0.5f, .05f, Time.deltaTime);
        }

        void HandleRoll()
        {
            animator.CrossFade(PlayerAnimationClips.Roll, 0.05f);
        }

    }

}
