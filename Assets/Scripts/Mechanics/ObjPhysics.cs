using UnityEngine;
using System.Collections;

public class ObjPhysics : MonoBehaviour {

    private float pushPower = 2.0f;
    private Rigidbody body;

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        body = hit.collider.attachedRigidbody;

        if (body == null || body.isKinematic)
        {
            return;
        }

        if (hit.moveDirection.y < -0.3f)
        {
            return;
        }

        Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.velocity = pushDirection * pushPower;
            
     }

}
