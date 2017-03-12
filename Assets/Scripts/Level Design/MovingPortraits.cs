using UnityEngine;
using System.Collections;

public class MovingPortraits : MonoBehaviour
{
    public Camera _renderCamera;
    private Transform _paintingContents;
    private Transform _player;
    private float _invSqrColliderDistance;

    public Transform pictureTexture;

    [Range(0, 5)]
    [Header("Edit me!")]
    public int textureScale = 3;

    [Header("Not me!")]
    public int texWidth;
    public int texHeight;

    void Awake()
    {
        // Get my components
        _renderCamera = GetComponentInChildren<Camera>();
        _invSqrColliderDistance = GetComponentInChildren<SphereCollider>().radius; // Get the radius.
        _invSqrColliderDistance *= _invSqrColliderDistance; // Square it
        _invSqrColliderDistance = 1 / _invSqrColliderDistance; // Inverse it
        _paintingContents = transform.GetChild(0);

        // Create and set the render texture.
        RenderTexture rTex = new RenderTexture(texWidth, texHeight, 0);
        _renderCamera.targetTexture = rTex;
        pictureTexture.GetComponent<MeshRenderer>().material.mainTexture = rTex;

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

        // Some sort of percentage value for affecting the resolution of the image?
        float percentDistance = sqrDistance * _invSqrColliderDistance;
        //print(percentDistance);

        // move the contents of the painting accordingly 


    }

    void OnValidate()
    {
        texWidth = (int)Mathf.Pow(2, 5 + textureScale);
        texHeight = (int)Mathf.Pow(2, 5 + textureScale);

    }
}
