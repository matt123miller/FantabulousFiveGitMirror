using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class DeviceTilt : MonoBehaviour {

    public static float AcccelerometerUpdateInterval = 1.0f / 60.0f;
    public static float LowPassKernalWidthInSeconds = 0.5f; //the greater the value, the longer the length in reading the acceleration? idk, the smaller it is the more sensitive it is

    private float LowPassFilterFactor = AcccelerometerUpdateInterval / LowPassKernalWidthInSeconds;
    private Vector3 lowPassValue = Vector3.zero;

    void Start()
    {
        lowPassValue = Input.acceleration;
    }

    public Vector3 LowPassFilterAccelerometer()
    {
        lowPassValue = Vector3.Lerp(lowPassValue, Input.acceleration, LowPassFilterFactor);
        return lowPassValue;
    }


}
