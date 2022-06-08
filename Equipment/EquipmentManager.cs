using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace AF
{

    public class EquipmentManager : MonoBehaviour, ISaveable
    {

        public Weapon weapon;
        public Shield shield;
        public Armor helmet;
        public Armor chest;
        public Armor gauntlets;
        public Armor legwear;
        public Accessory accessory1;
        public Accessory accessory2;

        EquipmentMenu equipmentMenu;

        public List<string> helmetNakedParts = new List<string>
        {
            "Hair",
            "Head"
        };

        public List<string> armorNakedParts = new List<string>
        {
            "Torso",
        };

        public List<string> gauntletsNakedParts = new List<string>
        {
            "Left Arm",
            "Right Arm",
        };

        public List<string> legwearNakedParts = new List<string>
        {
            "Hips",
            "Left Leg",
            "Right Leg"
        };

        void Start()
        {
            SaveSystem.instance.OnGameLoad += OnGameLoaded;

            ReloadEquipmentGraphics();

            equipmentMenu = FindObjectOfType<EquipmentMenu>(true);
        }

        public void Equip(Weapon weaponToEquip)
        {
            if (weaponToEquip == null) return;

            this.weapon = weaponToEquip;

            equipmentMenu.UpdateEquipmentButtonTexts();
        }

        public void Equip(Shield shieldToEquip)
        {
            if (shieldToEquip == null) return;

            this.shield = shieldToEquip;

            equipmentMenu.UpdateEquipmentButtonTexts();
        }

        public void Equip(Armor armor)
        {
            if (armor == null) return;

            ArmorSlot armorType = armor.armorType;

            if (armorType == ArmorSlot.Head)
            {
                this.helmet = armor;
            }
            else if (armorType == ArmorSlot.Chest)
            {
                this.chest = armor;
            }
            else if (armorType == ArmorSlot.Arms)
            {
                this.gauntlets = armor;
            }
            else if (armorType == ArmorSlot.Legs)
            {
                this.legwear = armor;
            }

            ReloadEquipmentGraphics();

            equipmentMenu.UpdateEquipmentButtonTexts();
        }

        public void EquipAccessory(Accessory accessory, int index)
        {
            if (accessory == null) return;

            if (index == 0)
            {
                this.accessory1 = accessory;
            }
            else
            {
                this.accessory2 = accessory;
            }

            equipmentMenu.UpdateEquipmentButtonTexts();
        }

        public void UnequipWeapon()
        {
            weapon = null;

            equipmentMenu.UpdateEquipmentButtonTexts();
        }

        public void UnequipShield()
        {
            shield = null;


            equipmentMenu.UpdateEquipmentButtonTexts();
        }

        public void UnequipArmorSlot(ArmorSlot armorType)
        {
            if (armorType == ArmorSlot.Head)
            {

                foreach (Transform t in GetComponentsInChildren<Transform>(true))
                {
                    if (this.helmet != null && this.helmet.graphicNameToShow == t.gameObject.name)
                    {
                        t.gameObject.SetActive(false);
                    }
                }

                this.helmet = null;

            }
            else if (armorType == ArmorSlot.Chest)
            {
                foreach (Transform t in GetComponentsInChildren<Transform>(true))
                {
                    if (this.chest != null && this.chest.graphicNameToShow == t.gameObject.name)
                    {
                        t.gameObject.SetActive(false);
                    }
                }

                this.chest = null;
            }
            else if (armorType == ArmorSlot.Arms)
            {
                foreach (Transform t in GetComponentsInChildren<Transform>(true))
                {
                    if (this.gauntlets != null && this.gauntlets.graphicNameToShow == t.gameObject.name)
                    {
                        t.gameObject.SetActive(false);
                    }
                }

                this.gauntlets = null;
            }
            else if (armorType == ArmorSlot.Legs)
            {
                foreach (Transform t in GetComponentsInChildren<Transform>(true))
                {
                    if (this.legwear != null && this.legwear.graphicNameToShow == t.gameObject.name)
                    {
                        t.gameObject.SetActive(false);
                    }
                }

                this.legwear = null;
            }


            ReloadEquipmentGraphics();
            equipmentMenu.UpdateEquipmentButtonTexts();
        }

        public void UnequipAccessory(int slotIndex)
        {
            if (slotIndex == 0)
            {
                this.accessory1 = null;
            }
            else
            {
                this.accessory2 = null;
            }

            equipmentMenu.UpdateEquipmentButtonTexts();
        }

        void ReloadEquipmentGraphics()
        {
            foreach (Transform t in GetComponentsInChildren<Transform>(true))
            {
                // HELMET
                if (helmet == null)
                {
                    if (helmetNakedParts.IndexOf(t.gameObject.name) != -1)
                    {
                        t.gameObject.SetActive(true);
                    }
                }
                else
                {
                    if (helmet.graphicNameToShow == t.gameObject.name)
                    {
                        t.gameObject.SetActive(true);
                    }

                    foreach (string graphicNameToHide in helmet.graphicNamesToHide)
                    {
                        if (t.gameObject.name == graphicNameToHide)
                        {
                            t.gameObject.SetActive(false);
                        }
                    }
                }

                // ARMOR
                if (chest == null)
                {
                    if (armorNakedParts.IndexOf(t.gameObject.name) != -1)
                    {
                        t.gameObject.SetActive(true);
                    }
                }
                else
                {
                    if (chest.graphicNameToShow == t.gameObject.name)
                    {
                        t.gameObject.SetActive(true);
                    }

                    foreach (string graphicNameToHide in chest.graphicNamesToHide)
                    {
                        if (t.gameObject.name == graphicNameToHide)
                        {
                            t.gameObject.SetActive(false);
                        }
                    }
                }

                // GAUNTLETS
                if (gauntlets == null)
                {
                    if (gauntletsNakedParts.IndexOf(t.gameObject.name) != -1)
                    {
                        t.gameObject.SetActive(true);
                    }
                }
                else
                {
                    if (gauntlets.graphicNameToShow == t.gameObject.name)
                    {
                        t.gameObject.SetActive(true);
                    }

                    foreach (string graphicNameToHide in gauntlets.graphicNamesToHide)
                    {
                        if (t.gameObject.name == graphicNameToHide)
                        {
                            t.gameObject.SetActive(false);
                        }
                    }
                }

                // LEGWEAR
                if (legwear == null)
                {
                    if (legwearNakedParts.IndexOf(t.gameObject.name) != -1)
                    {
                        t.gameObject.SetActive(true);
                    }
                }
                else
                {
                    if (legwear.graphicNameToShow == t.gameObject.name)
                    {
                        t.gameObject.SetActive(true);
                    }

                    foreach (string graphicNameToHide in legwear.graphicNamesToHide)
                    {
                        if (t.gameObject.name == graphicNameToHide)
                        {
                            t.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }

        public WeaponInstance GetWeaponInstance()
        {
            WeaponInstance weaponInstance;
            weapon.graphic.TryGetComponent(out weaponInstance);

            if (weaponInstance == null)
            {
                return null;
            }

            return weaponInstance;
        }

        public ShieldInstance GetShieldInstance()
        {
            if (shield == null)
            {
                return null;
            }

            ShieldInstance shieldInstance;
            shield.graphic.TryGetComponent(out shieldInstance);

            if (shieldInstance == null)
            {
                return null;
            }

            return shieldInstance;
        }



        public void OnGameLoaded(GameData gameData)
        {
            Debug.Log("Loading equipment");
            PlayerEquipmentData playerEquipmentData = gameData.playerEquipmentData;

            if (!String.IsNullOrEmpty(playerEquipmentData.weaponName))
            {
                Equip(InventoryDatabase.instance.GetWeapon(playerEquipmentData.weaponName));
            }
            else
            {
                UnequipWeapon();
            }

            if (!String.IsNullOrEmpty(playerEquipmentData.shieldName))
            {
                Equip(InventoryDatabase.instance.GetShield(playerEquipmentData.shieldName));
            }
            else
            {
                UnequipShield();
            }

            if (!String.IsNullOrEmpty(playerEquipmentData.helmetName))
            {
                Equip(InventoryDatabase.instance.GetHelmet(playerEquipmentData.helmetName));
            }
            else
            {
                UnequipArmorSlot(ArmorSlot.Head);
            }

            if (!String.IsNullOrEmpty(playerEquipmentData.chestName))
            {
                Equip(InventoryDatabase.instance.GetChestArmor(playerEquipmentData.chestName));
            }
            else
            {
                UnequipArmorSlot(ArmorSlot.Chest);
            }

            if (!String.IsNullOrEmpty(playerEquipmentData.legwearName))
            {
                Equip(InventoryDatabase.instance.GetLegwear(playerEquipmentData.legwearName));
            }
            else
            {
                UnequipArmorSlot(ArmorSlot.Legs);
            }

            if (!String.IsNullOrEmpty(playerEquipmentData.gauntletsName))
            {
                Equip(InventoryDatabase.instance.GetGauntlets(playerEquipmentData.gauntletsName));
            }
            else
            {
                UnequipArmorSlot(ArmorSlot.Arms);
            }

            if (!String.IsNullOrEmpty(playerEquipmentData.accessory1Name))
            {
                EquipAccessory(InventoryDatabase.instance.GetAccessory(playerEquipmentData.accessory1Name), 0);
            }
            else
            {
                UnequipAccessory(0);
            }

            if (!String.IsNullOrEmpty(playerEquipmentData.accessory2Name))
            {
                EquipAccessory(InventoryDatabase.instance.GetAccessory(playerEquipmentData.accessory2Name), 1);
            }
            else
            {
                UnequipAccessory(1);
            }
        }

    }
}
