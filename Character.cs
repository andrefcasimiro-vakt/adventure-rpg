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
        public float health = 100;
        public float stamina = 60;

        [Header("Combat")]
        public Hitbox weaponHitbox;
        public GameObject shield;
        public State stateTriggeredWhenSeeingPlayer;

        public Transform parryPosition;
        public Transform parryPositionBloodFx;


        [Header("Senses")]
        public float rotationSpeed = 1.5f;
        public float sightDistance = 20f;

        [Header("Flags")]
        public bool isBusy;
        public bool isBlocking;
        public bool isTakingDamage;
        public bool isRolling;
        public bool isDead;
        public bool receivingParryDamage;

        [Header("State")]
        public State currentState;
        public State nextState;

        // Components
        [HideInInspector] public Animator animator => GetComponent<Animator>();
        [HideInInspector] public NavMeshAgent agent => GetComponent<NavMeshAgent>();
        [HideInInspector] public CapsuleCollider capsuleCollider => GetComponent<CapsuleCollider>();
        [HideInInspector] public Rigidbody rigidbody => GetComponent<Rigidbody>();
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

            if (shield != null)
            {
                shield.SetActive(false);
            }
        }

        protected void Update()
        {
            isBusy = animator.GetBool("isBusy");
            isBlocking = animator.GetBool("isBlocking");
            isTakingDamage = animator.GetBool("isTakingDamage");
            isRolling = animator.GetBool("isRolling");
            isDead = animator.GetBool("isDead");
            receivingParryDamage = animator.GetBool("receivingParryDamage");

            if (shield != null)
            {
                shield.SetActive(isBlocking);
            }

            if (IsNotAvailable())
            {
                return;
            }

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

        public void FacePlayer(float rotationSpeed)
        {

            var lookPos = player.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
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

        public virtual void TakeDamage(float amount)
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

            PlayBusyAnimation("TakeDamage");
            ObjectPooler.instance.SpawnFromPool("Blood", player.weaponGameObject.transform.position, Quaternion.identity, 1f);

            health -= amount;

            if (health <= 0) {

                Die();
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

            Destroy(this.gameObject);
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

        public void ApplyKnockback(Vector3 enemyPosition)
        {
            Vector3 _moveDirection = transform.position - enemyPosition;
            _moveDirection.y = 0;
            rigidbody.AddForce(_moveDirection.normalized * player.impactOnHittinEnemyShield);
        }

        public bool IsNotAvailable()
        {
            return isDead || ParrySystem.instance.parryingOngoing;
        }

        public void ActivateHitbox()
        {
            player.isParrying = false;

            weaponHitbox.Enable();
        }
        public void DeactivateHitbox()
        {
            weaponHitbox.Disable();
        }

        public void DisableParry()
        {
            Debug.Log("Disable parry");

            if (player.isParrying)
            {
                Debug.Log("Player was parrying!");

                ParrySystem.instance.Dispatch(player, this);
            }

            player.isParrying = false;
        }
    }
}
