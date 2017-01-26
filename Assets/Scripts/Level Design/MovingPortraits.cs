using UnityEngine;
using System.Collections;

public class MovingPortraits : MonoBehaviour
{
    private Camera _renderCamera;
    private Transform _paintingContents;
    private Transform _player;
    private float _invSqrColliderDistance;
    //private float sqr

    [Range(0, 5)]
    public int wScale = 1, hScale = 1;

    [Range(0, 256)]
    public int width, height;

    void Awake()
    {
        // Get my components
        _renderCamera = GetComponentInChildren<Camera>();
        _invSqrColliderDistance = GetComponent<SphereCollider>().radius; // Get it.
        _invSqrColliderDistance *= _invSqrColliderDistance; // Square it
        _invSqrColliderDistance = 1 / _invSqrColliderDistance; // Inverse it
        _paintingContents = transform.GetChild(0);

        // Create and set the render texture.
        RenderTexture rTex = new RenderTexture(width, height, 0);
        _renderCamera.targetTexture = rTex;
        GetComponent<MeshRenderer>().material.mainTexture = rTex;

    }

    void Start()
    {

    }

    // Update is called once per frame
    void OnTriggerStay(Collider collider)
    {
        if (!collider.CompareTag("Player"))
            return;

        _player = collider.transform;

        // _player in relation to painting
        Vector3 playerToPainting = _player.position - transform.position;
        float sqrDistance = playerToPainting.sqrMagnitude;

        float percentDistance = sqrDistance * _invSqrColliderDistance;
        //print(percentDistance);

        // move the contents of the painting accordingly 


    }

    void OnValidate()
    {
        // Rescale the object to match the given width height
        transform.localScale = new Vector3(wScale, hScale, 1);
        
    }
}
