using System.Collections;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
    This class is designed to manage all scene transitions in an asynchronous way.
    It nicely wraps all of the complexity and accessed via the singleton instance SceneTransitionManager.Instance
    
    This class belongs on a canvas with an Image child. That image should just be a large plain black texture.
    This black texture is then faded up and down to ease UX of transitions.
    Async loading should start once the texture fades in, then the desired level will load.
    The screen will remain black until async is finished at which point the texutre will fade out.
    
    The methods for fading the screen to or from black have been left public if, for some reason, 
    you want to fade the screen without changing the level.
    
    There is a loading bar and flashing loading text included as well, this is optional.
    If you do not wish to use these then remove the all the code concerning loadingText and loadingSlider.
    To turn these off and on call the ToggleLoadingUI(bool) method, passing it a boolean for whether you want them on (true) or off (false).
*/

public class SceneTransitionManager : MonoBehaviour
{

    private static SceneTransitionManager _instance;

    // Singleton object, access this via SceneTransitionManager.Instance whenever you need to call a scene transition method.
    public static SceneTransitionManager Instance
    {
        get
        {
            if (_instance == null) { _instance = new SceneTransitionManager(); }
            return _instance;
        }
    }


    [Tooltip("To begin immediately use 0, increase it to delay the start time")]
    [SerializeField]
    private float fadeStartTime = 0;
    [SerializeField]
    private float fadeMultiplier = 5;
    private float fadeProgress = 0;
    private Image loadingImage;
    private Text loadingText;
    private Slider loadingSlider;


    void Awake()
    {
        _instance = this;

        var fader = transform.GetChildWithTag("ScreenFader");
        fader.gameObject.SetActive(true);
        
        loadingImage = fader.GetComponent<Image>();
        loadingText = fader.GetComponentInChildren<Text>();
        loadingSlider = fader.GetComponentInChildren<Slider>();

        loadingImage.enabled = true;
        
        ToggleLoadingUI(false);
    }


    void Start()
    {
        // Performed in Start to allow all variables to be cacher first.
        BeginFadeToClear();
    }
 
    private void ToggleLoadingUI(bool set)
    {
        loadingText.gameObject.SetActive(set);
        loadingText.enabled = set;
        loadingSlider.gameObject.SetActive(set);
        loadingSlider.enabled = set;
    }

    /// <summary>
    /// Asynchronously begins loading the chosen level in the build settings
    /// Assumes that the class calling the method knows which scene it wants to go to.
    /// </summary>
    /// <param name="targetScene"></param>
    public void LoadTargetLevel(int targetScene)
    {
        if (targetScene > SceneManager.sceneCountInBuildSettings)
        {
            // Handle this error however you like.
        }
        else 
        {
            StartCoroutine(AsyncLoadLevel(targetScene));
        }
    }

    /// <summary>
    /// Asynchronously begins loading the next level in the build settings, assuming there are levels left to load.
    /// </summary>
    public void LoadNextLevel()
    {
        int targetScene = SceneManager.GetActiveScene().buildIndex + 1;
        
        if (targetScene > SceneManager.sceneCountInBuildSettings)
        {
            // There are no scenes left. Maybe return to main menu?
            // Handle this error however you like.
        }
        else 
        {
            StartCoroutine(AsyncLoadLevel(targetScene));
        }
    }

    private IEnumerator AsyncLoadLevel(int targetScene)
    {
        BeginFadeToBlack(false);

        while (fadeProgress < 0.95)
        {
            yield return null;
        }
                
        ToggleLoadingUI(true);
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene);

        while (!asyncLoad.isDone)
        {
            // Here you put your loading screen code.
            loadingSlider.value = asyncLoad.progress;
            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));

            yield return null;
        }
        
        // You don't need to turn the text and slider back off or call BeginFadeToClear() here as the old scene will now be destroyed.
        // The new scene that was just loaded asynchonously will replace it and should have a SceneManager object in it to handle fading etc.
    }
   

    public void BeginFadeToBlack(bool fadeToClearFlag)
    {
        StartCoroutine(FadeToBlack(fadeStartTime + 1, fadeToClearFlag));
    }

    public void BeginFadeToClear()
    {
        StartCoroutine(FadeToClear(fadeStartTime + 1));
    }

    private IEnumerator FadeToClear(float fadeStartTime)
    {
        loadingImage.enabled = true;

        for (float f = fadeStartTime; f >= 0; f -= ((0.1f * fadeMultiplier) * Time.deltaTime))
        {

            fadeProgress = f;
            Color c = loadingImage.color;
            c.a = f;
            loadingImage.color = c;
            yield return null;
        }

        loadingImage.enabled = false;
    }

    private IEnumerator FadeToBlack(float fadeStartTime, bool fadeToClearFlag)
    {
        loadingImage.enabled = true;

        for (float f = 0f; f <= fadeStartTime; f += ((0.1f * fadeMultiplier) * Time.deltaTime))
        {
            fadeProgress = f;
            Color c = loadingImage.color;
            c.a = f;
            loadingImage.color = c;
            yield return null;
        }
        // Included in the unlikely event that scenes will fade to black only. 
        if (fadeToClearFlag)
        {
            BeginFadeToClear();
        }
        else
        {
            
        }
    }

}
