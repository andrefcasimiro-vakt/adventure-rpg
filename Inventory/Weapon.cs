using UnityEngine;
using System.Collections;

namespace AF
{

    [RequireComponent(typeof(BoxCollider))]
    public class Weapon : MonoBehaviour
    {
        [Header("Stats")]
        public float attackPower = 30;
        public float criticalAttackPower = 75;

        // References
        BoxCollider boxCollider => GetComponent<BoxCollider>();
        Healthbox targetHealthbox;

        // Start is called before the first frame update
        void Start()
        {
            DisableHitbox();
        }

        public void EnableHitbox()
        {
            boxCollider.enabled = true;
        }

        public void DisableHitbox()
        {
            boxCollider.enabled = false;
        }


        public void OnTriggerEnter(Collider other)
        {
            other.TryGetComponent<Healthbox>(out targetHealthbox);

            if (targetHealthbox == null)
            {
                return;
            }

            targetHealthbox.TakeDamage(attackPower, this.transform);
        }

    }

}
