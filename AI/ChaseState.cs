using UnityEngine;
using System.Collections;

namespace AF
{
    public class ChaseState : State
    {
        public CombatState combatState;
        public PatrolState patrolState;

        [Header("Chase Settings")]
        public float maximumChaseDistance = 20f;

        public override State Tick(Character character)
        {
            if (character.player.isDead)
            {
                return patrolState;
            }

            if (character.isBusy)
            {
                return this;
            }

            character.FacePlayer(character.rotationSpeed);
            character.animator.Play("Run");
            character.agent.SetDestination(character.player.transform.position);

            if (Vector3.Distance(character.agent.transform.position, character.player.transform.position) > maximumChaseDistance)
            {
                return patrolState;
            }

            if (Vector3.Distance(character.agent.transform.position, character.player.transform.position) <= character.agent.stoppingDistance)
            {
                return combatState;
            }


            return this;
        }
    }
}
