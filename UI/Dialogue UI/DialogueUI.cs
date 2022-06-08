using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace AF
{

    public class DialogueUI : MonoBehaviour
    {

        public GameObject dialoguePanel;

        public GameObject actorNameImage;
        public Text actorName;
        public Text message;


        InputActions inputActions;
        bool hasFinished = false;

        public static DialogueUI instance;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
            }

            DontDestroyOnLoad(this.gameObject);

            HideUI();
        }

        private void OnEnable()
        {
            inputActions = new InputActions();

            // Decision Button Input
            inputActions.PlayerActions.Attack.performed += ctx =>
            {
                hasFinished = true;
            };

            inputActions.Enable();

        }

        private void OnDisable()
        {
            if (inputActions != null)
            {
                inputActions.Disable();
                inputActions = null;
            }
        }

        private void FixedUpdate()
        {
            if (hasFinished == true)
            {
                hasFinished = false;
            }
        }

        public IEnumerator ShowDialogue(string actorName, string message)
        {
            if (System.String.IsNullOrEmpty(actorName) == false)
            {
                this.actorName.text = actorName;
                this.actorNameImage.SetActive(true);
            }
            else
            {
                this.actorNameImage.SetActive(false);
            }

            this.message.text = message;

            DisplayUI();

            yield return new WaitUntil(() => hasFinished == true);

            HideUI();
        }

        private void DisplayUI()
        {
            this.dialoguePanel.SetActive(true);
        }

        private void HideUI()
        {
            this.dialoguePanel.SetActive(false);
        }


    }

}
