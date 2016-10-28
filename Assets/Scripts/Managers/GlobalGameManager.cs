using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GlobalGameManager : MonoBehaviour
{
    private static GlobalGameManager _instance;
    private GameObject uiCanvas;
    private GameObject playerPrefab;
    private GameObject player;
    private string characterSelected;
    private GameObject spawnPoint;

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
        uiCanvas = GameObject.FindWithTag("UICanvas");
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name != "Tish Test")
        {
            string resourcesString = "Prefabs/Scene Requirements/Character/";
            spawnPoint = GameObject.FindWithTag("SpawnPoint");
            characterSelected = PlayerPrefs.GetString("CharacterSelected");
            resourcesString += characterSelected;
            playerPrefab = (GameObject)Resources.Load(resourcesString, typeof(GameObject));
            player = Instantiate(playerPrefab, spawnPoint.transform.position, Quaternion.identity) as GameObject;
        }

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
            if (joystick)
            {
                joystick.SetAxis(joystick.axesToUse);
            }
        }
    }
}
