using UnityEngine;
using System.Collections;

namespace AF
{

    public class InputListener : MonoBehaviour
    {
        [HideInInspector]
        public bool hasPressedConfirmButton;
        [HideInInspector]
        public bool hasPressedCancelButton;

        InputActions inputActions;

        protected void OnEnable()
        {
            inputActions = new InputActions();

            // Decision Button Input
            inputActions.PlayerActions.Attack.performed += ctx =>
            {
                hasPressedConfirmButton = true;
            };

            inputActions.PlayerActions.Attack.canceled += ctx =>
            {
                hasPressedConfirmButton = false;
            };


            inputActions.Enable();

        }

        protected void OnDisable()
        {
            if (inputActions != null)
            {
                inputActions.Disable();
                inputActions = null;
            }
        }

    }
}

