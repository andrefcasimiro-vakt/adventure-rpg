using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AF { 
    public class Hitbox : MonoBehaviour
    {
        public Character weaponOwner;
        public bool isPlayerWeapon = false;

        public float weaponDamage = 30;
        public float weaponCriticalDamage = 75;

        Character enemy;
        Player player;
        BoxCollider boxCollider => GetComponent<BoxCollider>();

        public AudioClip weaponImpactSfx;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }

        // Start is called before the first frame update
        void Start()
        {
            Disable();
        }

        public void Enable() {
            boxCollider.enabled = true;
        }

        public void Disable()
        {
            boxCollider.enabled = false;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (isPlayerWeapon)
            {
                other.gameObject.TryGetComponent(out enemy);

                if (enemy != null)
                {
                    enemy.combatAudiosource.PlayOneShot(weaponImpactSfx);
                    enemy.TakeDamage(weaponDamage, player);
                }
            }
            else if (other.gameObject == player.gameObject)
            {
                if (player.isParrying) return;

                player.combatAudiosource.PlayOneShot(weaponImpactSfx);
                player.TakeDamage(weaponDamage, weaponOwner);
            }

        }
    }
}
