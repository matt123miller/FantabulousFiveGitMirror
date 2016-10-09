using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.CrossPlatformInput
{
    [RequireComponent(typeof(Camera))]
    public class TiltCamera : MonoBehaviour
    {
        // This class is applied to the camera, which is a child of the character controller
        // As such all rotations are localRotation, in relation to the parent object (character controller);
        public float damping = 0.5f;
        public float xSpeed = 100.0f;
        public float ySpeed = 100.0f;
        public float zSpeed = 100.0f;

        // I feel like this will be useful, to help control players view. Maybe pointless though, then remove the Clamp()
        public int yMinLimit = -20;
        public int yMaxLimit = 80;
        public float xDeg = 0.0f;
        public float yDeg = 0.0f;
        public float zDeg = 0.0f;

        public bool tilting = false;
        public Gyroscope gyro;

        private Quaternion resetRotation;
        private Quaternion phoneResetRotation;
        private Quaternion currentRotation;
        private Quaternion desiredRotation;

        public GameObject debugText;

        // Use this for initialization
        void Start()
        {
            gyro = Input.gyro;
            gyro.enabled = true;

            // Grab the current rotations as starting points.
            phoneResetRotation = DeviceRotation.GetRotation();
            resetRotation = transform.localRotation;
            currentRotation = transform.localRotation;
            desiredRotation = transform.localRotation;

            debugText.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            // Eventually move this stuff into some sort of overall UIInputManager class?

            // Seems wasteful on the CPU to use the many layered CrossPlatformInputManager system.
            // Maybe write a much simpler one for android only? 
            // Might be faster, this is slower but it lets us use 1 system for all inputs. Pros and cons

            // Phone Input.
            if (CrossPlatformInputManager.GetButton("Look"))
                Tilt();
            else if (CrossPlatformInputManager.GetButtonUp("Look"))
                Reset();

            // PC input.
            if (Input.GetMouseButton(1))
                Tilt();
            else if (Input.GetMouseButtonUp(1))
                Reset();
        }

        public void Tilt()
        {
            // Helps to stop us getting caught in a bad change of states. Reset at the end of LerpCameraBack().
            if (!tilting)
            {
                // Save our current rotation before doing anything else.
                resetRotation = transform.localRotation;
                tilting = true;
            }

            if (Application.isEditor)
            {
                // Using mouse while android hates me.
                xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                // Clamp the vertical axis for the tilt.
                yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);

                // Set camera rotation, move from the safety of angles to the unknown black magic of quaternions.
                currentRotation = transform.localRotation;
                var lerpTo = Quaternion.Euler(yDeg, xDeg, zDeg);
                // Apply the changes
                desiredRotation = Quaternion.Lerp(currentRotation, lerpTo, Time.deltaTime);
            }
            else if (Application.isMobilePlatform)// This for android.
            {
                //xDeg += Input.acceleration.x * xSpeed * 0.02f;
                //yDeg -= Input.acceleration.y * ySpeed * 0.02f;
                // will it need Input.acceleration.z ? Add it to the Quaternion.Euler() call
                //zDeg += Input.acceleration.z * zSpeed * 0.02f; // +=, -=, = ????? No idea.

                var v3 = gyro.attitude.eulerAngles;
                v3 = DeviceRotation.GetRotation().eulerAngles;
                // Do we have to always do a variety of these depending on phone orientation?
                // Is gyro.attitude based on upright phone orientation? Takes phone orientation into account?
                //v3.x += 90;
                //v3.y += 90;
                //v3.z += 90;
                //v3.x -= 90;
                v3.y -= 90;
                //v3.z -= 90;
                desiredRotation = Quaternion.Euler(v3);

               
                //xDeg = gyro.attitude.x;
                //yDeg = gyro.attitude.y;
                //zDeg = gyro.attitude.z;
                //desiredRotation = Quaternion.Euler(xDeg, yDeg, zDeg);

            
            }

            //// Set camera rotation, move from the safety of angles to the unknown black magic of quaternions.
            //currentRotation = transform.localRotation;
            //// Apply the changes
            //desiredRotation = Quaternion.Lerp(currentRotation, Quaternion.Euler(yDeg, xDeg, zDeg), Time.deltaTime);


            // Set rotation at the end, assumes desiredRotation has been set in 1 of the above if statements.
            transform.localRotation = desiredRotation;
            //transform.Rotate(0, -90, 0, Space.Self); // cheap hack because I don't understand quaternions much.
        }

        public void Reset()
        {
            // Include anything special that needs to be reset separate to the coroutine.
            xDeg = 0;
            yDeg = 0;
            tilting = false;

            StartCoroutine("LerpCameraBack");
        }

        // This works perfectly in all my test so far.
        private IEnumerator LerpCameraBack()
        {
            // How do I find the return angle and rotate back to normal? So scared.
            // Quaternions are the devil
            currentRotation = transform.localRotation;
            desiredRotation = resetRotation;

            float remainingAngle = Quaternion.Angle(transform.localRotation, desiredRotation);
            float lerpTime = 0;

            while (remainingAngle > 1) // 1 degree of rotation
            {
                lerpTime += Time.smoothDeltaTime;
                transform.localRotation = Quaternion.Slerp(transform.localRotation, desiredRotation, lerpTime * damping);
                remainingAngle = Quaternion.Angle(transform.localRotation, desiredRotation);

                yield return null;
            }

            // Sets the rotation back, compensate for any remaining angle changes.
            transform.localRotation = desiredRotation;
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
}