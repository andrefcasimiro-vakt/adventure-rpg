using UnityEngine;
using System.Collections;

namespace AF { 
    public class IdleState : State
    {
        public PatrolState patrolState;
        public ChaseState chaseState;

        public override State Tick(Character character)
        {
            character.animator.CrossFade("Idle", .2f);

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
