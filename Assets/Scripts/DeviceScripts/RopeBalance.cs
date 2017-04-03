using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

using UnityStandardAssets.CrossPlatformInput;


//We need a target at the each end of the rope for alligning the player to.


public class RopeBalance : MonoBehaviour
{
    private ThirdPersonUserControl _character;
    private Joystick _joystick;
    private GameObject _dial;
    // private Transform _characterSkeleton;
    // private Quaternion _negatePhoneRotation;

    private float _enterZAngle;
    private float _currentZAngle;
    private float _inverseAngleThreshold;
    private Vector3 _moveTowards;

    public Transform dialRoot;
    public float angleThreshold = 15f;
    [Range(0, 1)]
    public float characterSpeed = 0.6f;
    public bool isBalancing = false;

    // Use this for initialization
    void Start()
    {
        _character = FindObjectOfType<ThirdPersonUserControl>();
        _joystick = FindObjectOfType<Joystick>();

        if(!_character || !_joystick)
        {
            Destroy(this);
        }

        _dial = transform.parent.FindChild("BalanceScreen").gameObject;
        // _characterSkeleton = _character.transform;//.FindChild("EthanSkeleton");

        _inverseAngleThreshold = angleThreshold * -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isBalancing) return;

        // Move towards the other rope node.
        transform.LookAt(_moveTowards);

        // Walk slowly forward, it should be facing the node at the other end.
        CrossPlatformInputManager.SetAxis("Horizontal", 0);
        CrossPlatformInputManager.SetAxis("Vertical", characterSpeed);


        var difference = BalanceAngleDifference();

        dialRoot.rotation = Quaternion.Euler(0, 0, difference);

        Camera.main.transform.Rotate(Vector3.forward, difference * characterSpeed, Space.Self);

        if (difference > angleThreshold || difference < _inverseAngleThreshold)
        {
            // What happens when it's too much? We fall off I assume.
            print("Fall off");
            // Why can't I add force here? Is there some tick box on?
            var rb = _character.GetComponent<Rigidbody>();
            rb.AddForce(new Vector3(1, 1, 0) * difference);
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
        _moveTowards = otherEnd.position;

        _enterZAngle = DeviceRotation.GetRotation().eulerAngles.z;
        // _negatePhoneRotation = Quaternion.Inverse(DeviceRotation.GetRotation());

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

        // A bit of normalisation
        if (difference > 180) difference -= 360f;

        // Return the inverse as otherwise the UI rotation is counter intuitive (it's the opposite)
        return difference * -1;
    }
}
