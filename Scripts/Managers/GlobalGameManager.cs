using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class GlobalGameManager : MonoBehaviour
{
    private static GlobalGameManager _instance;
    private GameObject uiCanvas;
    private GameObject player;

    // Singleton object, access this via GlobalGameManager.Instance whenever you need the global stuff.
    public static GlobalGameManager Instance
    {
        get
        {
            if (_instance == null) { _instance = new GlobalGameManager(); }
            return _instance;
        }
    }

    // 3 starting methods called in the order shown. Awake, OnEnable, Start.

    void Awake()
    {
        _instance = this;

        player = GameObject.FindWithTag("Player");
        uiCanvas = GameObject.FindWithTag("UICanvas");
    }

    void OnEnable()
    {
        
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleUI(bool setter)
    {
        uiCanvas.SetActive(setter);

        if (setter)
        {
            var joystick = uiCanvas.GetComponentInChildren<Joystick>();
            joystick.SetAxis(joystick.axesToUse);
        }
    }
}
