using UnityEngine;
using System.Collections;
using UnityEngine.Analytics;

[RequireComponent(typeof(Animator))]
public class Checkpoint : MonoBehaviour
{
    public bool activated = false;
    public static GameObject[] checkpoints;
    private Animator _animator;
    private SaveLoad _saveLoadsScript;

    void Start()
    {
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        _animator = GetComponent<Animator>();
        _saveLoadsScript = GameObject.Find("GameManager").GetComponent<SaveLoad>();
    }

    void OnTriggerEnter(Collider other)
    {
        // If the player passes through the checkpoint, we activate it
        if (other.tag == "Player")
        {
            ActivateCheckPoint();
            _saveLoadsScript.Save(gameObject.transform.position);
            Debug.Log("Saved!");
            
        }
    }

    private void ActivateCheckPoint()
    {
        // We deactive all checkpoints in the scene
        foreach (GameObject cp in checkpoints)
        {
            cp.GetComponent<Checkpoint>().activated = false;
            //cp.GetComponent<Animator>().SetBool("Active", false);
        }

        // We activated this instance of the checkpoint, which would be the one just hit
        activated = true;
        //_animator.SetBool("Active", true);
    }

    public static Vector3 GetActiveCheckpointPosition()
    {
        // If player die without activate any checkpoint, we will return a default position
        var result = GameObject.FindWithTag("SpawnPoint").transform.position;

        foreach (var point in checkpoints)
        {
            if (point.GetComponent<Checkpoint>().activated)
            {
                result = point.transform.position;
                break;
            }
        }

        return result;
    }

    //overload for use when loading saved data
    public static Vector3 GetActiveCheckpointPosition(GameObject[] loadedCheckPoints)
    {
        // If player die without activate any checkpoint, we will return a default position
        var result = GameObject.FindWithTag("SpawnPoint").transform.position;

        foreach (var point in loadedCheckPoints)
        {
            if (point.GetComponent<Checkpoint>().activated)
            {
                result = point.transform.position;
                break;
            }
        }

        return result;
    }

    public override string ToString()
    {
        string playerprefValue = "";

        // What should it say?

        return playerprefValue;
    }
}
