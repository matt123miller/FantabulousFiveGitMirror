using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;


    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        public ThirdPersonCharacter characterController { get; private set; }
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;      // the world-relative desired move direction, calculated from the camForward and user input.
        public bool isHanging;



        private void Start()
        {
            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            characterController = GetComponent<ThirdPersonCharacter>();
            isHanging = false;
        }


        private void Update()
        {
            // If we're not jumping...
            if (!m_Jump)
            {
                // Then request the jump status from CrossPlatformInputManager.
                // It may be true or false, who knows, it's used in FixedUpdate()
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            bool crouch = false;//Input.GetKey(KeyCode.C);

            CalculateAndMove(v, h, crouch);

            m_Jump = false;
        }

        public void CalculateAndMove(float v, float h, bool crouch)
        {
            m_Move = CalculateMoveVector(v, h);

            // pass all parameters to the character control script
            characterController.Move(m_Move, crouch, m_Jump, isHanging, m_Cam.right);
        }

        public Vector3 CalculateMoveVector(float v, float h)
        {
            Vector3 returnVector;
            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                returnVector = v * m_CamForward + h * m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                returnVector = v * Vector3.forward + h * Vector3.right;
            }
#if !MOBILE_INPUT
// walk speed multiplier
	        if (Input.GetKey(KeyCode.LeftShift)) returnVector *= 0.5f;
#endif
            return returnVector;
        }

        //gets the current movement value
        public float GetMoveMagnitude()
        {
            return m_Move.magnitude;
        }

    }

