using UnityEngine;
using System.Collections;

public static class DeviceRotation
{
    // Call DeviceRotation.GetRotation() for all your tilt phone needs.
    // You will have to offset for the phones idea of forward though. 
    private static bool _gyroInitialized = false;

    public static bool HasGyroscope
    {
        get
        {
            return SystemInfo.supportsGyroscope;
        }
    }

    public static Quaternion GetRotation()
    {
        if (!_gyroInitialized)
        {
            InitGyro();
        }

        return HasGyroscope
            ? ReadGyroscopeRotation()
            : Quaternion.identity;
    }

    private static void InitGyro()
    {
        if (HasGyroscope)
        {
            Input.gyro.enabled = true;                // enable the gyroscope
            Input.gyro.updateInterval = 0.0167f;      // set the update interval to it's highest value (60 Hz)
        }
        _gyroInitialized = true;
    }

    // This is the real magic. I have no idea how it works but it does!
    private static Quaternion ReadGyroscopeRotation()
    {
        return new Quaternion(0.5f, 0.5f, -0.5f, 0.5f) * Input.gyro.attitude * new Quaternion(0, 0, 1, 0);
    }


}