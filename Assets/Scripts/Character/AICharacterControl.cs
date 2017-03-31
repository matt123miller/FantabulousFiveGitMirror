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
    private AISight _aiSight;

    [SerializeField]
    private AIState _currentState;
    [SerializeField]
    private bool _inSight = false;
    [SerializeField]
    private Transform _movementTarget;                                // target to aim for
    [Range(0.1f, 0.5f)]
    public float walkSpeed = 0.4f;
    [Range(0.5f, 0.8f)]
    public float chaseSpeed = 0.5f;
    [Tooltip("Max value of 1 is advised")]
    [Range(1,1)]
    public float runSpeed = 1;
    [Tooltip("How close until the AI attacks?")]
    public float attackDistance = 5f;
    [Tooltip("How far can they see?")]
    public float sightDistance = 15f;

    [Header("Waypointing system")]
    public Transform[] waypoints;
    public int currentWaypoint;

    private void Awake()
    {
        // get the components on the object we need ( should not be null due to require component so no need to check )
        agent = GetComponentInChildren<NavMeshAgent>();
        character = GetComponent<ThirdPersonCharacter>();
        player = FindObjectOfType<ThirdPersonUserControl>();
        _aiSight = GetComponentInChildren<AISight>();
        _aiSight.aiController = this;
    }

    private void Start()
    {

        agent.updateRotation = false;
        agent.updatePosition = true;
        agent.speed = walkSpeed;

        _currentState = AIState.Patrol;
        _movementTarget = waypoints[currentWaypoint];

        // Give the agent a path before anything else happens
        EnterPatrol();
    }


    private void Update()
    {
        // Makes the ThirdPersonCharacter magically work.
        if (agent.remainingDistance > agent.stoppingDistance)
            character.Move(agent.desiredVelocity, false, false, false, Vector3.zero);
        else
            character.Move(Vector3.zero, false, false, false, Vector3.zero);
        

        var targetDistance = (_movementTarget.position - transform.position).magnitude;

        // Perform state logic.
        switch (_currentState)
        {
            case AIState.Patrol:
                // Waypointing
                Patrol(targetDistance);
                break;
            case AIState.Chase:
                Chase(_movementTarget, targetDistance);
                break;
            case AIState.Attack:
                Attack(_movementTarget, targetDistance);
                break;
            default:
                break;
        }

    }

    private void EnterPatrol()
    {
        _currentState = AIState.Patrol;
        agent.speed = walkSpeed;
        SetTarget(waypoints[currentWaypoint]);
        agent.SetDestination(_movementTarget.position);
        agent.Resume();
    }

    private void EnterChase()
    {
        _currentState = AIState.Chase;
        agent.speed = chaseSpeed;
    }

    private void EnterAttack()
    {
        _currentState = AIState.Attack;
        agent.speed = runSpeed;
    }

    private void Patrol(float targetdistance)
    {
        // Should we exit patrol state into chasing?
        if (_inSight && targetdistance < sightDistance)
        {
            // Close enough to chase
            EnterChase();
            // Do we stop? Don't know
            print("Close enough ot Chase! Changing state!");
        }
        // If not lets just patrol more
        else if (agent.remainingDistance < agent.stoppingDistance)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
            _movementTarget = waypoints[currentWaypoint];
            agent.SetDestination(_movementTarget.position);
            agent.Resume();
        }
    }

    public void Chase(Transform target, float targetdistance)
    {
        if (_inSight && targetdistance > attackDistance)
        {
            // Chase the player
            agent.SetDestination(_movementTarget.position);
            agent.Resume();
            print("Chasing!");
        }
        else if (_inSight && targetdistance < attackDistance)
        {
            // Close enough to attack
            EnterAttack();
            // Do we stop? Don't know
            print("Close enough to Attack! Changing state!");

        }
        else
        {
            print("The player escaped! Changing to Patrol!");
            EnterPatrol();
        }
    }

    public void Attack(Transform target, float targetdistance)
    {
        // Can we see the player?
        if (!_inSight)
        {
            EnterChase();
            // Do we need to do anything about knowing WHERE to chase them? Or will it all gracefully follow through?
            // We'll just let chase decide.
        }
        // Is the player running away? But I definitely can see them
        else if (targetdistance > attackDistance)
        {
            _currentState = AIState.Chase;
        }
        // If everything is good them lets get 'em!
        else
        {
            print("Attacking");
            agent.Stop(); // Stop so we can do some attack animations?
            // Now what?
        }
    }

    public void SetTarget(Transform target)
    {
        this._movementTarget = target;
    }

    public void SetInSight(bool value, Transform target)
    {
        _inSight = value;
        if (value)
        {
            SetTarget(target);
        }
        else
        {
            EnterPatrol();
        }
    }
}

