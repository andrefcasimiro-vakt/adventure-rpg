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

        int attackComboIndex = 0;


        Animator animator => GetComponent<Animator>();
        Player player => GetComponent<Player>();

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
                animator.CrossFade("Attack1", 0.05f);
            }
            else if (attackComboIndex == 1)
            {
                animator.CrossFade("Attack2", 0.05f);
            }
            else
            {
                animator.CrossFade("Attack3", 0.05f);
            }

            attackComboIndex++;
        }

        public void Guard()
        {
            if (player.IsNotAvailable() || player.isSprinting)
            {
                return;
            }

            animator.SetBool("isBlocking", true);
            animator.SetBool("isParrying", true);

            PlaySfx(startBlockingSfx);

            if (player.inventory.shield != null)
            {
                player.inventory.shield.SetActive(true);
            }
        }

        public void StopGuard()
        {
            animator.SetBool("isBlocking", false);

            if (player.inventory.shield != null)
            {
                player.inventory.shield.SetActive(false);
            }
        }


    }

}