using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class LedgeGrab : MonoBehaviour
{

    private GameObject character = null;
    private bool onLedge = false;
    private GameObject ledgeCollider = null;
    private GameObject ledgeParent = null;
    private GameObject mobileJoystick = null;
    private Joystick joystickScript = null;
    private Rigidbody characterRigidbody = null;

    // Use this for initialization
    void Start()
    {

        ledgeParent = gameObject.transform.parent.gameObject;
        ledgeCollider = ledgeParent.transform.GetChild(1).gameObject;
        mobileJoystick = GameObject.Find("MobileJoystick");
        joystickScript = mobileJoystick.GetComponent<Joystick>();
    }

    // Update is called once per frame
    void Update()
    {
        //if on ledge = true and player presses jump, teleport to top of ledge
        if (onLedge && CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            //stops player from making further input
            character.gameObject.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>().enabled = false;
            ////teleports character to top of the box
            character.transform.position = Vector3.Lerp(character.transform.position, ledgeCollider.transform.position, 5);
            character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y + 0.5f);
            resetCharacter();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            character = collider.gameObject;
            characterRigidbody = character.GetComponent<Rigidbody>();
        }

        if (character != null)
        {
            grabLedge();
        }
    }

    void OnTriggerExit(Collider collider)
    {
        resetCharacter();
        onLedge = false;
    }


    //the aim is to have a ledge pull up animation
    //stop the character
    //stop the player input
    //play animation
    //give back input
    public void grabLedge()
    {
        //stops character from moving
        //  characterRigidbody.isKinematic = true;
        characterRigidbody.velocity = Vector3.zero;
        characterRigidbody.constraints = RigidbodyConstraints.FreezePositionY  | RigidbodyConstraints.FreezeRotation;
        onLedge = true;
 
    }

    public void resetCharacter()
    {
        characterRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        joystickScript.axesToUse = Joystick.AxisOption.Both;
        character.gameObject.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>().enabled = true;
    }
}