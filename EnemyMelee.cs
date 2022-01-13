using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMelee : MonoBehaviour
{

    [Header("Stats")]
    public int health = 100;
    public int stamina = 60;

    [Header("Flags")]
    public bool isSprinting = false;
    public bool isRolling = false;
    public bool isAttacking = false;
    public bool isGuarding = false;

    [Header("Patrol")]
    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent => GetComponent<NavMeshAgent>();

    private Animator animator => GetComponent<Animator>();

    private void Start()
    {
        agent.autoBraking = false;

        GotoNextPoint();
    }

    private void Update()
    {
        Patrol();
    }

    void Patrol()
    {
        animator.CrossFade("Walk", .2f);

        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }
        
    void GotoNextPoint()
    {
        if (points.Length == 0)
        {
            return;
        }

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }

}
