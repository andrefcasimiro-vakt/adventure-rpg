using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AF
{

    public class EquipmentManager : MonoBehaviour
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
            ReloadEquipmentGraphics();

            equipmentMenu = FindObjectOfType<EquipmentMenu>(true);
        }

        public void Equip(Weapon weaponToEquip)
        {
            this.weapon = weaponToEquip;

            equipmentMenu.UpdateEquipmentButtonTexts();
        }

        public void Equip(Shield shieldToEquip)
        {
            this.shield = shieldToEquip;

            equipmentMenu.UpdateEquipmentButtonTexts();
        }

        public void Equip(Armor armor)
        {
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
                    if (this.helmet.graphicNameToShow == t.gameObject.name)
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
                    if (this.chest.graphicNameToShow == t.gameObject.name)
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
                    if (this.gauntlets.graphicNameToShow == t.gameObject.name)
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
                    if (this.legwear.graphicNameToShow == t.gameObject.name)
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
    }
}
