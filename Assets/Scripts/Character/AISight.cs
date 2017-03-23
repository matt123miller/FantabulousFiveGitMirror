using System;
using UnityEngine;
using System.Collections;


public class AISight : MonoBehaviour
{
    public AICharacterControl aiController;
    RaycastHit hit;
    private bool inSight;
    public float updateTarget = 0.2f;
    public float updateTimer = 0.0f;
    Vector3 targetPos;

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
            updateTimer += Time.deltaTime;
            if (updateTimer > updateTarget)
            {
                updateTimer = 0.0f;
                Look(other.transform.position);
                aiController.SetInSight(inSight);
            }
        }
    }

    public bool Look(Vector3 otherPosition)
    {
        Vector3 direction = otherPosition - eyes.position;

        hit = new RaycastHit();

        Debug.DrawRay(transform.position, direction, Color.green);
        if (Physics.Raycast(eyes.position, direction, out hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                inSight = true;
                Debug.Log("Player is Sighted, player LKP updated");
            }
        }
        else
        {
            inSight = false;
            Debug.Log("In collider, not in sight");
        }
        return inSight;
    }

    private void OnTriggerExit(Collider other)
    {
        if (inSight && other.CompareTag("Player"))
        {
            // if this ever occurs and the player IS in sight then there's a problem
            // if not delete this breakpoint
            // Breakpoint removed for now, might add it agian later to test.
            inSight = false;
            aiController.SetInSight(inSight);
        }
    }
}


