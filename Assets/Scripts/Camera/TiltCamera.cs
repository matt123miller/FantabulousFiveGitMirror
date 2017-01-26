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
        
        private Quaternion _resetRotation;
        private Quaternion _negatePhoneRotation;
        private Quaternion _currentRotation;
        private Quaternion _desiredRotation;


        // Use this for initialization
        void Start()
        {
            // Grab the current rotations as starting points.
            _resetRotation = transform.localRotation;
            _currentRotation = transform.localRotation;
            _desiredRotation = transform.localRotation;
            

            // Will this simple fix solve the first tilt on mobile?
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
                _resetRotation = transform.localRotation;
                // This is the opposite of the phones rotation when entering the tilt mode. 
                // We are aiming to negate by this value later.
                _negatePhoneRotation = Quaternion.Inverse(DeviceRotation.GetRotation());
                tilting = true;
                //debugText.enabled = true;
            }
            
            // None! This is 1 rotation offest by another. No idea how it works.
            // Why do you offset the right by the left? Who knows. It's magic.
            _desiredRotation = _negatePhoneRotation * DeviceRotation.GetRotation();

            // Set rotation at the end, assumes _desiredRotation has been set in 1 of the above if statements.
            transform.localRotation = _desiredRotation;
            // Cache it back into the conveniently shorter variable name.
            _currentRotation = transform.localRotation;
            //debugText.text = _desiredRotation.ToString();
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
            _currentRotation = transform.localRotation;
            // We will return to this point. desiredRotaiton doesn't change throughout the whole coroutine.
            _desiredRotation = _resetRotation;
            float lerpCompletion = 0;
            
            while (lerpCompletion < 0.99) 
            {
                lerpCompletion += Time.smoothDeltaTime * lerpMultiplier;
                var curveValue = curve.Evaluate(lerpCompletion);

                transform.localRotation = Quaternion.Slerp(_currentRotation, _desiredRotation, curveValue);

                yield return null;
            }

            // The reset rotation has now finished, set any states and variables that need setting.

            // Sets the rotation back, compensate for any remaining angle changes.
            transform.localRotation = _desiredRotation;
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