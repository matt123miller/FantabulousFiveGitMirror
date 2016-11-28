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
        public float lerpMultiplier = 0.5f;
        public AnimationCurve curve;
        public bool tilting = false;
        //public Text debugText;
        
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

            // Remove for release
            //debugText = GameObject.Find("Rotation Debug").GetComponent<Text>();
           // debugText.enabled = false;

            // Will this simple fix solve the first tilt on mobile?
            //TiltPhone();
            //tilting = false;
            Input.gyro.enabled = true;
        }

        // Update is called once per frame
        void Update()
        {
            // Phone Input.
            if (CrossPlatformInputManager.GetButton(tiltString))
                TiltPhone();
            else if (CrossPlatformInputManager.GetButtonUp(tiltString))
                ResetRotation();
            
        }

        /// <summary>
        /// This rotates the camera to match the orientation of the phone
        /// </summary>
        public void TiltPhone()
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
                //debugText.enabled = true;
            }
            
            // None! This is 1 rotation offest by another. No idea how it works.
            // Why do you offset the right by the left? Who knows. It's magic.
            desiredRotation = negatePhoneRotation * DeviceRotation.GetRotation();

            // Set rotation at the end, assumes desiredRotation has been set in 1 of the above if statements.
            transform.localRotation = desiredRotation;
            // Cache it back into the conveniently shorter variable name.
            currentRotation = transform.localRotation;
            //debugText.text = desiredRotation.ToString();
        }

        public void ResetRotation()
        {
            // Include anything special that needs to be reset separate to the coroutine.
            tilting = false;
            //debugText.enabled = false;

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
            float lerpCompletion = 0;
            
            while (lerpCompletion < 0.99) 
            {
                lerpCompletion += Time.smoothDeltaTime * lerpMultiplier;
                var curveValue = curve.Evaluate(lerpCompletion);

                transform.localRotation = Quaternion.Slerp(currentRotation, desiredRotation, curveValue);

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