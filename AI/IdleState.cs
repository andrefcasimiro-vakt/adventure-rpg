using UnityEngine;
using System.Collections;

namespace AF { 
    public class IdleState : State
    {
        public PatrolState patrolState;
        public ChaseState chaseState;

        public override void OnEnter(Character character)
        {
            if (!character.animator.GetBool("isTurning"))
            {
                character.animator.Play("Idle");
            }
        }


        public override State Tick(Character character)
        {
            character.currentTimeOnWaypoint += Time.deltaTime;

            if (character.PlayerIsSighted())
            {
                return chaseState;
            }

            if (character.currentTimeOnWaypoint > character.restingTimeOnWaypoint)
            {
                character.GotoNextPoint();
                return patrolState;
            }

            return this;
        }
    }
}
