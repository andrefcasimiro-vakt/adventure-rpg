using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AF
{

    public class InventoryManager : MonoBehaviour
    {

        [Header("Melee Weapon")]
        public Weapon meleeWeapon;

        [Header("Shield")]
        public GameObject shield;

        [Header("Range Weapon")]
        public GameObject rangeWeapon;
        public Projectile projectilePrefab;

        public List<Item> items = new List<Item>();

        private void Start()
        {
            if (rangeWeapon != null)
            {
                rangeWeapon.SetActive(false);
            }

            if (shield != null)
            {
                shield.SetActive(false);
            }
        }

        public List<Weapon> GetWeapons()
        {
            List<Weapon> weapons = new List<Weapon>();

            foreach (Item i in items)
            {
                Weapon weapon = i as Weapon;
                if (weapon != null)
                {
                    weapons.Add(weapon);
                }
            }

            return weapons;
        }

        public List<Shield> GetShields()
        {
            List<Shield> shields = new List<Shield>();

            foreach (Item i in items)
            {
                Shield shield = i as Shield;
                if (shield != null)
                {
                    shields.Add(shield);
                }
            }

            return shields;
        }

        public List<Armor> GetHelmets()
        {
            List<Armor> helmets = new List<Armor>();

            foreach (Item i in items)
            {
                Armor armor = i as Armor;
                if (armor != null)
                {
                    if (armor.armorType == ArmorSlot.Head)
                    {
                        helmets.Add(armor);
                    }
                }
            }

            return helmets;
        }

        public List<Armor> GetChestpieces()
        {
            List<Armor> chestpieces = new List<Armor>();

            foreach (Item i in items)
            {
                Armor armor = i as Armor;
                if (armor != null)
                {
                    if (armor.armorType == ArmorSlot.Chest)
                    {
                        chestpieces.Add(armor);
                    }
                }
            }

            return chestpieces;
        }

        public List<Armor> GetGauntlets()
        {
            List<Armor> gauntlets = new List<Armor>();

            foreach (Item i in items)
            {
                Armor armor = i as Armor;
                if (armor != null)
                {
                    if (armor.armorType == ArmorSlot.Arms)
                    {
                        gauntlets.Add(armor);
                    }
                }
            }

            return gauntlets;
        }

        public List<Armor> GetLegwear()
        {
            List<Armor> legwear = new List<Armor>();

            foreach (Item i in items)
            {
                Armor armor = i as Armor;
                if (armor != null)
                {
                    if (armor.armorType == ArmorSlot.Legs)
                    {
                        legwear.Add(armor);
                    }
                }
            }

            return legwear;
        }


        public List<Accessory> GetAccessories()
        {
            List<Accessory> accessories = new List<Accessory>();

            foreach (Item i in items)
            {
                Accessory accessory = i as Accessory;
                if (accessory != null)
                {
                    accessories.Add(accessory);
                }
            }

            return accessories;
        }

    }

}
