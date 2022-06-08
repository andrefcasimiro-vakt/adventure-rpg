using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace AF
{

    public class EquipmentMenu : MonoBehaviour
    {

        [Header("UI")]
        public GameObject equipmentMenuUI;
        public GameObject firstSelectedButton;

        [Header("Equipment Buttons")]
        public Text weaponBTNText;
        public Text shieldBTNText;
        public Text headBTNText;
        public Text chestBTNText;
        public Text armsBTNText;
        public Text legsBTNText;
        public Text accessory1Text;
        public Text accessory2Text;

        Player player;

        
        private void Start()
        {
            Setup();
        }

        private void OnSceneLoaded()
        {
            Setup();
        }

        void Setup()
        {
            GameObject.FindWithTag("Player").TryGetComponent<Player>(out player);
        }


        public void Open()
        {
            UpdateEquipmentButtonTexts();

            MenuManager.instance.SetWindow(MenuWindow.EQUIPMENT, this.firstSelectedButton, true);
        }


        public void Close()
        {
            if (MenuManager.instance.currentMenuWindow != MenuWindow.EQUIPMENT)
            {
                return;
            }

            MenuManager.instance.SetWindow(MenuWindow.MAIN, this.firstSelectedButton, true);
        }

        public void UpdateEquipmentButtonTexts()
        {
            Setup();

            if (player.equipmentManager.weapon != null)
            {
                weaponBTNText.text = "WEAPON | " + player.equipmentManager.weapon.name;
            }
            else
            {
                weaponBTNText.text = "WEAPON | None";
            }


            if (player.equipmentManager.shield != null)
            {
                shieldBTNText.text = "SHIELD | " + player.equipmentManager.shield.name;
            }
            else
            {
                shieldBTNText.text = "SHIELD | None";
            }

            if (player.equipmentManager.helmet != null)
            {
                headBTNText.text = "HEAD | " + player.equipmentManager.helmet.name;
            }
            else
            {
                headBTNText.text = "HEAD | None";
            }

            if (player.equipmentManager.chest != null)
            {
                chestBTNText.text = "CHEST | " + player.equipmentManager.chest.name;
            }
            else
            {
                chestBTNText.text = "CHEST | None";
            }

            if (player.equipmentManager.gauntlets != null)
            {
                legsBTNText.text = "ARMS | " + player.equipmentManager.gauntlets.name;
            }
            else
            {
                legsBTNText.text = "ARMS | None";
            }

            if (player.equipmentManager.legwear != null)
            {
                legsBTNText.text = "LEGS | " + player.equipmentManager.legwear.name;
            }
            else
            {
                legsBTNText.text = "LEGS | None";
            }

            if (player.equipmentManager.accessory1 != null)
            {
                accessory1Text.text = "ACCESSORY 1 | " + player.equipmentManager.accessory1.name;
            }
            else
            {
                accessory1Text.text = "ACCESSORY 1 | None";
            }

            if (player.equipmentManager.accessory2 != null)
            {
                accessory2Text.text = "ACCESSORY 2 | " + player.equipmentManager.accessory2.name;
            }
            else
            {
                accessory2Text.text = "ACCESSORY 2 | None";
            }
        }
    }

}
