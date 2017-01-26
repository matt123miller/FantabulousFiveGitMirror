using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

/// <summary>
/// Currently not in use, I don't know if we'll ever need it now that we've found Event Triggers
/// </summary>
public class TouchInputManager : MonoBehaviour {

	private Ray _ray;
	private RaycastHit _hit;
	private Touch _touch;
	private Vector3 _touchPosition;

    private List<TouchInteractive> _touchables;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {

            _touchPosition = Input.mousePosition;
            //print(_touchPosition);	
            _ray = Camera.main.ScreenPointToRay(_touchPosition);


            if (Physics.Raycast(_ray, out _hit))
            {

                // Get all my _touchables.
                _touchables = new List<TouchInteractive>(_hit.transform.GetComponents<TouchInteractive>());

                foreach (TouchInteractive touch in _touchables)
                {
                    // Call Interact(vector3) on it.
                    touch.Interact(_hit.transform.position);
                }
            }
        }

        else if (Input.GetMouseButtonUp(0))
        {
            //if (_touchables.Count != 0)
            //{
            //    _touchables
            //    _touchables.Clear();
            //}
            foreach (var touch in _touchables)
            {
                touch.FinishInteraction();
            }
            _touchables.Clear();
        }
    }

    void TapOccurred(){

	}

	void TwoFingerTapOccurred(){

	}

	void DragOccurred(){

	}
}
