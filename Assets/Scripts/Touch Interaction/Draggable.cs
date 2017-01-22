using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(Rigidbody))]
public class Draggable : MonoBehaviour, TouchInteractive {

	// Can a draggable also be tapped? 
	// Or should it just do the drag action, for example it would only be dragged a tiiiiny bit

	// Need to work out how to define a drag axis, 1 or 2 dimensions.
    public enum MoveAxis
    {
        None, Both, Horizontal, Vertical
    }

    public MoveAxis movementAxis = MoveAxis.Both;
    public Rigidbody rb;
    
	void Awake ()
	{
	    rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
	
	} 

	public void Interact(Vector3 fingerPosition)
	{
        print("Drag that");
	    Vector3 moveBy = transform.position - fingerPosition;
	    Vector3 adjustment;

	    if (movementAxis == MoveAxis.None)
	    {
	        return;
        }

        // strip out the unnecessary axis of movement
	    if (movementAxis == MoveAxis.Both)
	    {
            adjustment = new Vector3(1, 0, 1);
	    }
        else if (movementAxis == MoveAxis.Vertical)
	    {
            adjustment = new Vector3(0, 0, 1);
        }
        else // Horizontal
	    {
            adjustment = new Vector3(1, 0, 0);
        }

        // Move towards the finger position as fast as the rigidbody will allow
        // Is there a shorter way to do this???
	    moveBy.x *= adjustment.x;
        moveBy.y *= adjustment.y;
        moveBy.z *= adjustment.z;
        
	    // move


	}

    public void FinishInteraction()
    {
        print("Drag finished");
    }
}
