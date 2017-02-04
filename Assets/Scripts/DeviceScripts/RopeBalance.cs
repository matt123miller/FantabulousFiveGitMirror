using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;


//We need a target at the each end of the rope for alligning the player to.


public class RopeBalance : MonoBehaviour
{
    private ThirdPersonUserControl _character;
    private GameObject _dial;

    private float _enterZAngle;
    private float _currentZAngle;
    private float _inverseAngleThreshold;

    public Transform dialRoot;
    public float angleThreshold = 15f;
    [Range(0, 1)]
    public float characterSpeeed = 0.7f;
    public bool isBalancing = false;

    // Use this for initialization
    void Start()
    {
        _character = FindObjectOfType<ThirdPersonUserControl>();
        _dial = transform.parent.FindChild("BalanceScreen").gameObject;

        _inverseAngleThreshold = angleThreshold * -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isBalancing) return;

        // Walk slowly forward, it should be facing the node at the other end.
        _character.CalculateAndMove(characterSpeeed, 0, false);

        var difference = BalanceAngleDifference();

        dialRoot.rotation = Quaternion.Euler(0, 0, difference);

        if (difference > angleThreshold || difference < _inverseAngleThreshold)
        {
            // What happens when it's too much? We fall off I assume.

            // Whatever happens, we still have to end the balance.
            EndBalance();
        }
    }

    public void BeginBalance(Transform thisEnd, Transform otherEnd)
    {
        print("Begin balance");

        isBalancing = true;
        _dial.SetActive(true);

        _character.transform.position = thisEnd.position;
        _character.transform.LookAt(otherEnd);

        _enterZAngle = DeviceRotation.GetRotation().eulerAngles.z;

        GlobalGameManager.Instance.ToggleUI(false);
    }

    public void EndBalance()
    {
        print("End balance");

        isBalancing = false;
        _dial.SetActive(false);

        GlobalGameManager.Instance.ToggleUI(true);
    }


    private float BalanceAngleDifference()
    {
        // What is the z angle of the phone? 
        _currentZAngle = DeviceRotation.GetRotation().eulerAngles.z;
        // How different is it to the entry z angle?
        var difference = _enterZAngle - _currentZAngle;

        if (difference > 180) difference -= 360f;

        // Return the inverse as otherwise the UI rotation is counter intuitive (it's the opposite)
        return difference * -1;
    }
}
