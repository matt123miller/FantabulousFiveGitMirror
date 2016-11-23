using UnityEngine;
using System.Collections;

public class Tappable : TouchInteractive {

	public Color newColour;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void Interact(Vector3 fingerPosition){

		print("Tap that");
		GetComponent<MeshRenderer>().material.color = newColour;
	}
}
