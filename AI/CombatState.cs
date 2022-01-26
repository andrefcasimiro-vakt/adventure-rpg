using UnityEngine;
using System.Collections;

namespace AF
{
    public class CombatState : State
    {
        public ChaseState chaseState;

        [Header("Block Settings")]
        public float minBlockChance = 0f;
        public float maxBlockChance = 1f;
        public float blockChance = 0.5f;

        public override void OnEnter(Character character)
        {
            if (!character.animator.GetBool("isTurning"))
            {
                character.animator.Play("Idle");
            }
        }

        public override State Tick(Character character)
        {
            if (character.isBusy)
            {
                return this;
            }

            character.FacePlayer(character.rotationSpeed);

            if (character.player.isAttacking)
            {
                return ResponseToPlayer(character);
            }

            if (IsPlayerFarAway(character))
            {
                return chaseState;
            }

            return this;
        }

        public State ResponseToPlayer(Character character)
        {
            if (character.player.isAttacking)
            {
                // Try block
                float blockDice = Random.Range(minBlockChance, maxBlockChance);
                if (blockDice >= blockChance)
                {
                    character.FacePlayer(100f);
                    character.PlayBusyAnimation("Block");
                    return this;
                }

                // Try dodge

                // Don't respond to player
                character.PlayBusyAnimation("NoAction");
                return this;
            }

            return this;
        }


        public bool IsPlayerFarAway(Character character)
        {
            return Vector3.Distance(transform.position, character.player.transform.position) > character.agent.stoppingDistance + character.capsuleCollider.radius;
        }

    }
}