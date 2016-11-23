using UnityEngine;
using System.Collections;

public class StupidLevelChange : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            print("Changing level");
            SceneTransitionManager.Instance.LoadNextLevel();
        }
    }
}
