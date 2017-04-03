using System;
using UnityEngine;
using System.Collections;


public class AISight : MonoBehaviour
{
    public AICharacterControl aiController;
    RaycastHit _hit;

    [SerializeField]
    private bool _inSight;
    public float updateTarget = 0.1f;
    public float updateTimer = 0.0f;
    public Collider blockingcollider;

    [SerializeField]
    private Transform eyes;

    public Transform Eyes
    {
        get { return eyes; }
        set { eyes = value; }
    }

    private void Start()
    {
        blockingcollider = aiController.GetComponent<CapsuleCollider>();
        // Start each agents timer on a semi random time, why not? 
        // Should space out the bigger computations across different frames instead of all agents in 1 frame
        updateTimer = UnityEngine.Random.Range(-0.2f, 0);
    }

    private void Update()
    {
        if(updateTimer < 0) 
            Debug.DrawRay(eyes.position, (transform.position - eyes.position) * 2, Color.green);
    }
    
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var target = other.transform;
            var targetPos = target.position;

            targetPos.Set(targetPos.x, targetPos.y/* + 0.3f*/, targetPos.z); // I think I'm adding 0.3 to make sure I hit the players body? Probably unnecessary.
            var direction = targetPos - eyes.position;

            Debug.DrawRay(eyes.position, direction, Color.red);

            updateTimer += Time.deltaTime;
            if (updateTimer > updateTarget)
            {
                updateTimer = 0.0f;
                _inSight = Look(target, direction);
                aiController.SetInSight(_inSight, other.transform);
            }
        }
    }

    public bool Look(Transform target, Vector3 direction)
    {
        var spotted = false;
        _hit = new RaycastHit();
        blockingcollider.enabled = false;

        if (Physics.Raycast(eyes.position, direction, out _hit))
            print("Hit something" + _hit.transform.name);
            spotted = _hit.collider.CompareTag("Player");

        print(spotted ? "Player is sighted" : "In collider, not in sight");
        blockingcollider.enabled = true;

        return spotted;
    }

    private void OnTriggerExit(Collider other)
    {
        if (_inSight && other.CompareTag("Player"))
        {
            // The player ran away further than the AI can see
            _inSight = false;
            aiController.SetInSight(_inSight, other.transform);
            updateTimer = UnityEngine.Random.Range(-0.2f, 0); ;
        }
    }
}


