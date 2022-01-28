using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AF { 
    public class Hitbox : MonoBehaviour
    {
        public EnemyMelee owner;

        public bool isPlayerWeapon = false;

        Character enemy;
        Player player;

        BoxCollider boxCollider => GetComponent<BoxCollider>();

        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
            boxCollider.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
        
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
                    enemy.TakeDamage(player.attackPower);
                }
            }
            else if (other.gameObject == player.gameObject) { 
                player.TakeDamage(owner.attackPower, owner);
            }

        }
    }
}
