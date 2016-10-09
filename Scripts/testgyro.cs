using UnityEngine;
using System.Collections;

public class testgyro : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Input.gyro.attitude;
	}
}
