using UnityEngine;
using UnityEngine.UI;

namespace AF
{

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(InventoryManager))]
    public class Healthbox : MonoBehaviour
    {
        public readonly int hashTakingDamage = Animator.StringToHash("TakingDamage");
        public readonly int hashDying = Animator.StringToHash("Dying");

        [Header("Stats")]
        public float health = 100f;

        [Header("UI")]
        public GameObject healthUI;
        public Image healthBarFillImage;

        [Header("Sounds")]
        public AudioSource combatAudioSource;
        public AudioClip damageSfx;
        public AudioClip deathGruntSfx;

        Character character => GetComponent<Character>();
        Animator animator => GetComponent<Animator>();
        InventoryManager inventory => GetComponent<InventoryManager>();

        private void Update()
        {
            healthBarFillImage.fillAmount = health * 0.01f;
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

            if (character.IsDodging())
            {
                return;
            }

            health -= amount;
            ObjectPooler.instance.SpawnFromPool("Blood", attackTransform.position, Quaternion.identity, 1f);

            Utils.PlaySfx(combatAudioSource, damageSfx);
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

        /// <summary>
        /// Animation Event
        /// </summary>
        public void PlayDeathGrunt()
        {
            Utils.PlaySfx(combatAudioSource, deathGruntSfx);
        }
    }

}