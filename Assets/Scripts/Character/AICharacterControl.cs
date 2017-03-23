using System;
using UnityEngine;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(ThirdPersonCharacter))]
public class AICharacterControl : MonoBehaviour
{
    private enum AIState
    {
        Patrol = 0,
        Chase = 1,
        Attack = 2
    }

    public NavMeshAgent agent { get; private set; }                 // the navmesh agent required for the path finding
    public ThirdPersonCharacter character { get; private set; }     // the character we are controlling
    public ThirdPersonUserControl player { get; private set; }
    private AISight aiSight;

    [SerializeField]
    private AIState currentState;
    public bool inSight = false;
    public Transform movementTarget;                                // target to aim for
    public float sightDistance = 15f;
    public float attackDistance = 5f;

    public Transform[] waypoints;
    public int currentWaypoint;

    private void Start()
    {
        // get the components on the object we need ( should not be null due to require component so no need to check )
        agent = GetComponentInChildren<NavMeshAgent>();
        character = GetComponent<ThirdPersonCharacter>();
        player = FindObjectOfType<ThirdPersonUserControl>();

        agent.updateRotation = false;
        agent.updatePosition = true;

        currentState = AIState.Patrol;
        movementTarget = waypoints[currentWaypoint];

    }


    private void Update()
    {
        if (agent.remainingDistance > agent.stoppingDistance)
            character.Move(agent.desiredVelocity, false, false, false, Vector3.zero);
        else
            character.Move(Vector3.zero, false, false, false, Vector3.zero);

        // Can it see the player?



        // Perform state logic.
        switch (currentState)
        {
            case AIState.Patrol:
                // Waypointing
                Patrol();
                break;
            case AIState.Chase:
                Chase(movementTarget);
                break;
            case AIState.Attack:
                Attack(movementTarget);
                break;
            default:
                break;
        }

    }

    private void Patrol()
    {
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
            movementTarget = waypoints[currentWaypoint];
            agent.SetDestination(movementTarget.position);
            agent.Resume();
        }
    }

    public void Chase(Transform target)
    {
        var targetDistance = (target.position - transform.position).magnitude;

        if (inSight && targetDistance > attackDistance)
        {
            // Chase the player
            agent.SetDestination(movementTarget.position);
            agent.Resume();
        }
        else if (inSight && targetDistance < attackDistance)
        {
            // Close enough to attack
            currentState = AIState.Attack;
            // Do we stop? Don't know
            agent.Stop();

        }
        else
        {
            currentState = AIState.Patrol;
        }
    }

    public void Attack(Transform target)
    {

    }

    public void SetTarget(Transform target)
    {
        this.movementTarget = target;
    }

    public void SetInSight(bool value)
    {
        inSight = value;
        if (!value)
        {
            currentState = AIState.Patrol;
        }
    }
}

