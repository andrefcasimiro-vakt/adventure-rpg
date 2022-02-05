using UnityEngine;
using System.Collections;

namespace AF
{
    public class CombatState : State
    {
        public PatrolState patrolState;
        public ChaseState chaseState;

        [Header("Block Settings")]
        public float minBlockChance = 0f;
        public float maxBlockChance = 1f;
        public float blockChance = 0.5f;

        [Header("Dodge Settings")]
        public float minDodgeChange = 0f;
        public float maxDodgeChance = 1f;
        public float dodgeChance = 0.5f;

        public override State Tick(Enemy character)
        {
            if (character.player.isDead)
            {
                return patrolState;
            }

            if (character.isBusy)
            {
                return this;
            }

            if (character.isDead)
            {
                return this;
            }

            character.FaceTarget(character.player.transform, character.rotationSpeed);

            // if dead or parrying or main menu open
            if (character.IsNotAvailable())
            {
                return patrolState;
            }

            if (IsPlayerFarAway(character))
            {
                return chaseState;
            }

            character.animator.CrossFade("Idle", .05f);

            // Reactions

            if (character.player.isAttacking)
            {
                return ResponseToPlayer(character);
            }

            // Player Still Deciding, Take A Chance To Attack
            return AttackPlayer(character);
        }

        public State AttackPlayer(Enemy character)
        {
            float attackDice = Random.Range(0, 1f);

            if (attackDice > 0 && attackDice <= 0.25f)
            {
                character.PlayBusyAnimation("Attack1");
            } else if (attackDice > 0.25f && attackDice <= 0.5f)
            {
                character.PlayBusyAnimation("Attack2");
            } else if (attackDice > 0.5f && attackDice <= 0.85f)
            {
                character.PlayBusyAnimation("Attack3");
            } else
            {
                character.PlayBusyAnimation("NoAction");
            }

            return this;
        }

        public State ResponseToPlayer(Enemy character)
        {
            if (character.player.isAttacking)
            {
                // Try block
                float blockDice = Random.Range(minBlockChance, maxBlockChance);
                if (blockDice >= blockChance)
                {
                    character.FaceTarget(character.player.transform, 100f);
                    character.PlayBusyAnimation("Block");
                    return this;
                }

                // Try dodge
                float dodgeDice = Random.Range(minDodgeChange, maxDodgeChance);
                if (dodgeDice >= dodgeChance)
                {
                    character.FaceTarget(character.player.transform, 100f);

                    float dodgeClipDice = Random.Range(0f, 1f);
                    if (dodgeClipDice > 0.5f)
                    {
                        character.PlayBusyAnimation("Dodge1");
                    }
                    else
                    {
                        character.PlayBusyAnimation("Dodge2");
                    }
                    return this;
                }

                // Don't respond to player
                character.PlayBusyAnimation("NoAction");
                return this;
            }

            return this;
        }


        public bool IsPlayerFarAway(Enemy character)
        {
            return Vector3.Distance(character.agent.transform.position, character.player.transform.position) > character.agent.stoppingDistance + 0.5f;  
        }

    }
}