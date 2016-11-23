using UnityEngine;
using System.Collections;

// public interface ITouchInteractive
// {
// 	public void Interact(Vector3 fingerPosition);
// }


public abstract class TouchInteractive : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public abstract void Interact(Vector3 touchPosition);
}


