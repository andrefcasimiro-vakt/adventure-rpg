using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AF {

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class Character : MonoBehaviour
    {
        [Header("Stats")]
        public int health = 100;
        public int stamina = 60;

        [Header("Senses")]
        public float rotationSpeed = 1.5f;
        public float sightDistance = 20f;

        [Header("Flags")]
        public bool isBusy;
        public bool isBlocking;
        public bool isTakingDamage;

        [Header("State")]
        public State currentState;
        public State nextState;

        // Components
        [HideInInspector] public Animator animator => GetComponent<Animator>();
        [HideInInspector] public NavMeshAgent agent => GetComponent<NavMeshAgent>();
        [HideInInspector] public CapsuleCollider capsuleCollider => GetComponent<CapsuleCollider>();
        [HideInInspector] public Player player;

        // Patrol
        [Header("Patrolling")]
        public Transform[] waypoints;
        public float restingTimeOnWaypoint = 2f;
        public int destinationPoint = 0;
        [HideInInspector] public float currentTimeOnWaypoint = 0f;

        protected void Start()
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
            agent.autoBraking = false;
        }

        protected void Update()
        {
            isBusy = animator.GetBool("isBusy");
            isBlocking = animator.GetBool("isBlocking");
            isTakingDamage = animator.GetBool("isTakingDamage");

            if (currentState != null)
            {
                State nextState = currentState.Tick(this);

                if (nextState != null)
                {
                    if (currentState != nextState)
                    {
                        nextState.OnEnter(this);
                    }

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

        public void FacePlayer(float rotationSpeed)
        {

            var lookPos = player.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

        }

        public virtual void TakeDamage()
        {
            if (isBlocking)
            {
                player.ApplyKnockback(this.transform.position);
                return;
            }

            PlayBusyAnimation("TakeDamage");
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
    }
}
