using UnityEngine;
using System.Collections;

namespace AF
{

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(InventoryManager))]
    public class CombatManager : MonoBehaviour
    {
        public AudioSource combatAudioSource;

        [Header("Combat Sounds")]
        public AudioClip weaponSwingSfx;
        public AudioClip dodgeSfx;
        public AudioClip groundImpactSfx;
        public AudioClip clothSfx;

        InventoryManager inventory => GetComponent<InventoryManager>();

        /// <summary>
        /// Animation Event
        /// </summary>
        public void ActivateHitbox()
        {
            // inventory.meleeWeapon.EnableHitbox();

            // Usually the weapon swing matches the hitbox activation
            Utils.PlaySfx(combatAudioSource, weaponSwingSfx);
        }

        /// <summary>
        /// Animation Event
        /// </summary>
        public void DeactivateHitbox()
        {
            // inventory.meleeWeapon.GetComponent<Weapon>().DisableHitbox();
        }

        /// <summary>
        /// Animation Event
        /// </summary>
        public void PlayGroundImpact()
        {
            Utils.PlaySfx(combatAudioSource, groundImpactSfx);
        }

        /// <summary>
        /// Animation Event
        /// </summary>
        public void PlayCloth()
        {
            Utils.PlaySfx(combatAudioSource, clothSfx);
        }

        /// <summary>
        /// Animation Event
        /// </summary>
        public void FireBow()
        {
            Player player = GameObject.FindWithTag("Player").GetComponent<Player>();

            Utils.FaceTarget(this.transform, player.transform);

            GameObject projectileInstance = Instantiate(inventory.projectilePrefab.gameObject, inventory.rangeWeapon.transform.position, Quaternion.identity);
            Projectile projectile = projectileInstance.GetComponent<Projectile>();
            Utils.PlaySfx(combatAudioSource, projectile.projectileSfx);
            projectile.Shoot(player.headTransform);
        }
    }

}