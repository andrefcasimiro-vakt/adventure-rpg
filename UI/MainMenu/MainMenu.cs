using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AF
{

    public class MainMenu : MonoBehaviour
    {

        [Header("UI")]
        public GameObject menuUI;
        public GameObject firstSelectedButton;

        Player player;

        private void Start()
        {
            GameObject.FindWithTag("Player").TryGetComponent<Player>(out player);

            if (player != null)
            {
                player.inputActions.PlayerActions.MainMenu.performed += ctx => Open();
            }
        }

        public void Open()
        {
            if (MenuManager.instance.isOpen && MenuManager.instance.currentMenuWindow == MenuWindow.MAIN)
            {
                Close();
                return;
            }

            player.animator.SetFloat("movementSpeed", 0f);
            MenuManager.instance.SetWindow(MenuWindow.MAIN, this.firstSelectedButton, true);
        }

        public void Close()
        {
            MenuManager.instance.SetWindow(MenuWindow.MAIN, this.firstSelectedButton, false);
        }
    }

}
