using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AF
{
    public class FilteredInventorySlot : MonoBehaviour
    {
        public Item item;
        public int accessoryIndexSlot = -1;

        // Empty Type
        public bool isEmpty;
        public bool isEmptyWeaponSlot;
        public bool isEmptyShieldSlot;
        public ArmorSlot emptyArmorSlot;

        EquipmentManager equipmentManager;

        private void Start()
        {
            GameObject.FindWithTag("Player").TryGetComponent(out equipmentManager);

            if (item != null)
            {
                transform.GetChild(0).GetComponent<Text>().text = item.name;
            }
        }

        public void OnClick()
        {
            if (equipmentManager == null)
            {
                return;
            }

            if (isEmpty)
            {
                if (isEmptyWeaponSlot)
                {
                    equipmentManager.UnequipWeapon();
                }
                else if (isEmptyShieldSlot)
                {
                    equipmentManager.UnequipShield();
                }
                else if (accessoryIndexSlot != -1)
                {
                    equipmentManager.UnequipAccessory(accessoryIndexSlot);
                }
                else
                {
                    equipmentManager.UnequipArmorSlot(emptyArmorSlot);
                }

                return;
            }

            if (item != null)
            {

                Weapon weapon = item as Weapon;
                if (weapon != null)
                {
                    equipmentManager.Equip(weapon);
                    return;
                }

                Shield shield = item as Shield;
                if (shield != null)
                {
                    equipmentManager.Equip(shield);
                    return;
                }

                if (accessoryIndexSlot != -1)
                {
                    equipmentManager.EquipAccessory(item as Accessory, accessoryIndexSlot);
                    return;
                }

                equipmentManager.Equip(item as Armor);
                return;
            }

        }

    }

}
