using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.CrossPlatformInput
{
    [RequireComponent(typeof(Camera))]
    public class TiltCamera : MonoBehaviour
    {

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

        public Quaternion resetRotation;
        private Quaternion currentRotation;
        private Quaternion desiredRotation;

        // Use this for initialization
        void Start()
        {

            resetRotation = transform.localRotation;

            //xDeg = Vector3.Angle(Vector3.right, transform.right);
            //yDeg = Vector3.Angle(Vector3.up, transform.up);

            //be sure to grab the current rotations as starting points.
            currentRotation = transform.localRotation;
            desiredRotation = transform.localRotation;
        }

        // Update is called once per frame
        void Update()
        {
            // Eventually move this into some sort of overall UIInputManager class?
            // Seems wasteful on the CPU to use this many layered system.
            // Maybe write a much simpler one for android only? 
            // Might be faster, this is slower but it lets us use 1 system for all inputs. Pros and cons
            if (CrossPlatformInputManager.GetButton("Look"))
            {
                Tilt();
            }
            else if (CrossPlatformInputManager.GetButtonUp("Look"))
            {
                Reset();
            }
        }

        public void Tilt()
        {

            if (!tilting)
            {
                // Save our current rotation before doing anything else
                resetRotation = transform.localRotation;
                tilting = true;
            }

            if (Application.isEditor)
            {
                // Using mouse while android hates me.
                xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            }
            //else if (Application.isMobilePlatform)
            //{
            //    // This for android.
            //    xDeg += Input.acceleration.x * xSpeed * 0.02f;
            //    yDeg -= Input.acceleration.y * ySpeed * 0.02f;
            //    // will it need Input.acceleration.z ? Add it to the Quaternion.Euler() call
            //    zDeg += Input.acceleration.z * zSpeed * 0.02f; // +=, -=, = ?????
            //}

            //Clamp the vertical axis for the tilt
            yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);
            // set camera rotation, move from the safety of angles to the unknown black magic of quaternions
            desiredRotation = Quaternion.Euler(yDeg, xDeg, zDeg);
            currentRotation = transform.localRotation;

            // Apply the changes
            transform.localRotation = Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime); // * damping
        }

        public void Reset()
        {
            // Include anything special that needs to be reset separate to the coroutine.
            xDeg = 0;
            yDeg = 0;
            tilting = false;

            StartCoroutine("LerpCameraBack");
        }


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
                print(remainingAngle);
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
