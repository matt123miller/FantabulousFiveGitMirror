using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class CameraMovement : MonoBehaviour {

    private ThirdPersonUserControl _thirdPersonUserControl;
    private RopeBalance _balance;
	private Joystick _joystick;
    private GameObject _character;
    private Rigidbody _characterRigidbody;
    private Vector3 _hangingCameraPos;

    public GameObject characterCam;
    public GameObject idleCameraPos;
    public GameObject travellingCameraPos;
    public GameObject balancingCameraPos;
    public float lerpTime = 0.5f;

    // Use this for initialization
    void Awake()
    {
        _character = GameObject.FindWithTag("Player");
        // characterCam = GameObject.FindWithTag("MainCamera");
        _characterRigidbody = _character.GetComponent<Rigidbody>();
        _thirdPersonUserControl = _character.GetComponent<ThirdPersonUserControl>();
        _balance = FindObjectOfType<RopeBalance>();

    }

	void Start() {

		_joystick = FindObjectOfType<Joystick> ();
		if (_joystick == null) {
			Destroy (this);
		}
	}
    // Update is called once per frame
    void Update()
    {
		// This is the old way, relying on player speed to power the lerping.2
        //float characterVelocity = _characterRigidbody.velocity.normalized.magnitude;

		float characterVelocity = _joystick.movementAmount;

        // Change the camera to the 'balancing' position - far out and above
        if (_balance.isBalancing)
        {
			lerpCameraBetweenPos(idleCameraPos.transform, balancingCameraPos.transform, characterVelocity);
        }
        //changes camera to 'travelling' position - far out
        else if (characterVelocity > 0.1 && characterCam.transform.localPosition != travellingCameraPos.transform.localPosition)
        {
			lerpCameraBetweenPos(idleCameraPos.transform, travellingCameraPos.transform, characterVelocity);
        }
        //changed camera to 'idle' position - close up
        else if (characterVelocity <= 0.1 && characterCam.transform.localPosition != idleCameraPos.transform.localPosition && !_thirdPersonUserControl.isHanging)
        {
			lerpCameraBetweenPos(idleCameraPos.transform, characterCam.transform, characterVelocity);
        }
        //changes camera to 'travelling' position only if hanging on a ledge
        else if(_thirdPersonUserControl.isHanging)
        {
			lerpCameraBetweenPos(idleCameraPos.transform, travellingCameraPos.transform, characterVelocity);
        }

        print("Velocity:" + characterVelocity);
    }

	/*
    void lerpCameraBetweenPos(Transform originalPos, Transform newPos, float time)
    {
       characterCam.transform.localPosition =  Vector3.Lerp(originalPos.transform.localPosition, newPos.transform.localPosition, Time.deltaTime * time);
       characterCam.transform.localRotation =  Quaternion.Lerp(originalPos.localRotation, newPos.localRotation, Time.deltaTime * time);
    }
*/

	void lerpCameraBetweenPos(Transform originalPos, Transform newPos, float lerpAmount)
	{
		characterCam.transform.localPosition =  Vector3.Lerp(originalPos.transform.localPosition, newPos.transform.localPosition, lerpAmount);
		characterCam.transform.localRotation =  Quaternion.Lerp(originalPos.localRotation, newPos.localRotation, lerpAmount);
	}
}
