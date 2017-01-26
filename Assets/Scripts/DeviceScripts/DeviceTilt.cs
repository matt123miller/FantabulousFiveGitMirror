using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class DeviceTilt : MonoBehaviour {

    public static float AcccelerometerUpdateInterval = 1.0f / 60.0f;
    public static float LowPassKernalWidthInSeconds = 0.5f; //the greater the value, the longer the length in reading the acceleration? idk, the smaller it is the more sensitive it is

    private float _lowPassFilterFactor = AcccelerometerUpdateInterval / LowPassKernalWidthInSeconds;
    private Vector3 _lowPassValue = Vector3.zero;

    void Start()
    {
        _lowPassValue = Input.acceleration;
    }

    public Vector3 LowPassFilterAccelerometer()
    {
        _lowPassValue = Vector3.Lerp(_lowPassValue, Input.acceleration, _lowPassFilterFactor);
        return _lowPassValue;
    }


}
