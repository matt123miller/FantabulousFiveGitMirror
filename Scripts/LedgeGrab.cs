using UnityEngine;
using System.Collections;

public class LedgeGrab : MonoBehaviour
{

    private GameObject character = null;
    private GameObject ledgeCollider = null;
    private GameObject ledgeParent = null;

    // Use this for initialization
    void Start()
    {
        ledgeParent = gameObject.transform.parent.gameObject;
        ledgeCollider = ledgeParent.transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            character = collider.gameObject;
        }

        if (character != null)
        {
            grabLedge();
        }
    }


    //the aim is to have a ledge pull up animation
    //stop the character
    //stop the player input
    //play animation
    //give back input
    public void grabLedge()
    {
        //stops character from moving
        Rigidbody characterRigidbody = character.GetComponent<Rigidbody>();
        characterRigidbody.isKinematic = true;
        //stops player from making further input
        character.gameObject.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>().enabled = false;
        //teleports character to top of the box
        character.transform.position = Vector3.Lerp(character.transform.position, ledgeCollider.transform.position, 5);
        character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y + 0.5f);
        //resets character for further input
        characterRigidbody.isKinematic = false;
        character.gameObject.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>().enabled = true;

    }
}