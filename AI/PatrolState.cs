using UnityEngine;
using System.Collections;

namespace AF
{
    public class PatrolState : State
    {

        public ChaseState chaseState;
        public IdleState idleState;

        public override void OnEnter(Character character)
        {
        }

        public override State Tick(Character character)
        {
            if (character.PlayerIsSighted())
            {
                return chaseState;
            }

            character.animator.Play("Walk");
            transform.LookAt(character.agent.destination);

            // Choose the next destination point when the agent gets
            // close to the current one.
            if (!character.agent.pathPending && character.agent.remainingDistance < 0.5f)
            {
                return idleState;
            }

            return this;
        }

    }
}