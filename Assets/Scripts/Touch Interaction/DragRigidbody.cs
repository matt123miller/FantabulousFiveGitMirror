using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class DragRigidbody : MonoBehaviour
{
    private const float _spring = 50.0f;
    private const float _damper = 5.0f;
    private const float _drag = 10.0f;
    private const float _angularDrag = 5.0f;
    private const float _distance = 0.2f;
    private const bool _attachToCenterOfMass = false;

    private SpringJoint _springJoint;


    private void Update()
    {
        // Make sure the user pressed the mouse down
        if (!Input.GetMouseButton(0))
        {
            return;
        }


        var mainCamera = FindCamera();

        // We need to actually hit an object
        RaycastHit hit = new RaycastHit();
        if (
            !Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition).origin,
                             mainCamera.ScreenPointToRay(Input.mousePosition).direction, out hit, 100,
                             Physics.DefaultRaycastLayers))
        {
            return;
        }
        // We need to hit a rigidbody that is not kinematic
        if (!hit.rigidbody || hit.rigidbody.isKinematic)
        {
            return;
        }


        if (!_springJoint)
        {
            var go = new GameObject("Rigidbody dragger");
            Rigidbody body = go.AddComponent<Rigidbody>();
            _springJoint = go.AddComponent<SpringJoint>();
            body.isKinematic = true;
        }

        _springJoint.transform.position = hit.point;
        _springJoint.anchor = Vector3.zero;

        _springJoint.spring = _spring;
        _springJoint.damper = _damper;
        _springJoint.maxDistance = _distance;
        _springJoint.connectedBody = hit.rigidbody;

        StartCoroutine("DragObject", hit.distance);
    }


    private IEnumerator DragObject(float distance)
    {
        var oldDrag = _springJoint.connectedBody.drag;
        var oldAngularDrag = _springJoint.connectedBody.angularDrag;
        _springJoint.connectedBody.drag = _drag;
        _springJoint.connectedBody.angularDrag = _angularDrag;
        var mainCamera = FindCamera();
        while (Input.GetMouseButton(0))
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            _springJoint.transform.position = ray.GetPoint(distance);
            yield return null;
        }


        if (_springJoint.connectedBody)
        {
            _springJoint.connectedBody.drag = oldDrag;
            _springJoint.connectedBody.angularDrag = oldAngularDrag;
            _springJoint.connectedBody = null;
        }

        //At the end remove the spawned spring object.
        Destroy(_springJoint.gameObject);

    }


    private Camera FindCamera()
    {
        if (GetComponent<Camera>())
        {
            return GetComponent<Camera>();
        }

        return Camera.main;
    }
}
