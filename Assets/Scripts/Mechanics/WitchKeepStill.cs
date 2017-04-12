using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;


public class WitchKeepStill : MonoBehaviour
{

    public delegate void WitchArrivingDelegate();
    public event WitchArrivingDelegate witchArriveEvent;
    public delegate void WitchLeaveDelegate();
    public event WitchLeaveDelegate witchLeaveEvent;

    private DeviceTilt _deviceTilt;
    private Vector3 _accelerometerTiltVal;
    private bool keepStillTriggered = false;
    private bool isStill = true;
    private Text witchPromptText;
    private Vector3 initialDeviceRotation;
    private Vector3 currentDeviceRotation;
    private ThirdPersonUserControl thirdPersonUserControl;
    private LoadLevel loadLevelScript;


	// Use this for initialization
	void Start () {
        witchPromptText = GameObject.Find("Witch Prompt Text").GetComponent<Text>();
        _deviceTilt = GameObject.Find("MechanicsScripts").GetComponent<DeviceTilt>();
        thirdPersonUserControl = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonUserControl>();
        loadLevelScript = GameObject.Find("GameManager").GetComponent<LoadLevel>();
	}
	
	// Update is called once per frame
	void Update () {

        _accelerometerTiltVal = _deviceTilt.LowPassFilterAccelerometer();
        movementCheck(_accelerometerTiltVal);
 
	}

    //timer for 10 seconds
    public IEnumerator KeepStillTime()
    {
        //guard boolean to make sure the movementCheck is only relevant for these 10 seconds
        keepStillTriggered = true;
        initialDeviceRotation = DeviceRotation.GetRotation().eulerAngles;

        // Notify the AI that they need to run away and whatnot
        if (witchArriveEvent != null) witchArriveEvent();

        for (int i = 10; i >= 1; i--)
        {
            //if we moved, stop the timer
            if(!isStill)
            {
                yield return new WaitForSecondsRealtime(2.5f);
                loadLevelScript.LoadSavedLevel(true);
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
        if (witchLeaveEvent != null) witchLeaveEvent();
        reset();
    }


    //checks magnitude of the acceleromter value - detecting any shake
    public void movementCheck(Vector3 accelerometerCurrentVal)
    {
        currentDeviceRotation = DeviceRotation.GetRotation().eulerAngles;
        if (keepStillTriggered)
        {
            if (accelerometerCurrentVal.magnitude > 1f || hasRotationChanged(currentDeviceRotation) || thirdPersonUserControl.GetMoveMagnitude() > 0)
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
