using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AF {

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : Character
    {
        [Header("Combat")]
        public State stateTriggeredWhenSeeingPlayer;
        public Transform parryPosition;
        public Transform parryPositionBloodFx;

        [Header("Senses")]
        public float rotationSpeed = 1.5f;
        public float sightDistance = 20f;

        [Header("Flags")]
        public bool isBusy;
        public bool isTakingDamage;
        public bool receivingParryDamage;

        [Header("State")]
        public State currentState;
        public State nextState;

        // Components
        [HideInInspector] public NavMeshAgent agent => GetComponent<NavMeshAgent>();
        [HideInInspector] public Player player;

        // Patrol
        [Header("Patrolling")]
        public Transform[] waypoints;
        public float restingTimeOnWaypoint = 2f;
        public int destinationPoint = 0;
        [HideInInspector] public float currentTimeOnWaypoint = 0f;


        protected void Start()
        {
            base.Start();

            player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }

        protected void Update()
        {
            base.Update();

            isBusy = animator.GetBool("isBusy");
            isTakingDamage = animator.GetBool("isTakingDamage");
            receivingParryDamage = animator.GetBool("receivingParryDamage");

            if (currentState != null)
            {
                State nextState = currentState.Tick(this);

                if (nextState != null)
                {
                    SwitchToNextState(nextState);
                }
            }
        }

        public void SwitchToNextState(State state)
        {
            currentState = state;
        }

        public bool PlayerIsSighted()
        {
            if (Vector3.Distance(transform.position, player.transform.position) > sightDistance)
            {
                return false;
            }

            Vector3 playerDirection = transform.position - player.transform.position;
            float angle = Vector3.Angle(transform.forward, playerDirection);

            if (Mathf.Abs(angle) > 90 && Mathf.Abs(angle) < 270)
            {
                return true;
            }

            return false;
        }


        public void TakeParryDamage(float amount, Vector3 bloodFxPos)
        {
            ObjectPooler.instance.SpawnFromPool("Blood", bloodFxPos, Quaternion.identity, 1f);

            health -= amount;

            if (health <= 0)
            {

                DieAndCancelParrySystem();

            }
        }

        public override void TakeDamage(float amount, Character character)
        {
            if (IsNotAvailable())
            { 
                return;
            }

            if (isBlocking)
            {
                player.ApplyKnockback(this.transform.position);
                PlayBusyAnimation("BlockHit");
                return;
            }

            health -= amount;
            ObjectPooler.instance.SpawnFromPool("Blood", player.weapon.transform.position, Quaternion.identity, 1f);

            if (health <= 0) {

                Die();
            }
            else
            {
                PlayBusyAnimation("TakeDamage");
                PlayDamage();
            }
        }

        public void Die()
        {
            PlayBusyAnimation("Die");
            StartCoroutine(Destroy());
        }

        public void DieAndCancelParrySystem()
        {
            PlayBusyAnimation("DieAndCancelParrySystem");
            StartCoroutine(Destroy());
        }

        IEnumerator Destroy()
        {
            yield return new WaitForSeconds(3);

            this.gameObject.SetActive(false);
        }

        public void PlayBusyAnimation(string animationName)
        {
            animator.CrossFade(animationName, 0.05f);
            animator.SetBool("isBusy", true);
        }

        public void GotoNextPoint()
        {
            currentTimeOnWaypoint = 0;

            // Set the agent to go to the currently selected destination.
            agent.destination = waypoints[destinationPoint].position;

            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            destinationPoint = (destinationPoint + 1) % waypoints.Length;
        }

        #region Animation Events
        public void ActivateHitbox()
        {
            player.animator.SetBool("isParrying", false);

            base.ActivateHitbox();
        }

        public void DisableParry()
        {
            if (player.isParrying)
            {
                ParrySystem.instance.Dispatch(player, this);
            }
        }
        #endregion
    }
}
