using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WitchKeepStill : MonoBehaviour {

    private DeviceTilt deviceTilt;
    private Vector3 accelerometerTiltVal;
    private bool keepStillTriggered = false;
    private bool isStill = true;
    private Text witchPromptText; 

	// Use this for initialization
	void Start () {
        witchPromptText = GameObject.Find("Witch Prompt Text").GetComponent<Text>();
        deviceTilt = GameObject.Find("DeviceTiltTest").GetComponent<DeviceTilt>();
	}
	
	// Update is called once per frame
	void Update () {

        accelerometerTiltVal= deviceTilt.getTilt();
        movementCheck(accelerometerTiltVal);

	}

    //timer for 10 seconds
    public IEnumerator KeepStillTime()
    {
        //guard boolean to make sure the movementCheck is only relevant for these 10 seconds
        keepStillTriggered = true;
        for (int i = 10; i >= 1; i--)
        {
            //if we moved, stop the timer
            if(!isStill)
            {
                break;
            }

            yield return new WaitForSecondsRealtime(1);
        }
        keepStillTriggered = false;

        //if we stayed still for the full time, prompt user and then clear text
        if(isStill)
        {
            witchPromptText.text = "WELL DONE!";

            yield return new WaitForSecondsRealtime(1);
        }

        witchPromptText.text = " ";
        reset();
    }

    //checks magnitude of the acceleromter value - detecting any shake
    public void movementCheck(Vector3 accelerometerCurrentVal)
    {
        if (keepStillTriggered)
        {
            Debug.Log(accelerometerCurrentVal.magnitude);
            if (Input.acceleration.magnitude > 1.01f)
            {
                witchPromptText.text = "YOU MOVED - GAME OVER";
                isStill = false;
            }
        }
    }

    public void reset()
    {
        keepStillTriggered = false;
        isStill = true;
    }
}
