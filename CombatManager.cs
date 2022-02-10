using UnityEngine;
using System.Collections;

namespace AF
{

    [RequireComponent(typeof(Inventory))]
    public class CombatManager : MonoBehaviour
    {
        public AudioSource combatAudioSource;

        [Header("Combat Sounds")]
        public AudioClip weaponSwingSfx;
        public AudioClip dodgeSfx;
        public AudioClip groundImpactSfx;
        public AudioClip clothSfx;

        Inventory inventory => GetComponent<Inventory>();

        /// <summary>
        /// Animation Event
        /// </summary>
        public void ActivateHitbox()
        {
            inventory.meleeWeapon.EnableHitbox();

            // Usually the weapon swing matches the hitbox activation
            PlaySfx(weaponSwingSfx);
        }

        /// <summary>
        /// Animation Event
        /// </summary>
        public void DeactivateHitbox()
        {
            inventory.meleeWeapon.GetComponent<Weapon>().DisableHitbox();
        }

        /// <summary>
        /// Animation Event
        /// </summary>
        public void PlayDodge()
        {
            PlaySfx(dodgeSfx);
        }


        /// <summary>
        /// Animation Event
        /// </summary>
        public void PlayGroundImpact()
        {
            PlaySfx(groundImpactSfx);
        }

        /// <summary>
        /// Animation Event
        /// </summary>
        public void PlayCloth()
        {
            PlaySfx(clothSfx);
        }

        /// <summary>
        /// Animation Event
        /// </summary>
        public void PlayProjectileSound()
        {
            Projectile projectile = inventory.projectilePrefab.GetComponent<Projectile>();
            PlaySfx(projectile.projectileSfx);
        }

        protected void PlaySfx(AudioClip clip)
        {
            float pitch = Random.Range(0.95f, 1.05f);
            combatAudioSource.pitch = pitch;

            combatAudioSource.PlayOneShot(clip);
        }

        /// <summary>
        /// Animation Event
        /// </summary>
        public void FireBow()
        {
            Player player = GameObject.FindWithTag("Player").GetComponent<Player>();

            Utils.FaceTarget(this.transform, player.transform);

            GameObject projectile = Instantiate(inventory.projectilePrefab.gameObject, inventory.rangeWeapon.transform.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().Shoot(player.headTransform);
        }
    }

}