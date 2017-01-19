using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody))]
public class Tappable : TouchInteractive {

	public Color newColour = new Color(0,1,0);
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

    public override void FinishInteraction()
    {
        print("Finish tap");
    }
}
