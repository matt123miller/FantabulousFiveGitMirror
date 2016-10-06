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

    private float xDeg = 0.0f;
    private float yDeg = 0.0f;
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

    // Update is called once per frame
    void Update ()
    {
        // Eventually move into it's own method to call on button hold.
        if (Input.GetMouseButton(1))
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

            rotation = Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime); // * damping
            transform.rotation = rotation;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            desiredRotation = initialRotation;
            StartCoroutine("LerpCameraBack");
        }
	}

    private IEnumerator LerpCameraBack() {

        // How do I find the return angle back to normal? So scared`

        currentRotation = transform.localRotation;
        float remainingAngle = Quaternion.Angle(currentRotation, desiredRotation);

        while (remainingAngle < 1)
        {
            rotation = Quaternion.Slerp(currentRotation, desiredRotation, Time.deltaTime); //  Time.deltaTime * damping
            transform.localRotation = rotation;
            yield return null;
        }


    }

    private static float ClampAngle(float angle, float min, float max)
    {
        // The equivalent of angle % 360 though I don't think that works with negative numbers.
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
