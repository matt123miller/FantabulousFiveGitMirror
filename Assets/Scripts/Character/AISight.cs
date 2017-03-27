using System;
using UnityEngine;
using System.Collections;


public class AISight : MonoBehaviour
{
    public AICharacterControl aiController;
    RaycastHit _hit;

    private bool _inSight;
    public float updateTarget = 0.1f;
    public float updateTimer = 0.0f;

    [SerializeField]
    private Transform eyes;

    public Transform Eyes
    {
        get { return eyes; }
        set { eyes = value; }
    }

    private void Start()
    {
        // Start each agents timer on a semi random time, why not? 
        // Should space out the bigger computations across different frames instead of all agents in 1 frame
        updateTimer += UnityEngine.Random.Range(-0.1f, 0.1f);
    }


    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var target = other.transform.position;
            target.Set(target.x, target.y + 0.3f, target.z);
            var direction =  target - eyes.position;

            Debug.DrawRay(eyes.position, direction, Color.red, 0.3f);

            updateTimer += Time.deltaTime;
            if (updateTimer > updateTarget)
            {
                updateTimer = 0.0f;
                _inSight = Look(target, direction);
                aiController.SetInSight(_inSight);
            }
        }
    }

    public bool Look(Vector3 otherPosition, Vector3 direction)
    {
        _hit = new RaycastHit();
        var spotted = false;

        if (Physics.Raycast(eyes.position, direction, out _hit))
            spotted = _hit.collider.CompareTag("Player");

        print(spotted ? "Player is sighted" : "In collider, not in sight");

        return spotted;
    }

    private void OnTriggerExit(Collider other)
    {
        if (_inSight && other.CompareTag("Player"))
        {
            // The player ran away further than the AI can see
            _inSight = false;
            aiController.SetInSight(_inSight);
        }
    }
}


