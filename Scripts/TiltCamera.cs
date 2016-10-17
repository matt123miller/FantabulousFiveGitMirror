using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UnityStandardAssets.CrossPlatformInput
{
    [RequireComponent(typeof(Camera))]
    public class TiltCamera : MonoBehaviour
    {
        
        [SerializeField]
        string tiltString;
        // This class is applied to the camera, which is a child of the character controller
        // As such all rotations are localRotation, in relation to the parent object (character controller);
        [Tooltip("The speed the camera will return back to normal, higher is quicker."), Range(0.1f, 1)]
        public float damping = 0.5f;
        public bool tilting = false;
        public Text debugText;
        
        [Header("Only used for PC input")]
        private float xSpeed = 100.0f;
        private float ySpeed = 100.0f;
        private float zSpeed = 100.0f;
        // I feel like this will be useful, to help control players view. Maybe pointless though, then remove the Clamp()
        private int yMinLimit = -20;
        private int yMaxLimit = 80;
        private float xDeg = 0.0f;
        private float yDeg = 0.0f;
        private float zDeg = 0.0f;

        private Quaternion resetRotation;
        private Quaternion negatePhoneRotation;
        private Quaternion currentRotation;
        private Quaternion desiredRotation;


        // Use this for initialization
        void Start()
        {
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
            if (CrossPlatformInputManager.GetButton(tiltString))
                Tilt();
            else if (CrossPlatformInputManager.GetButtonUp(tiltString))
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
            
            // None! This is 1 rotation offest by another. No idea how it works.
            // Why do you offset the right by the left? Who knows. It's magic.
            desiredRotation = negatePhoneRotation * DeviceRotation.GetRotation();

            // Set rotation at the end, assumes desiredRotation has been set in 1 of the above if statements.
            transform.localRotation = desiredRotation;
            // Cache it back into the conveniently shorter variable name.
            currentRotation = transform.localRotation;
            debugText.text = desiredRotation.ToString();
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