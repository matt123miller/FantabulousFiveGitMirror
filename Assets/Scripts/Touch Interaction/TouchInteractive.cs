using UnityEngine;
using System.Collections;


[RequireComponent(typeof(AudioSource))]
public abstract class TouchInteractive : MonoBehaviour {

    public enum MoveAxis
    {
        Both, Horizontal, Vertical
    }

    public abstract void Interact(Vector3 touchPosition);
}


