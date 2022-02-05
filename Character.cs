using UnityEngine;
using System.Collections;

namespace AF
{

    [RequireComponent(typeof(Animator))]
    public class Character : MonoBehaviour
    {
        public string uuid;

        [Header("Stats")]
        public float health = 100;
        public float stamina = 60;

        [Header("Combat")]
        public GameObject weapon;
        [HideInInspector] public Hitbox weaponHitbox;
        public GameObject shield;
        public float impactOnHittinEnemyShield = 50000f;        

        [Header("Flags")]
        public bool isBlocking = false;
        public bool isRolling = false;
        public bool isDead = false;

        [Header("Sounds")]
        public float minPitchVariation = 0.8f;
        public float maxPitchVariation = 1.05f;
        public AudioSource combatAudiosource;
        public AudioSource footstepAudioSource;
        public AudioClip startBlockingSfx;
        public AudioClip blockKnockbackImpactSfx;
        public AudioClip clothSfx;
        public AudioClip weaponSwingSfx;
        public AudioClip damageSfx;
        public AudioClip dodgeSfx;
        public AudioClip dieSfx;
        public AudioClip groundImpactSfx;

        // Components
        [HideInInspector] public Animator animator => GetComponent<Animator>();
        [HideInInspector] public Rigidbody rigidbody => GetComponent<Rigidbody>();
        [HideInInspector] public CapsuleCollider capsuleCollider => GetComponent<CapsuleCollider>();

        // Ground
        float distanceToTheGround;

        private void Awake()
        {
            uuid = this.gameObject.name;
        }

        protected void Start()
        {
            if (shield != null)
            {
                shield.SetActive(false);
            }

            if (weapon != null)
            {
                weaponHitbox = weapon.GetComponent<Hitbox>();
            }

            distanceToTheGround = capsuleCollider.bounds.extents.y;
        }

        protected void Update()
        {
            isBlocking = animator.GetBool("isBlocking");
            isRolling = animator.GetBool("isRolling");
            isDead = animator.GetBool("isDead");

            if (shield != null)
            {
                shield.SetActive(isBlocking);
            }


            if (IsNotAvailable())
            {
                return;
            }

        }

        public virtual void TakeDamage(float amount, Character target)
        {

        }


        #region Physics

        public void FaceTarget(Transform target, float rotationSpeed)
        {
            var lookPos = target.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }

        public void ApplyKnockback(Vector3 targetPosition)
        {
            PlaySfx(combatAudiosource, blockKnockbackImpactSfx);


            Vector3 _moveDirection = transform.position - targetPosition;
            _moveDirection.y = 0;
            rigidbody.AddForce(_moveDirection.normalized * impactOnHittinEnemyShield);
        }
        #endregion

        #region Animation Events

        public void ActivateHitbox()
        {
            weaponHitbox.Enable();

            // Usually the weapon swing matches the hitbox activation
            PlaySfx(combatAudiosource, weaponSwingSfx);
        }

        public void DeactivateHitbox()
        {
            weaponHitbox.Disable();
        }

        public void PlayFootstep()
        {
            string groundTag = GetGroundTag();
            if (groundTag == null)
            {
                return;
            }

            AudioClip clip = FootstepSystem.instance.GetFootstepClip(groundTag);
            PlaySfx(footstepAudioSource, clip);
        }

        public void PlayDamage()
        {
            PlaySfx(combatAudiosource, damageSfx);
        }

        // Impact of the enemy when he hits the player's shield
        public void PlayParriedImpact()
        {
            PlaySfx(combatAudiosource, blockKnockbackImpactSfx);
        }

        // Impact of the enemy when receiving critical player attack that follows the parry
        public void PlayParryDamage()
        {
            PlaySfx(combatAudiosource, damageSfx);
        }

        // Swing of player weapon
        public void PlayParrySwing()
        {
            PlaySfx(combatAudiosource, weaponSwingSfx);
        }

        public void PlayDodge()
        {
            PlaySfx(combatAudiosource, dodgeSfx);

        }

        public void PlayDeath()
        {
            PlaySfx(combatAudiosource, dieSfx);
        }

        public void PlayGroundImpact()
        {
            PlaySfx(combatAudiosource, groundImpactSfx);
        }

        public void PlayCloth()
        {
            PlaySfx(combatAudiosource, clothSfx);
        }

        #endregion

        #region Sound
        public void PlaySfx(AudioSource audioSource, AudioClip clip)
        {
            float pitch = Random.Range(minPitchVariation, maxPitchVariation);
            audioSource.pitch = pitch;

            audioSource.PlayOneShot(clip);
        }
        #endregion


        #region Boolean Flags
        public bool IsNotAvailable()
        {
            return isDead || ParrySystem.instance.parryingOngoing || MainMenu.instance.isOpen;
        }

        public bool IsGrounded()
        {
            return Physics.Raycast(transform.position, -Vector3.up, distanceToTheGround + 0.1f);
        }
        #endregion

        public string GetGroundTag()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -Vector3.up, out hit))
            {
                if (hit.transform != null)
                {
                    return hit.transform.gameObject.tag;
                }
            }

            return null;
        }

        #region

        public void Revive(float amountOfHealth)
        {
            health = amountOfHealth;
            animator.SetBool("isDead", false);
            this.gameObject.SetActive(true);
        }

        #endregion

    }

}
