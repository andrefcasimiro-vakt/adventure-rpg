using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AF
{
    public enum MenuWindow
    {
        NULL,
        MAIN,
        EQUIPMENT
    }

    [RequireComponent(typeof(AudioSource))]
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager instance;

        [Header("UI")]
        public GameObject mainMenuUI;
        public GameObject equipmentMenuUI;

        [Header("SFX")]
        public AudioClip buttonSelectSfx;
        AudioSource audioSource => GetComponent<AudioSource>();

        [HideInInspector] public bool isOpen = false;
        [HideInInspector] public MenuWindow currentMenuWindow = MenuWindow.NULL;


        [HideInInspector] public GameObject firstSelectedGameObject;

        Player player;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            RefreshWindows(null);
        }

        public void SetWindow(MenuWindow nextMenuWindow, GameObject firstSelectedGameObject, bool shouldOpen)
        {
            currentMenuWindow = nextMenuWindow;

            isOpen = shouldOpen;

            RefreshWindows(firstSelectedGameObject);
        }

        void RefreshWindows(GameObject firstSelectedGameObject)
        {
            switch (currentMenuWindow)
            {
                case MenuWindow.MAIN:
                    equipmentMenuUI.SetActive(false);
                    mainMenuUI.SetActive(isOpen);
                    break;
                case MenuWindow.EQUIPMENT:
                    equipmentMenuUI.SetActive(isOpen);
                    mainMenuUI.SetActive(false);
                    break;
                default:
                    equipmentMenuUI.SetActive(false);
                    mainMenuUI.SetActive(false);
                    break;
            }

            EventSystem.current.SetSelectedGameObject(firstSelectedGameObject);
        }

        public void PlayButtonSelect()
        {
            audioSource.PlayOneShot(buttonSelectSfx);
        }
    }

}