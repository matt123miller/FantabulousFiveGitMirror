using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TouchInputManager : MonoBehaviour {

	Ray _ray;
	RaycastHit _hit;
	Touch _touch;
	Vector3 _touchPosition;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    //    if (EventSystem.current.IsPointerOverGameObject() ||
    //EventSystem.current.currentSelectedGameObject != null)
    //    {
    //        print("what is this");
    //        return;
    //    }
#if UNITY_EDITOR
        print("Editor");
        if (Input.GetMouseButtonDown(0)){
			_touchPosition = Input.mousePosition;
			print(_touchPosition);	
		
			_ray = Camera.main.ScreenPointToRay(_touchPosition);

			if (Physics.Raycast(_ray, out _hit)) {

				var touchable = _hit.transform.GetComponent<TouchInteractive>();

				if (touchable){
					touchable.Interact(_touchPosition);
				}
			}
		}
#elif MOBILE_INPUT
        print("Mobile");
        var touches = Input.touches;

        // Exit early if nothing is going on
        if (touches.Length == 0)
        {
            print("No touches");
            return;
        }
        // Otherwise we need to work out what's happened. 

        // Cache some things.
        _touch = touches[0];
        _touchPosition = _touch.position;

        // Raycast at the world position of the finger.
        _ray = Camera.main.ScreenPointToRay(_touchPosition);

        if (Physics.Raycast(_ray, out _hit))
        {

            // Check for an ITouchInteractive
            var touchable = _hit.transform.GetComponent<TouchInteractive>();

            if (touchable)
            {
                // Call Interact() on it
                touchable.Interact(_touchPosition);
            }
        }
#endif


    }

    void TapOccurred(){

	}

	void TwoFingerTapOccurred(){

	}

	void DragOccurred(){

	}
}
