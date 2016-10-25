using UnityEngine;
using System.Collections;

public class StupidNextLevel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Load()
    {
        SceneTransitionManager.Instance.LoadTargetLevel(1);
    }
}
