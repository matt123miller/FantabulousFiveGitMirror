using UnityEngine;
using System.Collections;

public class MovingPortraits : MonoBehaviour
{
    private Camera renderCamera;
    private Transform paintingContents;
    private Transform player;
    private float invSqrColliderDistance;
    //private float sqr

    [Range(0, 5)]
    public int wScale = 1, hScale = 1;

    [Range(0, 256)]
    public int width, height;

    void Awake()
    {
        // Get my components
        renderCamera = GetComponentInChildren<Camera>();
        invSqrColliderDistance = GetComponent<SphereCollider>().radius; // Get it.
        invSqrColliderDistance *= invSqrColliderDistance; // Square it
        invSqrColliderDistance = 1 / invSqrColliderDistance; // Inverse it
        paintingContents = transform.GetChild(0);

        // Create and set the render texture.
        RenderTexture rTex = new RenderTexture(width, height, 0);
        renderCamera.targetTexture = rTex;
        GetComponent<MeshRenderer>().material.mainTexture = rTex;

    }

    void Start()
    {
        player = GlobalGameManager.Instance.PlayerTransform;

    }

    // Update is called once per frame
    void OnTriggerStay(Collider collider)
    {
        if (!collider.CompareTag("Player"))
            return;

        // player in relation to painting
        Vector3 playerToPainting = player.position - transform.position;
        float sqrDistance = playerToPainting.sqrMagnitude;

        float percentDistance = sqrDistance * invSqrColliderDistance;
        print(percentDistance);
        // move the contents of the painting accordingly 


    }

    void OnValidate()
    {
        // Rescale the object to match the given width height
        transform.localScale = new Vector3(wScale, hScale, 1);
        
    }
}
