using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GlobalGameManager : MonoBehaviour
{
    private static GlobalGameManager _instance;

    private Transform _playerTransform;

    // Various 
    private Canvas touchCanvas;
    private GameObject pauseScreen;
    private GameObject fadeScreen;

    private bool m_paused = false;

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

        touchCanvas = GameObject.FindWithTag("UICanvas").GetComponent<Canvas>();
        pauseScreen = transform.FindChild("PauseScreen").gameObject;
        fadeScreen = transform.FindChild("FadeScreen").gameObject;

        pauseScreen.SetActive(false);
        fadeScreen.SetActive(true);

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
        touchCanvas.enabled = setter;

        // If we're turning the UI on....
        if (setter)
        {
            // We need to check if this canvas have a joystick?
            var joystick = touchCanvas.gameObject.GetComponentInChildren<Joystick>();
            if (joystick)
            {
                joystick.SetAxis(joystick.axesToUse);
            }

        }

        //// Loop through our canvases
        //for (int i = 0; i < uiCanvases.Count; i++)
        //{
        //    var canvas = uiCanvases[i];
        //    canvas.enabled = setter;

        //    // If we're turning the UI on....
        //    if (setter)
        //    {
        //        // We need to check if this canvas have a joystick?
        //        var joystick = canvas.gameObject.GetComponentInChildren<Joystick>();

        //        if (joystick == null)
        //        {
        //            // We need to poke the Joystick into working properly, don't know why
        //            joystick.SetAxis(joystick.axesToUse);
        //        }
        //    }
        //}

    }

    public void PauseGame()
    {
        m_paused = !m_paused;
        // Stop time
        Time.timeScale = m_paused ? 0f : 1f;

        ToggleUI(!m_paused);
        pauseScreen.SetActive(m_paused);
    }

    /// <summary>
    /// Returns to the main menu scene and tidies up variables/state
    /// </summary>
    public void ReturnToMenu()
    {
        // Do we need to tidy up any variables or state?
        pauseScreen.SetActive(false);
        SceneTransitionManager.Instance.LoadTargetLevel(0);
    }

    /// <summary>
    /// Closes the game
    /// </summary>
    public void CloseGame()
    {
        // Does anything need to happen? Save data to playerprefs or whatever?

        Application.Quit();
    }
}
