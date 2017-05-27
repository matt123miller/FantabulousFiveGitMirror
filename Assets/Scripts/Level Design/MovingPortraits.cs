using UnityEngine;
using System.Collections;

public class MovingPortraits : MonoBehaviour
{
    // TODO Maybe turn the render camera off when it's far away from the player for optimisation?
    public Camera _renderCamera;
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

        // Create and set the render texture.
        RenderTexture rTex = new RenderTexture(texWidth, texHeight, 0);
        _renderCamera.targetTexture = rTex;
        pictureTexture.GetComponent<MeshRenderer>().material.mainTexture = rTex;

    }

    void Start()
    {
        
    }
    
    // This is called whenever you change a value in the inspector
    // Therefore it's used to alter the resolution width and height as users change the slider value.
    void OnValidate()
    {
        texWidth = (int)Mathf.Pow(2, 5 + textureScale);
        texHeight = (int)Mathf.Pow(2, 5 + textureScale);

    }
}
