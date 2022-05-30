using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AF
{

    public class FilteredInventorySlotPanelManager : MonoBehaviour
    {

        public enum FilteredInventoryMode
        {
            Weapon,
            Shield,
            Head,
            Torso,
            Arms,
            Leg,
            Accessory1,
            Accessory2,
        }

        [HideInInspector] public FilteredInventoryMode filter;

        public GameObject emptyFilteredInventorySlotPrefab;
        public GameObject filteredInventorySlotPrefab;

        InventoryManager inventoryManager;
        EquipmentManager equipmentManager;

        public GameObject filteredInventoryPanel;

        Player player;

        private void Start()
        {
            GameObject.FindWithTag("Player").TryGetComponent<Player>(out player);

            if (player != null)
            {
                player.inputActions.PlayerActions.MainMenu.performed += ctx => Close();
            }

            filteredInventoryPanel.gameObject.SetActive(false);

            player.TryGetComponent(out inventoryManager);
            player.TryGetComponent(out equipmentManager);
        }

        public void OpenInventoryForWeapons()
        {
            this.filter = FilteredInventoryMode.Weapon;
            Open();
        }

        public void OpenInventoryForShields()
        {
            this.filter = FilteredInventoryMode.Shield;
            Open();
        }

        public void OpenInventoryForHelmets()
        {
            this.filter = FilteredInventoryMode.Head;
            Open();
        }

        public void OpenInventoryForTorsos()
        {
            this.filter = FilteredInventoryMode.Torso;
            Open();
        }

        public void OpenInventoryForArms()
        {
            this.filter = FilteredInventoryMode.Arms;
            Open();
        }

        public void OpenInventoryForLegs()
        {
            this.filter = FilteredInventoryMode.Leg;
            Open();
        }

        public void OpenInventoryForAccessories1()
        {
            this.filter = FilteredInventoryMode.Accessory1;
            Open();
        }

        public void OpenInventoryForAccessories2()
        {
            this.filter = FilteredInventoryMode.Accessory2;
            Open();
        }

        public void Open()
        {
            // Cleanup
            foreach (Transform t in filteredInventoryPanel.transform)
            {
                Destroy(t.gameObject);
            }

            filteredInventoryPanel.gameObject.SetActive(true);

            // Empty Inventory Slot Button Logic

            FilteredInventorySlot emptySlot = Instantiate(emptyFilteredInventorySlotPrefab, filteredInventoryPanel.transform).GetComponent<FilteredInventorySlot>();

            emptySlot.isEmpty = true;
            emptySlot.item = null;

            EventSystem.current.SetSelectedGameObject(emptySlot.gameObject);

            if (filter == FilteredInventoryMode.Weapon)
            {
                emptySlot.isEmptyWeaponSlot = true;
            }
            else if (filter == FilteredInventoryMode.Shield)
            {
                emptySlot.isEmptyShieldSlot = true;
            }
            else if (filter == FilteredInventoryMode.Head)
            {
                emptySlot.emptyArmorSlot = ArmorSlot.Head;
            }
            else if (filter == FilteredInventoryMode.Torso)
            {
                emptySlot.emptyArmorSlot = ArmorSlot.Chest;
            }
            else if (filter == FilteredInventoryMode.Arms)
            {
                emptySlot.emptyArmorSlot = ArmorSlot.Arms;
            }
            else if (filter == FilteredInventoryMode.Leg)
            {
                emptySlot.emptyArmorSlot = ArmorSlot.Legs;
            }
            else if (filter == FilteredInventoryMode.Accessory1)
            {
                emptySlot.accessoryIndexSlot = 0;
            }
            else if (filter == FilteredInventoryMode.Accessory2)
            {
                emptySlot.accessoryIndexSlot = 1;
            }

            if (filter == FilteredInventoryMode.Weapon)
            {
                List<Weapon> weapons = inventoryManager.GetWeapons();

                foreach (Weapon w in weapons)
                {
                    GameObject instantiatedSlot = Instantiate(filteredInventorySlotPrefab, filteredInventoryPanel.transform);
                    FilteredInventorySlot filteredInventorySlot;
                    instantiatedSlot.TryGetComponent(out filteredInventorySlot);
                    filteredInventorySlot.item = w;
                }

                return;
            }

            if (filter == FilteredInventoryMode.Shield)
            {
                List<Shield> shields = inventoryManager.GetShields();

                foreach (Shield s in shields)
                {
                    GameObject instantiatedSlot = Instantiate(filteredInventorySlotPrefab, filteredInventoryPanel.transform);
                    FilteredInventorySlot filteredInventorySlot;
                    instantiatedSlot.TryGetComponent(out filteredInventorySlot);
                    filteredInventorySlot.item = s;
                }

                return;
            }

            if (filter == FilteredInventoryMode.Head)
            {
                List<Armor> helmets = inventoryManager.GetHelmets();

                foreach (Armor armor in helmets)
                {
                    GameObject instantiatedSlot = Instantiate(filteredInventorySlotPrefab, filteredInventoryPanel.transform);
                    FilteredInventorySlot filteredInventorySlot;
                    instantiatedSlot.TryGetComponent(out filteredInventorySlot);
                    filteredInventorySlot.item = armor;
                }

                return;
            }


            if (filter == FilteredInventoryMode.Torso)
            {
                List<Armor> chestpieces = inventoryManager.GetChestpieces();

                foreach (Armor armor in chestpieces)
                {
                    GameObject instantiatedSlot = Instantiate(filteredInventorySlotPrefab, filteredInventoryPanel.transform);
                    FilteredInventorySlot filteredInventorySlot;
                    instantiatedSlot.TryGetComponent(out filteredInventorySlot);
                    filteredInventorySlot.item = armor;
                }

                return;
            }

            if (filter == FilteredInventoryMode.Arms)
            {
                List<Armor> gauntlets = inventoryManager.GetGauntlets();

                foreach (Armor armor in gauntlets)
                {
                    GameObject instantiatedSlot = Instantiate(filteredInventorySlotPrefab, filteredInventoryPanel.transform);
                    FilteredInventorySlot filteredInventorySlot;
                    instantiatedSlot.TryGetComponent(out filteredInventorySlot);
                    filteredInventorySlot.item = armor;
                }

                return;
            }


            if (filter == FilteredInventoryMode.Leg)
            {
                List<Armor> legwear = inventoryManager.GetLegwear();

                foreach (Armor armor in legwear)
                {
                    GameObject instantiatedSlot = Instantiate(filteredInventorySlotPrefab, filteredInventoryPanel.transform);
                    FilteredInventorySlot filteredInventorySlot;
                    instantiatedSlot.TryGetComponent(out filteredInventorySlot);
                    filteredInventorySlot.item = armor;
                }

                return;
            }


            if (filter == FilteredInventoryMode.Accessory1)
            {
                List<Accessory> accessories = inventoryManager.GetAccessories();

                foreach (Accessory accessory in accessories)
                {
                    GameObject instantiatedSlot = Instantiate(filteredInventorySlotPrefab, filteredInventoryPanel.transform);
                    FilteredInventorySlot filteredInventorySlot;
                    instantiatedSlot.TryGetComponent(out filteredInventorySlot);
                    filteredInventorySlot.item = accessory;
                }

                return;
            }

            if (filter == FilteredInventoryMode.Accessory2)
            {
                List<Accessory> accessories = inventoryManager.GetAccessories();

                foreach (Accessory accessory in accessories)
                {
                    GameObject instantiatedSlot = Instantiate(filteredInventorySlotPrefab, filteredInventoryPanel.transform);
                    FilteredInventorySlot filteredInventorySlot;
                    instantiatedSlot.TryGetComponent(out filteredInventorySlot);
                    filteredInventorySlot.item = accessory;
                }

                return;
            }

        }

        public void Close()
        {
            if (filteredInventoryPanel.activeSelf == false)
            {
                return;
            }

            filteredInventoryPanel.gameObject.SetActive(false);
        }

    }

}