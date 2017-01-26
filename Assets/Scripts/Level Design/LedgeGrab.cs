using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.ThirdPerson;

public class LedgeGrab : MonoBehaviour
{

    private GameObject _character = null;
    private bool _onLedge = false;
    private GameObject _ledgeCollider = null;
    private GameObject _ledgeParent = null;
    private GameObject _mobileJoystick = null;
    private Joystick _joystickScript = null;
    private Rigidbody _characterRigidbody = null;
    private ThirdPersonUserControl _thirdPersonUserControl = null;
    public bool automaticClimbUp = false;

    // Use this for initialization
    void Awake()
    {

        _ledgeCollider = gameObject.transform.parent.gameObject;
        _ledgeParent = _ledgeCollider.gameObject.transform.parent.gameObject;
   
    }

    // Update is called once per frame
    void Update()
    {
        //if on ledge = true and player presses jump, teleport to top of ledge
        if (_onLedge && CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            MoveCharacterToTop();
            ResetCharacter();
        }
    }

    void OnTriggerEnter(Collider collider)
    {

        if (collider.CompareTag("Player"))
        {
            _joystickScript = GameObject.FindWithTag("UICanvas").GetComponentInChildren<Joystick>();
            _character = collider.gameObject;
            _characterRigidbody = _character.GetComponent<Rigidbody>();
            _thirdPersonUserControl = _character.GetComponent<ThirdPersonUserControl>();
        }

        if (_character != null && _thirdPersonUserControl.characterController.m_LastObjectGroundedOn != _ledgeParent.name
            && _thirdPersonUserControl.characterController.m_LastObjectGroundedOn != this.gameObject.name)
        {
            if (!_thirdPersonUserControl.characterController.m_IsGrounded)
            {
                GrabLedge();
            }
            else
            {
                GrabLedgeGrounded();
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        ResetCharacter();
        _onLedge = false;
        _thirdPersonUserControl.isHanging = false;
    }


    //the aim is to have a ledge pull up animation
    //stop the character
    //stop the player input
    //play animation
    //give back input
    public void GrabLedge()
    {
        FreezeCharacter();
        LookAtLedge(_ledgeCollider.transform);
        _onLedge = true;
    }

    public void GrabLedgeGrounded()
    {
        LookAtLedge(_ledgeCollider.transform);
        _onLedge = true;
        
        if(automaticClimbUp)
        {
            MoveCharacterToTop();
        }
    }

    public void FreezeCharacter()
    {
        _characterRigidbody.velocity = Vector3.zero;
        _characterRigidbody.useGravity = false;
        _characterRigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        _joystickScript.SetAxis(Joystick.AxisOption.OnlyHorizontal);
        _thirdPersonUserControl.isHanging = true;
    }

    public void ResetCharacter()
    {
        _characterRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        _joystickScript.SetAxis(Joystick.AxisOption.Both);
        _character.GetComponent<ThirdPersonUserControl>().enabled = true;
        _characterRigidbody.useGravity = true;
    }

    public void LookAtLedge(Transform ledgeTransform)
    {
        // You can just do this, interpolates between the rotations by time, use 1 for instant.
        _character.transform.rotation = Quaternion.Slerp(_character.transform.rotation, transform.rotation, 1);
    }

    public void MoveCharacterToTop()
    {
        //stops player from making further input
        _thirdPersonUserControl.enabled = false;
        ////teleports character to top of the box
        _character.transform.position = new Vector3(_ledgeCollider.transform.position.x, _ledgeCollider.transform.position.y + 0.5f, _ledgeCollider.transform.position.z);
    }


}