using UnityEngine;
using System.Collections;

public class MoveObjectBetweenPoints : MonoBehaviour
{

    // probably attached to camera

    public Transform startTransform;
    public float timeUntilMoveBegins = 2f;
    public float moveDuration = 2f;

    private Transform originalTransform;
    private float movePercentage;

    // Use this for initialization
    void Start()
    {
        originalTransform = transform;
        // move to the placement object
        transform.position = startTransform.position;
        transform.rotation = startTransform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeUntilMoveBegins < 0)
        {
            movePercentage += (Time.smoothDeltaTime / moveDuration);
            transform.position = Vector3.Lerp(startTransform.position, originalTransform.position, movePercentage);
            transform.rotation = Quaternion.Slerp(startTransform.rotation, originalTransform.rotation, movePercentage);
        }
        else
        {
            timeUntilMoveBegins -= Time.deltaTime;
        }
    }
}
