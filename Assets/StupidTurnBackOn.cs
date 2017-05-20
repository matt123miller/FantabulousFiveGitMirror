using UnityEngine;
using System.Collections;

public class StupidTurnBackOn : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    private void OnTriggerEnter(Collider other)
    {
        transform.parent.gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        transform.parent.gameObject.GetComponent<BoxCollider>().enabled = true;
    }
}
