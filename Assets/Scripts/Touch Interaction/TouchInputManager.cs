using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class TouchInputManager : MonoBehaviour {

	Ray _ray;
	RaycastHit _hit;
	Touch _touch;
	Vector3 _touchPosition;

    //private TouchInteractive[] touchables;
    private List<TouchInteractive> touchables;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

        //if (Input.GetMouseButtonDown(0)) {

        //    _touchPosition = Input.mousePosition;
        //    //print(_touchPosition);	
        //    _ray = Camera.main.ScreenPointToRay(_touchPosition);


        //    if (Physics.Raycast(_ray, out _hit)) {

        //        // Get all my touchables.
        //        touchables = new List<TouchInteractive> (_hit.transform.GetComponents<TouchInteractive>());

        //        foreach (TouchInteractive touch in touchables)
        //        {
        //            // Call Interact(vector3) on it.
        //            touch.Interact(_hit.transform.position);
        //        }
        //    }
        //}

        //else if (Input.GetMouseButtonUp(0))
        //{
        //    //if (touchables.Count != 0)
        //    //{
        //    //    touchables
        //    //    touchables.Clear();
        //    //}
        //    foreach (var touch in touchables)
        //    {
        //        touch.FinishInteraction();
        //    }
        //    touchables.Clear();
        //}
    }

    void TapOccurred(){

	}

	void TwoFingerTapOccurred(){

	}

	void DragOccurred(){

	}
}
