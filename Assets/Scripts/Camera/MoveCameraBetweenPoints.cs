using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;

public class MoveCameraBetweenPoints : MonoBehaviour
{

    // probably attached to camera
    public Transform startTransform;
    public float timeUntilMoveBegins = 2f;
    public float moveDuration = 2f;
    public float timeValue = 0;
    public AnimationCurve curve;
    public float lerpCompletion;

    private Vector3 originalPos;
    private Quaternion originalRotation;

    void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {
        if (startTransform == null)
        {
            GameObject.FindWithTag("ScreenFader").GetComponent<ScreenFade>().turnUIOnAfter = true;
            this.enabled = false;
            return;
        }
        originalPos = transform.position;
        originalRotation = transform.rotation;
        print(originalPos);
        // move to the placement object
        transform.position = startTransform.position;
        transform.rotation = startTransform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeUntilMoveBegins < 0)
        {
            timeValue += (Time.deltaTime/moveDuration);
            lerpCompletion = curve.Evaluate(timeValue);

            transform.position = Vector3.Lerp(startTransform.position, originalPos, lerpCompletion);
            transform.rotation = Quaternion.Slerp(startTransform.rotation, originalRotation, lerpCompletion);

            if (lerpCompletion >= 1)
            {
                GlobalGameManager.Instance.ToggleUI(true);
                this.enabled = false;
                return; // probably return here as we don't want to do other things?
            }
        }
        else
        {
            timeUntilMoveBegins -= Time.deltaTime;
        }
    }
}
