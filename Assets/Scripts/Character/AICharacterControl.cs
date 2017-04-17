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
    [SerializeField] [Range(1,10)]private float _braveryThreshold = 2.5f;
    [SerializeField] private bool _brave = true;
    [SerializeField] private float _braveryCooldownTarget = 4f;
    private float _hideFromWitchCooldownTarget = 10f;
    private float _selectedCooldownTarget;
    private float _hideTimer;
    private bool _hidingFromWitch = false;

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

        _noiseScript = GameObject.FindGameObjectWithTag("NoiseBar").GetComponent<Noise>();
        var mechanicsObj = GameObject.FindWithTag("MechanicsScripts");
        micInput = mechanicsObj.GetComponent<MicrophoneInput>();
        _hideFromWitchCooldownTarget = mechanicsObj.GetComponent<WitchKeepStill>().keepStillDuration;
        _witchScript = mechanicsObj.GetComponent<WitchKeepStill>();
       
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

        //if (_hidingFromWitch)
        //    return;

        var targetDistance = (_movementTarget - transform.position).magnitude;

        //if(_brave)
        //{
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
                    Hide(); // Counts down to become brave again
                    break;
                default:
                    break;
            }
        //}
        //else
        //{
        //    print("I am not brave");
            
        //}
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

    private void EnterHide(float hideFor)
    {
        _currentState = AIState.Hide;
        agent.speed = runSpeed;
        SetColour(Color.black);
        // I became scared;
        //_brave = false;
        _selectedCooldownTarget = hideFor;
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
        }
        else if (_inSight && targetdistance < attackDistance)
        {
            // Close enough to attack
            EnterAttack();
        }
        else
        {
            EnterPatrol();
        }
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
        if (volume >= _braveryThreshold)
        {
            EnterHide(hideFor: _braveryCooldownTarget);

            RunAway(fromthewitch: false);
            // Add to the witch noise mechanic.
            // Make sure the values are in line with the noise made from moving objects.
            _noiseScript.AddToNoise(volume * 10);
        }
    }

    private void RunAway(bool fromthewitch)
    {
        print("RUN AWAY!");
        // Run away. Pick a location behind you maybe and navmesh there?

        var direction = fromthewitch ? transform.right  * 10 : transform.forward * -10;
        Vector3 newTarget = transform.position + direction;
        print("I am at " + transform.position);
        print("Behind me is " + newTarget);
        SetTarget(newTarget);
        // navmesh bits
        agent.SetDestination(newTarget);
        agent.Resume();
    }

    private void Hide()
    {
        if (agent.remainingDistance <= 0.1f) // Ai has stopped moving to cower in fear
        {
            _hideTimer += Time.deltaTime;
            if (_hideTimer > _selectedCooldownTarget)
            {
                print("I BECAME BRAVE!");
                EnterPatrol();
                
                _hideTimer = 0;
            }
        }
    }

    // connect these to the witch with events
    public void WitchHasArrived()
    {
        print("Witch arrived! AHHH!");
        _aiSight.gameObject.SetActive(false);
        EnterHide(hideFor: _hideFromWitchCooldownTarget);
        RunAway(fromthewitch: true);
        //_hidingFromWitch = true;
    }

    public void WitchHasLeft()
    {
        print("Witch left! We're safe now");
        _aiSight.gameObject.SetActive(true);
        EnterPatrol();
        //_hidingFromWitch = false;
        //_brave = true;
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

