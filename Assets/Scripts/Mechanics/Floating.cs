using UnityEngine;
using System.Collections;

public class Floating : MonoBehaviour {

    GameObject playerObj;
    Rigidbody playerRigid;
    MicrophoneInput microphoneInputScript;
    bool isFloating;

	// Use this for initialization
	void Start () {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        playerRigid = playerObj.GetComponent<Rigidbody>();
        microphoneInputScript = GameObject.Find("GameManager").GetComponent<MicrophoneInput>();
	}
	
	// Update is called once per frame
	void Update () {
	
        if(isFloating)
        {
            playerRigid.AddRelativeForce(0, microphoneInputScript.loudness, 0);
        }
	}

    public void FloatingTriggered()
    {
        isFloating = true;
        microphoneInputScript.StartInput();
    }

    public void TurnOffFloating()
    {
        isFloating = false;
        microphoneInputScript.StopInput();
    }
}
