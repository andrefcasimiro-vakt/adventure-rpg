using UnityEngine;
using System.Collections;

namespace AF
{

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Player))]
    public class PlayerCombatManager : CombatManager
    {
        [Header("Combat Sounds")]
        public AudioClip startBlockingSfx;

        Animator animator => GetComponent<Animator>();
        Player player => GetComponent<Player>();
        ParryManager parryManager => GetComponent<ParryManager>();

        int attackComboIndex = 0;

        public void HandleAttack()
        {
            if (player.IsNotAvailable() || player.isAttacking)
            {
                return;
            }

            if (player.isBlocking)
            {
                StopGuard();
            }

            if (attackComboIndex > 2)
            {
                attackComboIndex = 0;
            }

            if (attackComboIndex == 0)
            {
                animator.CrossFade(player.hashAttacking1, 0.05f);
            }
            else if (attackComboIndex == 1)
            {
                animator.CrossFade(player.hashAttacking2, 0.05f);
            }
            else
            {
                animator.CrossFade(player.hashAttacking3, 0.05f);
            }

            attackComboIndex++;
        }

        public void Guard()
        {
            if (player.IsNotAvailable() || player.isSprinting)
            {
                return;
            }

            animator.SetBool(player.hashBlocking, true);

            parryManager.EnableParrying();

            Utils.PlaySfx(combatAudioSource, startBlockingSfx);

            if (player.equipmentManager.GetShieldInstance() != null)
            {
                player.equipmentManager.GetShieldInstance().gameObject.SetActive(true);
            }
        }

        public void StopGuard()
        {
            animator.SetBool(player.hashBlocking, false);
            parryManager.DisableParrying();

            if (player.equipmentManager.GetShieldInstance() != null)
            {
                player.equipmentManager.GetShieldInstance().gameObject.SetActive(false);
            }
        }


    }

}