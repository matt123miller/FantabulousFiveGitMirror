using System;
using UnityEngine;

//TODO make floating a character component is probably better.
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(ThirdPersonCharacter))]
public class AICharacterControl : MonoBehaviour
{
    private enum AIState
    {
        Patrol = 0,
        Chase = 1,
        Attack = 2,
        Hide = 3
    }

    public NavMeshAgent agent { get; private set; }                 // the navmesh agent required for the path finding
    public ThirdPersonCharacter character { get; private set; }     // the character we are controlling
    public ThirdPersonUserControl player { get; private set; }
    private AISight _aiSight;
    public MicrophoneInput micInput;
    private Noise _noiseScript;
    private WitchKeepStill _witchScript;

    [SerializeField]
    private AIState _currentState;
    [SerializeField]
    private bool _inSight = false;
    [SerializeField]
    private Vector3 _movementTarget;                              // target to aim for

    [Range(0.1f, 0.5f)]
    public float walkSpeed = 0.4f;
    [Range(0.5f, 0.8f)]
    public float chaseSpeed = 0.5f;
    [Range(0.8f, 1.2f)]
    public float runSpeed = 1;
    [Tooltip("How close until the AI attacks?")]
    public float attackDistance = 5f;
    [Tooltip("How far can they see?")]
    public float sightDistance = 15f;
    [SerializeField] [Range(1,10)]private float braveryScore = 2.5f;
    [SerializeField] private bool brave = true;
    [SerializeField] private float braveryCooldownTarget = 2f;
    private float braveryCooldownTimer;
    private bool hidingFromWitch = false;

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
        micInput = GameObject.FindWithTag("GlobalGameManager").GetComponent<MicrophoneInput>();
        _noiseScript = GameObject.FindGameObjectWithTag("NoiseBar").GetComponent<Noise>();
        _witchScript = GameObject.FindWithTag("MechanicsObject").GetComponent<WitchKeepStill>();

        _aiSight.aiController = this;
    }

    private void Start()
    {
        _witchScript.witchArriveEvent += WitchHasArrived;
        _witchScript.witchLeaveEvent += WitchHasLeft;

        agent.updateRotation = false;
        agent.updatePosition = true;
        agent.speed = walkSpeed;

        _currentState = AIState.Patrol;
        _movementTarget = waypoints[currentWaypoint].position;

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

        if (hidingFromWitch)
            return;

        var targetDistance = (_movementTarget - transform.position).magnitude;

        if(brave)
        {
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
                    AssessFearLevel();
                    break;
                case AIState.Hide:
                    // Do nothing!
                    break;
                default:
                    break;
            }
        }
        else
        {
            print("I am not brave");
            braveryCooldownTimer += Time.deltaTime;
            if (braveryCooldownTimer > braveryCooldownTarget)
            {
                print("I BECAME BRAVE!");
                brave = true;
                braveryCooldownTimer = 0;
            }
        }
    }

    private void EnterPatrol()
    {
        _currentState = AIState.Patrol;
        agent.speed = walkSpeed;
        SetColour(Color.green);
        SetTarget(waypoints[currentWaypoint].position);
        agent.SetDestination(_movementTarget);
        agent.Resume();
    }

    private void EnterChase()
    {
        _currentState = AIState.Chase;
        agent.speed = chaseSpeed;
        SetColour(Color.yellow);
        micInput.StopInput();
    }

    private void EnterAttack()
    {
        _currentState = AIState.Attack;
        agent.speed = runSpeed;
        SetColour(Color.red);
        // Make sure the mic is accepting micInput
        micInput.StartInput();
    }

    private void EnterHide()
    {
        _currentState = AIState.Hide;
    }

    private void SetColour(Color colour)
    {
        transform.GetChild(0).GetComponent<Renderer>().material.color = colour;
    }


    private void Patrol(float targetdistance)
    {
        // Should we exit patrol state into chasing?
        if (_inSight && targetdistance < sightDistance)
        {
            // Close enough to chase
            EnterChase();
            // Do we stop? Don't know
            print("Close enough to Chase! Changing state!");
        }
        // If not lets just patrol more
        else if (agent.remainingDistance < agent.stoppingDistance)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
            _movementTarget = waypoints[currentWaypoint].position;
            agent.SetDestination(_movementTarget);
            agent.Resume();
        }
    }

    private void Chase(Vector3 target, float targetdistance)
    {
        if (_inSight && targetdistance > attackDistance)
        {
            // Chase the player
            agent.SetDestination(_movementTarget);
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

        //TODO create a timer for being in chase but not seeing the player, maybe I should introduce the last known position from my stealth game?
    }

    private void Attack(Vector3 target, float targetdistance)
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
            EnterChase();
        }
        // If everything is good them lets get 'em!
        else
        {
            print("Attacking");
            agent.Stop(); // Stop so we can do some attack animations?
            // Now what?
        }
    }

    private void AssessFearLevel() 
    {
        // Check the mic volume
        float volume = micInput.loudness;
        print(volume);
        // Is it enough to be scared?
        if (volume >= braveryScore)
        {
            RunAway();
            // I became scared;
            brave = false;

            // Add to the witch noise mechanic.
            // Make sure the values are in line with the noise made from moving objects.
            _noiseScript.AddToNoise(volume);
        }
    }

    private void RunAway()
    {
        print("RUN AWAY!");
        // Run away. Pick a location behind you maybe and navmesh there?
        Vector3 newTarget = transform.position + transform.forward * -10;
        print("I am at " + transform.position);
        print("Behind me is " + newTarget);
        SetTarget(newTarget);
        // navmesh bits
        agent.SetDestination(newTarget);
        agent.Resume();
    }

    // connect these to the witch with events
    public void WitchHasArrived()
    {
        print("Witch arrived! AHHH!");
        _aiSight.gameObject.SetActive(false);
        RunAway();

        hidingFromWitch = true;
    }

    public void WitchHasLeft()
    {
        print("Witch left! We're safe now");
        _aiSight.gameObject.SetActive(true);
        EnterPatrol();
        hidingFromWitch = false;
        brave = true;
    }

    public void SetTarget(Vector3 target)
    {
        _movementTarget = target;
    }

    public void SetInSight(bool value, Transform target)
    {
        _inSight = value;
        if (value)
        {
            SetTarget(target.position);
        }
    }

    void OnDestroy() // The destructor is called whenever the object is destroyed
    {
        // We make sure to unsubscribe our event listeners when this object is destroyed
        // If this isn't done and the event is invoked the pointer to our WitchHasArrived and WitchHasLeft
        // methods won't be pointing to anything and there'll be a problem.
        // Events are awesome, you just have to do a little tidying up here and there to safely use them.
        _witchScript.witchArriveEvent -= WitchHasArrived;
        _witchScript.witchLeaveEvent -= WitchHasLeft;
    }
}

