using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GlobalGameManager : MonoBehaviour
{
    private static GlobalGameManager _instance;

    private Transform _playerTransform;

    // Various 
    private Canvas _touchCanvas;
    private GameObject _pauseScreen;
    private GameObject _fadeScreen;


    private bool _paused = false;


    // Singleton object, access this via GlobalGameManager.Instance whenever you need the global stuff.
    public static GlobalGameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindWithTag("GlobalGameManager").GetComponent<GlobalGameManager>();
            }
            return _instance;
        }
    }


    public Transform PlayerTransform // Lazy loaded, will it work?
    {
        get
        {
            if (!_playerTransform)
            {
                _playerTransform = GameObject.FindWithTag("Player").transform; 
            }
            return _playerTransform;
        }
        set { _playerTransform = value; }
    }

    // 3 starting methods called in the order shown. Awake, OnEnable, Start.

    void Awake()
    {
        _instance = this;

        _touchCanvas = GameObject.FindWithTag("UICanvas").GetComponent<Canvas>();
        _pauseScreen = transform.FindChild("PauseScreen").gameObject;
        _fadeScreen = transform.FindChild("FadeScreen").gameObject;

        //PAUSE SCREEN TURNED OFF VIA MENU MANAGER
       // _pauseScreen.SetActive(false);
        _fadeScreen.SetActive(true);

    }

    void OnEnable()
    {


    }

    // Use this for initialization
    void Start()
    {
        ToggleUI(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Turns the stored Canvases off or on. Also handles the Joystick
    /// </summary>
    /// <param name="setter"></param>
    public void ToggleUI(bool setter)
    {
        _touchCanvas.enabled = setter;

        var joystick = _touchCanvas.gameObject.GetComponentInChildren<Joystick>();

        if(joystick)
            joystick.enabled = setter;

        // If we're turning the UI on....
        if (setter)
        {
            // We need to check if this canvas have a joystick?
            if (joystick)
            {
                joystick.SetAxis(joystick.axesToUse);
            }

            Noise noiseScript = _touchCanvas.gameObject.GetComponentInChildren<Noise>();
            noiseScript.ResetNoiseBar();


        }
    }

    /// <summary>
    /// Toggles pause setting and will turn the UI off/on
    /// </summary>
    public void PauseGame()
    {
        _paused = !_paused;
        // Stop time
        Time.timeScale = _paused ? 0f : 1f;

        ToggleUI(!_paused);
        _pauseScreen.SetActive(_paused);
    }

    /// <summary>
    /// Returns to the main menu scene and tidies up variables/state
    /// </summary>
    public void ReturnToMenu()
    {

        // Do we need to tidy up any variables or state?
        _pauseScreen.SetActive(false);
        PauseGame();
        SceneTransitionManager.Instance.LoadTargetLevel(0);
    }

    public void ReloadToCheckpoint()
    {
        _pauseScreen.SetActive(false);
        PauseGame();
        SceneTransitionManager.Instance.ReloadCurrentLevel();
    }

    /// <summary>
    /// Closes the game
    /// </summary>
    public void CloseGame()
    {
        // Does anything need to happen? Save data to playerprefs or whatever?

        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            PauseGame();
        }
        Application.Quit();
    }
}
