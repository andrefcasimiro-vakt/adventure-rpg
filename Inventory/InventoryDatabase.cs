using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AF
{

    // Holds all the scriptable objects in the game in order for the save / load system to retrieve them
    public class InventoryDatabase : MonoBehaviour
    {

        public List<Item> items = new List<Item>();
        public List<Armor> helmets = new List<Armor>();
        public List<Armor> chestArmors = new List<Armor>();
        public List<Armor> gauntlets = new List<Armor>();
        public List<Armor> legwear = new List<Armor>();
        public List<Weapon> weapons = new List<Weapon>();
        public List<Shield> shields = new List<Shield>();
        public List<Accessory> accessories = new List<Accessory>();

        public static InventoryDatabase instance;


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
        }

        public Item GetItem(string name)
        {
            return items.Find(item => item.name.Equals(name));
        }

        public Armor GetHelmet(string name)
        {
            return helmets.Find(item => item.name.Equals(name));
        }

        public Armor GetChestArmor(string name)
        {
            return chestArmors.Find(item => item.name.Equals(name));
        }

        public Armor GetLegwear(string name)
        {
            return legwear.Find(item => item.name.Equals(name));
        }

        public Armor GetGauntlets(string name)
        {
            return gauntlets.Find(item => item.name.Equals(name));
        }

        public Weapon GetWeapon(string name)
        {
            return weapons.Find(item => item.name.Equals(name));
        }

        public Shield GetShield(string name)
        {
            return shields.Find(item => item.name.Equals(name));
        }

        public Accessory GetAccessory(string name)
        {
            return accessories.Find(item => item.name.Equals(name));
        }
    }

}
