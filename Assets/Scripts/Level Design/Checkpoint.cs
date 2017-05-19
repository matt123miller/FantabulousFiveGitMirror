using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class Checkpoint : MonoBehaviour
{
    public bool activated = false;
    public static GameObject[] checkpoints;
    private PlayerData playerData;
    private Animator _animator;
    private SaveLoad _saveLoadsScript;
    private Noise noiseScript;

    void Start()
    { 
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint").OrderBy(go => go.name).ToArray();
        playerData = GameObject.Find("PlayerData").GetComponent<PlayerData>();
        _animator = GetComponent<Animator>();
        _saveLoadsScript = FindObjectOfType<SaveLoad>();
        noiseScript = GameObject.Find("NoiseBar").GetComponent<Noise>();
    }

    void OnTriggerEnter(Collider other)
    {
        // If the player passes through the checkpoint, we activate it
        if (other.tag == "Player" && gameObject.transform.position.ToString() != playerData.CheckPoint)
        {
            ActivateCheckPoint();
            playerData.CheckPoint = gameObject.transform.position.ToString();
            playerData.SceneToLoad = SceneManager.GetActiveScene().buildIndex;
            playerData.NoiseAmount = noiseScript.currentNoise;
            _saveLoadsScript.Save();
            Debug.Log("Saved Noise:" + playerData.NoiseAmount);
            
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
        DisablePriorCheckPoints(gameObject);
    }
    
    private void DisablePriorCheckPoints(GameObject _checkPoint)
    {
        foreach(GameObject cp in checkpoints)
        {
            if (cp.name == _checkPoint.name)
            {
                break;
            }
            else
            {
                cp.SetActive(false);
            }
        }
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
