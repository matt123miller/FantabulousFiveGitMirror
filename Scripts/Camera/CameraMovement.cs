using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    private GameObject character;
    public GameObject characterCam;
    private Rigidbody characterRigidbody;
    public GameObject idleCameraPos;
    public GameObject travellingCameraPos;
    private Vector3 hangingCameraPos;
    public float lerpTime = 0.5f;
    
    // Use this for initialization
    void Awake()
    {
        character = GameObject.Find("ThirdPersonController");
        // characterCam = GameObject.FindWithTag("MainCamera");
        characterRigidbody = character.GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {

        float characterVelocity = characterRigidbody.velocity.normalized.magnitude;
        Debug.Log(characterVelocity);

        if (characterVelocity > 0 && characterCam.transform.localPosition != travellingCameraPos.transform.localPosition)
        {
            lerpCameraBetweenPos(characterCam.transform, travellingCameraPos.transform, lerpTime);
        }
        else if (characterVelocity <= 0 && characterCam.transform.localPosition != idleCameraPos.transform.localPosition)
        {
            lerpCameraBetweenPos(characterCam.transform, idleCameraPos.transform, lerpTime);
        }

    }

    void lerpCameraBetweenPos(Transform originalPos, Transform newPos, float time)
    {
       characterCam.transform.localPosition =  Vector3.Lerp(originalPos.transform.localPosition, newPos.transform.localPosition, Time.deltaTime * time);
     //  characterCam.transform.rotation =  Quaternion.Lerp(originalPos.localRotation, newPos.localRotation, Time.deltaTime * time);
    }
}
