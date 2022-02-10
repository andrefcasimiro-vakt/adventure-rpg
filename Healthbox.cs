using UnityEngine;
using UnityEngine.UI;

namespace AF
{

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Inventory))]
    public class Healthbox : MonoBehaviour
    {
        public readonly int hashTakingDamage = Animator.StringToHash("TakingDamage");
        public readonly int hashDying = Animator.StringToHash("Dying");

        [Header("Stats")]
        public float health = 100f;

        [Header("UI")]
        public GameObject healthUI;
        public Image healthBarFillImage;

        Animator animator => GetComponent<Animator>();
        Inventory inventory => GetComponent<Inventory>();

        private void Update()
        {
            if (healthUI.activeSelf)
            {
                healthBarFillImage.fillAmount = health * 0.01f;
            }
        }

        public void TakeDamage(float amount, Transform attackTransform)
        {
            
            if (
                // If shield is visible
                inventory.shield.activeSelf
                // And enemy is facing weapon
                && Vector3.Angle(transform.forward * -1, attackTransform.forward) <= 90f
                )
            {
                return;
            }

            health -= amount;
            ObjectPooler.instance.SpawnFromPool("Blood", attackTransform.position, Quaternion.identity, 1f);

            if (health <= 0)
            {
                Die();
            }
            else
            {
                animator.SetTrigger(hashTakingDamage);
            }
        }

        public void Die()
        {
            animator.SetBool(hashDying, true);
        }

    }

}