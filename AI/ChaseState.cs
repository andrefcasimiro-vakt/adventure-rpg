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

        public override void OnEnter(Character character)
        {
        }

        public override State Tick(Character character)
        {

            character.FacePlayer(character.rotationSpeed);
            character.animator.Play("Run");
            character.agent.SetDestination(character.player.transform.position);

            if (Vector3.Distance(transform.position, character.player.transform.position) > maximumChaseDistance)
            {
                return patrolState;
            }

            if (character.agent.remainingDistance <= 1)
            {
                return combatState;
            }

            return this;
        }
    }
}