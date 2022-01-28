using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AF
{

    public class EnemyMelee : Character
    {
        public Hitbox weaponHitbox;
        public GameObject shield;

        public ChaseState chaseState;

        public Transform parryPosition;

        private void Start()
        {
            base.Start();

            shield.SetActive(false);
        }

        protected void Update()
        {
            shield.SetActive(isBlocking);

            base.Update();
        }

        public override void TakeDamage(float amount)
        {
            base.TakeDamage(amount);

            SwitchToNextState(chaseState);
        }


        #region
        /// <summary>
        /// Animation Event Function. Handles the hitbox activation
        /// </summary>
        public void ActivateHitbox()
        {
            player.isParrying = false;

            weaponHitbox.Enable();
        }
        public void DeactivateHitbox()
        {
            weaponHitbox.Disable();
        }

        public void DisableParry()
        {
            Debug.Log("Disable parry");

            if (player.isParrying)
            {
                Debug.Log("Player was parrying!");

                ParrySystem.instance.Dispatch(player, this);
            }

            player.isParrying = false;
        }
        #endregion

    }

}
