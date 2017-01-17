using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class TouchInputManager : MonoBehaviour {

	Ray _ray;
	RaycastHit _hit;
	Touch _touch;
	Vector3 _touchPosition;

    private TouchInteractive[] touchables;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetMouseButtonDown(0)){

			_touchPosition = Input.mousePosition;
			//print(_touchPosition);	
			_ray = Camera.main.ScreenPointToRay(_touchPosition);

			if (Physics.Raycast(_ray, out _hit)) {

                // Get all my touchables.
				touchables = _hit.transform.GetComponents<TouchInteractive>();
                
                foreach (TouchInteractive touch in touchables)
			    {
                    // Call Interact(v3) on it.
			        touch.Interact(touch.transform.position);
			    }
			}
		}
        
        else if (Input.GetMouseButtonUp(0))
        {
            foreach (var touch in touchables)
            {
                touch.FinishInteraction();
            }
        }

//#elif MOBILE_INPUT
//        print("Mobile");
//        var touches = Input.touches;

//        // Exit early if nothing is going on
//        if (touches.Length == 0)
//        {
//            print("No touches");
//            return;
//        }
//        // Otherwise we need to work out what's happened. 

//        // Cache some things.
//        _touch = touches[0];
//        _touchPosition = _touch.position;

//        // Raycast at the world position of the finger.
//        _ray = Camera.main.ScreenPointToRay(_touchPosition);

//        if (Physics.Raycast(_ray, out _hit))
//        {

//            // Check for an ITouchInteractive
//            var touchable = _hit.transform.GetComponent<TouchInteractive>();

//            if (touchable)
//            {
//                // Call Interact() on it
//                touchable.Interact(_touchPosition);
//            }
//        }
//#endif


    }

    void TapOccurred(){

	}

	void TwoFingerTapOccurred(){

	}

	void DragOccurred(){

	}
}
