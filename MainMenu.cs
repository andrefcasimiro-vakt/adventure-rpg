using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AF
{

    [RequireComponent(typeof(AudioSource))]
    public class MainMenu : MonoBehaviour
    {
        public static MainMenu instance;

        [Header("UI")]
        public GameObject menuUI;
        public Button firstSelectedButton;

        [Header("SFX")]
        public AudioClip buttonSelectSfx;
        AudioSource audioSource => GetComponent<AudioSource>();

        [HideInInspector] public bool isOpen = false;

        Player player;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            menuUI.SetActive(false);
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }

        public void Open()
        {
            if (isOpen)
            {
                Close();
                return;
            }

            player.animator.SetFloat("movementSpeed", 0f);
            

            menuUI.SetActive(true);
            EventSystem.current.SetSelectedGameObject(firstSelectedButton.gameObject);
            isOpen = true;
        }

        private void Update()
        {
            // Debug.Log(EventSystem.current.currentSelectedGameObject);
        }

        public void Close()
        {
            menuUI.SetActive(false);
            isOpen = false;
        }

        public void PlayButtonSelect()
        {
            audioSource.PlayOneShot(buttonSelectSfx);
        }
    }

}