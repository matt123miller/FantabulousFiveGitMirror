using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(Rigidbody))]
public class Draggable : TouchInteractive {

	// Can a draggable also be tapped? 
	// Or should it just do the drag action, for example it would only be dragged a tiiiiny bit

	// Need to work out how to define a drag axis, 1 or 2 dimensions.

    public MoveAxis movementAxis = MoveAxis.Both;
    private Rigidbody rb;
    
	void Awake ()
	{
	    rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
	
	} 

	public override void Interact(Vector3 fingerPosition)
	{
        print("Interacting");
	    Vector3 moveBy = transform.position - fingerPosition;

        // strip out the unnecessary axis of movement
	    if (movementAxis == MoveAxis.Both)
	    {
	        moveBy = new Vector3(moveBy.x, 0, moveBy.z);
	    }
        else if (movementAxis == MoveAxis.Vertical)
	    {
            moveBy = new Vector3(0, 0, moveBy.z);
        }
        else // Horizontal
	    {
            moveBy = new Vector3(moveBy.x, 0, 0);
        }

        // Move towards the finger position as fast as the rigidbody will allow
	    var v3 = 3;




	}
}
