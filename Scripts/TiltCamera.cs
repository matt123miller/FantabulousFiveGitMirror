using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class TiltCamera : MonoBehaviour {

    public float damping = 0.5f;
    public float xSpeed = 100.0f;
    public float ySpeed = 100.0f;

    // I feel like this will be useful, to help control players view. Maybe pointless though, then remove the Clamp()
    public int yMinLimit = -20;
    public int yMaxLimit = 80;

    public Quaternion initialRotation;
    public Transform resetTarget;

    public float xDeg = 0.0f;
    public float yDeg = 0.0f;
    private Quaternion currentRotation;
    private Quaternion desiredRotation;
    private Quaternion rotation;
    private Vector3 position;

    // Use this for initialization
    void Start () {

        initialRotation = transform.rotation;

        xDeg = Vector3.Angle(Vector3.right, transform.right);
        yDeg = Vector3.Angle(Vector3.up, transform.up);

        //be sure to grab the current rotations as starting points.
        position = transform.position;
        rotation = transform.rotation;
        currentRotation = transform.rotation;
        desiredRotation = transform.rotation;
    }

    public void Tilt()
    {
        // This for android.
        //xDeg += Input.acceleration.x * xSpeed * 0.02f;
        //yDeg -= Input.acceleration.y * ySpeed * 0.02f;

        // Using mouse while android hates me.
        xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
        yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

        //OrbitAngle

        //Clamp the vertical axis for the orbit
        yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);
        // set camera rotation
        desiredRotation = Quaternion.Euler(yDeg, xDeg, 0);
        currentRotation = transform.rotation;

        // Apply the changes
        transform.rotation = Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime); // * damping
    }

    public void Reset()
    {
        // Include anything special that needs to be reset separate to the coroutine.
     
        StartCoroutine("LerpCameraBack");
    }

    // Update is called once per frame
    void Update ()
    {
        // Eventually move this into some sort of overall UIInputManager class?
        if (Input.GetMouseButton(1))
        {
            Tilt();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            Reset();
        }
	}

    private IEnumerator LerpCameraBack() {

        // How do I find the return angle and rotate back to normal? So scared.
        //transform.rotation.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, desiredRotation, Time.deltaTime);
        currentRotation = transform.rotation;
        desiredRotation = initialRotation;

        float remainingAngle = Quaternion.Angle(transform.rotation, desiredRotation);
        float lerpTime = 0;

        while (remainingAngle > 1)
        {
            print(remainingAngle);
            lerpTime += Time.smoothDeltaTime;

            rotation = Quaternion.Slerp(transform.rotation, desiredRotation, lerpTime * damping); //  Time.deltaTime * damping
            transform.rotation = rotation;

            remainingAngle = Quaternion.Angle(transform.rotation, desiredRotation);

            yield return null;
        }

        print("LerpCameraBack finished, hopefully it's correct!");
        
        // Compensate for any remaining angle changes
        //transform.LookAt(resetTarget);
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        // The equivalent of angle % 360 though I don't think that works with negative numbers. Hence this.
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
