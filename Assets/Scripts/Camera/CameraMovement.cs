using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;

public class CameraMovement : MonoBehaviour {

    private GameObject character;
    public GameObject characterCam;
    private Rigidbody characterRigidbody;
    public GameObject idleCameraPos;
    public GameObject travellingCameraPos;
    private Vector3 hangingCameraPos;
    public float lerpTime = 0.5f;
    private ThirdPersonUserControl thirdPersonUserControl;
    
    // Use this for initialization
    void Awake()
    {
        character = GameObject.Find("ThirdPersonController");
        // characterCam = GameObject.FindWithTag("MainCamera");
        characterRigidbody = character.GetComponent<Rigidbody>();
        thirdPersonUserControl = character.GetComponent<ThirdPersonUserControl>();

    }
    // Update is called once per frame
    void Update()
    {

        float characterVelocity = characterRigidbody.velocity.normalized.magnitude;
        Debug.Log(characterVelocity);

        //changes camera to 'travelling' position - far out
        if (characterVelocity > 0 && characterCam.transform.localPosition != travellingCameraPos.transform.localPosition)
        {
            lerpCameraBetweenPos(characterCam.transform, travellingCameraPos.transform, lerpTime);
        }
        //changed camera to 'idle' position - close up
        else if (characterVelocity <= 0 && characterCam.transform.localPosition != idleCameraPos.transform.localPosition && !thirdPersonUserControl.isHanging)
        {
            lerpCameraBetweenPos(characterCam.transform, idleCameraPos.transform, lerpTime);
        }
        //changes camera to 'travelling' position only if hanging on a ledge
        else if(thirdPersonUserControl.isHanging)
        {
            lerpCameraBetweenPos(characterCam.transform, travellingCameraPos.transform, lerpTime);
        }
    }

    void lerpCameraBetweenPos(Transform originalPos, Transform newPos, float time)
    {
       characterCam.transform.localPosition =  Vector3.Lerp(originalPos.transform.localPosition, newPos.transform.localPosition, Time.deltaTime * time);
       characterCam.transform.localRotation =  Quaternion.Lerp(originalPos.localRotation, newPos.localRotation, Time.deltaTime * time);
 
    }
}
