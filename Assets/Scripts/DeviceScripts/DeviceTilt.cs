using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class DeviceTilt : MonoBehaviour {

    private Vector3 accelerometerTilt;
	
	// Update is called once per frame
	void Update () {

      //  getTilt();
     //   Debug.Log(accelerometerTilt);     

    }

    public Vector3 getTilt()
    {
        return accelerometerTilt = Input.acceleration;
    }
}
