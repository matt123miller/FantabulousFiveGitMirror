using UnityEngine;
using System.Collections;

public class ObjNoise : MonoBehaviour {

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("IMACT VELOCITY" + collision.relativeVelocity.magnitude);
    }
}
