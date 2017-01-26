using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.Utility.Inspector;


//We need a target at the each end of the rope for alligning the player to.


public class RopeBalance : MonoBehaviour
{
    private Quaternion _enterRotation;
    private Quaternion _currentPhoneRotation;
    private Quaternion _negatePhoneRotation;

    private ThirdPersonUserControl _character;

    public float currentAngleDifference;
    public float angleThreshold = 15f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _enterRotation = DeviceRotation.GetRotation();
        _character = other.GetComponent<ThirdPersonUserControl>();
        //_character.isGrounded = true;

        GlobalGameManager.Instance.ToggleUI(false);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        other.transform.rotation = transform.localRotation;
        var pos = other.transform.position;
        Vector3 posAdjusment = new Vector3(pos.x, pos.y, transform.position.z);
        other.transform.position = posAdjusment;

        GlobalGameManager.Instance.ToggleUI(true);

    }


    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _character.CalculateAndMove(0.5f,0,false);

        print("In the zone!");

        _currentPhoneRotation = DeviceRotation.GetRotation();

        // Use Quaternion.angle(q1, q2) or Atan2(q1.x, q2.x) ????
        // What is the difference between entry and current rotations?
        currentAngleDifference = Quaternion.Angle(_enterRotation, _currentPhoneRotation);
        //currentAngleDifference = Mathf.Atan2(_enterRotation.z, _currentPhoneRotation.z).AsAngle();

        if (currentAngleDifference > angleThreshold)
        {

        }
    }


}
