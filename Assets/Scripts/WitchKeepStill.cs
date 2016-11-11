using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WitchKeepStill : MonoBehaviour {

    DeviceTilt deviceTilt;
    Vector3 accelerometerTiltVal;
    bool keepStillTriggered = false;
    bool isStill = true;

    Text witchPromptText; 

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

    public IEnumerator KeepStillTime()
    {
        keepStillTriggered = true;
        for (int i = 10; i >= 0; i--)
        {
            yield return new WaitForSecondsRealtime(1);
        }
        keepStillTriggered = false;

        if(isStill)
        {
            witchPromptText.text = "WELL DONE!";

            yield return new WaitForSecondsRealtime(1);

            witchPromptText.text = " ";
        }
    }

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
}
