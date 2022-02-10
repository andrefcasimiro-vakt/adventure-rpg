using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AF
{

    public class Inventory : MonoBehaviour
    {

        [Header("Melee Weapon")]
        public Weapon meleeWeapon;

        [Header("Shield")]
        public GameObject shield;

        [Header("Range Weapon")]
        public GameObject rangeWeapon;
        public Projectile projectilePrefab;

        private void Start()
        {
            rangeWeapon.SetActive(false);
            shield.SetActive(false);
        }

    }

}
