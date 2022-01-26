using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AF
{

    public class EnemyMelee : Character
    {
        public GameObject shield;

        public ChaseState chaseState;

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

        public override void TakeDamage()
        {
            base.TakeDamage();

            SwitchToNextState(chaseState);
        }

    }

}
