using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.ThirdPerson;

public class WitchKeepStill : MonoBehaviour {

    private DeviceTilt deviceTilt;
    private Vector3 accelerometerTiltVal;
    private bool keepStillTriggered = false;
    private bool isStill = true;
    private Text witchPromptText;
    private Vector3 initialDeviceRotation;
    private Vector3 currentDeviceRotation;
    private ThirdPersonUserControl thirdPersonUserControl;

	// Use this for initialization
	void Start () {
        witchPromptText = GameObject.Find("Witch Prompt Text").GetComponent<Text>();
        deviceTilt = GameObject.Find("MechanicsScripts").GetComponent<DeviceTilt>();
        thirdPersonUserControl = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonUserControl>();
	}
	
	// Update is called once per frame
	void Update () {

        accelerometerTiltVal = deviceTilt.LowPassFilterAccelerometer();
        movementCheck(accelerometerTiltVal);
 
	}

    //timer for 10 seconds
    public IEnumerator KeepStillTime()
    {
        //guard boolean to make sure the movementCheck is only relevant for these 10 seconds
        keepStillTriggered = true;
        initialDeviceRotation = DeviceRotation.GetRotation().eulerAngles;

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
        currentDeviceRotation = DeviceRotation.GetRotation().eulerAngles;
        if (keepStillTriggered)
        {

            if (accelerometerCurrentVal.magnitude > 1f || hasRotationChanged(currentDeviceRotation) || thirdPersonUserControl.GetMoveValue() > 0)
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

    public bool hasRotationChanged(Vector3 rotation)
    {
        int rotBuffer = 10;

        if (currentDeviceRotation.x > (initialDeviceRotation.x + rotBuffer) || currentDeviceRotation.x < (initialDeviceRotation.x - rotBuffer)) 
        {
            return true;
        }
        else if (currentDeviceRotation.y > (initialDeviceRotation.y + rotBuffer) || currentDeviceRotation.y < (initialDeviceRotation.y - rotBuffer))
        {
            return true;
        }
        else if (currentDeviceRotation.z > (initialDeviceRotation.z + rotBuffer) || currentDeviceRotation.z < (initialDeviceRotation.z - rotBuffer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
