using UnityEngine;
using System.Collections;

namespace AF
{
    public class PatrolState : State
    {

        public ChaseState chaseState;
        public IdleState idleState;

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
            if (Vector3.Distance(character.agent.destination, character.agent.transform.position) < character.agent.stoppingDistance)
            {
                return idleState;
            }

            return this;
        }

    }
}