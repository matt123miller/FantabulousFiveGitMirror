using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;


//We need a target at the each end of the rope for alligning the player to.


public class RopeBalance : MonoBehaviour
{
    private Quaternion _enterRotation;
    private Quaternion _currentPhoneRotation;
    private Quaternion _negatePhoneRotation;

    private ThirdPersonUserControl _character;

    public float currentAngleDifference;
    public float angleThreshold = 15f;
    [Range(0,1)]
    public float characterSpeeed = 0.7f;
    public bool isBalancing = false;
    // Use this for initialization
    void Start()
    {
        _character = FindObjectOfType<ThirdPersonUserControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isBalancing)
        {
            // Walk slowly forward, it should be facing the node at the other end.
            _character.CalculateAndMove(characterSpeeed, 0, false);
            currentAngleDifference = BalanceDifferenceValue();

            print(currentAngleDifference);
            if (currentAngleDifference > angleThreshold)
            {
                // What happens when it's too much? We fall off I assume.

                // Whatever happens, we still have to end the balance.
                EndBalance();
            }
        }

    }

    public void BeginBalance(Transform thisEnd,Transform otherEnd)
    {
        isBalancing = true;
        _character.transform.position = thisEnd.position;
        _character.transform.LookAt(otherEnd);

        print("Begin balance");
        _enterRotation = DeviceRotation.GetRotation();
        GlobalGameManager.Instance.ToggleUI(false);
    }

    public void EndBalance()
    {
        print("End balance");
        isBalancing = false;
        GlobalGameManager.Instance.ToggleUI(true);
    }

    public float BalanceDifferenceValue()
    {
        _currentPhoneRotation = DeviceRotation.GetRotation();

        // Use Quaternion.angle(q1, q2) or Atan2(q1.x, q2.x) ????
        // What is the difference between entry and current rotations?
        return Quaternion.Angle(_enterRotation, _currentPhoneRotation);
        //return Mathf.Atan2(_enterRotation.z, _currentPhoneRotation.z).AsAngle();
    }



    //void OnTriggerExit(Collider other)
    //{
    //    if (!other.CompareTag("Player")) return;

    //    //other.transform.rotation = transform.localRotation;
    //    //var pos = other.transform.position;
    //    //Vector3 posAdjusment = new Vector3(pos.x, pos.y, transform.position.z);
    //    //other.transform.position = posAdjusment;


    //}
}
