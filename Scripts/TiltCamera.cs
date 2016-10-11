using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UnityStandardAssets.CrossPlatformInput
{
    [RequireComponent(typeof(Camera))]
    public class TiltCamera : MonoBehaviour
    {
        // This class is applied to the camera, which is a child of the character controller
        // As such all rotations are localRotation, in relation to the parent object (character controller);
        [Tooltip("The speed the camera will return back to normal, higher is quicker."), Range(0.1f, 1)]
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
        private Quaternion negatePhoneRotation;
        private Quaternion currentRotation;
        private Quaternion desiredRotation;

        public Text debugText;

        // Use this for initialization
        void Start()
        {
            gyro = Input.gyro;
            gyro.enabled = true;

            // Grab the current rotations as starting points.
            resetRotation = transform.localRotation;
            currentRotation = transform.localRotation;
            desiredRotation = transform.localRotation;

            debugText.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            // Phone Input.
            if (CrossPlatformInputManager.GetButton("Look"))
                Tilt();
            else if (CrossPlatformInputManager.GetButtonUp("Look"))
                ResetRotation();
            
        }

        public void Tilt()
        {
            // Helps to stop us getting caught in a bad change of states. Resets in ResetRotation().
            if (!tilting)
            {
                // Save our current rotation before doing anything else. This is where we'll return later.
                resetRotation = transform.localRotation;
                // This is the opposite of the phones rotation when entering the tilt mode. 
                // We are aiming to negate by this value later.
                negatePhoneRotation = Quaternion.Inverse(DeviceRotation.GetRotation());
                tilting = true;
                debugText.enabled = true;
            }

            if (Application.isEditor)
            {
                // Using mouse while android hates me.
                xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                // Clamp the vertical axis for the tilt.
                yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);

                // move from the safety of angles to the unknown black magic of quaternions.
                var lerpTo = Quaternion.Euler(yDeg, xDeg, zDeg);
                // Apply the changes
                desiredRotation = Quaternion.Lerp(currentRotation, lerpTo, Time.deltaTime);
            }
            else if (Application.isMobilePlatform)// This for android.
            {
                // what magic quaternion method do we use?
                //desiredRotation = Quaternion.FromToRotation(DeviceRotation.GetRotation().eulerAngles, negatePhoneRotation.eulerAngles);// 1 should be instant?
                //desiredRotation = Quaternion.Euler(v3);
                //desiredRotation = Quaternion.Slerp(ALL THE THINGS);

                // None! This is 1 rotation offest by another. No idea how it works.
                // Why do you offset the right by the left? Who knows. It's magic.
                desiredRotation = negatePhoneRotation * DeviceRotation.GetRotation();

                // Maybe slerp?
            }

            // Set rotation at the end, assumes desiredRotation has been set in 1 of the above if statements.
            transform.localRotation = desiredRotation;
            // Cache it back into the conveniently shorter variable name.
            currentRotation = transform.localRotation;
        }

        public void ResetRotation()
        {
            // Include anything special that needs to be reset separate to the coroutine.
            xDeg = 0;
            yDeg = 0;
            zDeg = 0;
            tilting = false;
            debugText.enabled = false;

            StartCoroutine("LerpCameraBack");
        }

        // This works perfectly in all my test so far.
        private IEnumerator LerpCameraBack()
        {
            // How do I find the return angle and rotate back to normal? So scared.
            // Quaternions are the devil
            currentRotation = transform.localRotation;
            // We will return to this point. desiredRotaiton doesn't change throughout the whole coroutine.
            desiredRotation = resetRotation;

            // This gives the euclidian (360 degrees type) angle between the 2 quaternions.
            // Like most of this geometry stuff it's Angle(from, to)
            float remainingAngle = Quaternion.Angle(transform.localRotation, desiredRotation);
            float lerpTime = 0;

            while (remainingAngle > 1) // 1 degree of rotation
            {
                lerpTime += Time.smoothDeltaTime;
                transform.localRotation = Quaternion.Slerp(transform.localRotation, desiredRotation, lerpTime * damping);
                // What is the new remaining angle? This will be evaluated in the while condition
                remainingAngle = Quaternion.Angle(transform.localRotation, desiredRotation);

                yield return null;
            }
            // The reset rotation has now finished, set any states and variables that need setting.

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