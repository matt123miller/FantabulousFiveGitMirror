using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    [Tooltip("To begin immediately use 0, increase it to delay the start time")]
    [SerializeField]
    private float fadeStartTime = 0;
    [SerializeField]
    private float fadeMultiplier = 5;
    public float fadeProgress = 0;
    public bool turnUIOnAfter = true;

	// These are the components manipulated by the class.
    private Image fadingImage;
    private Text loadingText;
    private Slider loadingSlider;
    private Image loadingSliderImage;


    void OnEnable()
    {
        fadingImage = GetComponent<Image>();
        loadingSlider = GetComponentInChildren<Slider>();
        loadingText = GetComponentInChildren<Text>();
        loadingSliderImage = loadingSlider.gameObject.GetComponentInChildren<Image>();

        fadingImage.enabled = true;
    }

    void Start()
    {
        // Performed in Start to allow all variables to be cached first.
        ToggleLoadingUIOn(false);
        BeginFadeToClear();
        SceneTransitionManager.Instance.fader = this;
    }

    public void ToggleLoadingUIOn(bool set)
    {
        loadingText.gameObject.SetActive(set);
        loadingText.enabled = set;
        loadingSlider.gameObject.SetActive(set);
        loadingSlider.enabled = set;
        loadingSliderImage.gameObject.SetActive(set);
        loadingSliderImage.enabled = set;
    }

    public void UpdateSlider(float progress)
    {
        loadingSlider.value = progress;
        loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));
    }

    public void BeginFadeToBlack(bool fadeToClearFlag)
    {
        GlobalGameManager.Instance.ToggleUI(false);
        StartCoroutine(FadeToBlack(fadeStartTime, fadeToClearFlag));
    }

    public void BeginFadeToClear()
    {
        StartCoroutine(FadeToClear(fadeStartTime));
    }

    private IEnumerator FadeToClear(float fadeStartTime)
    {
        fadingImage.enabled = true;
        fadeStartTime += 1;

        for (float f = fadeStartTime; f >= 0; f -= ((0.1f * fadeMultiplier) * Time.deltaTime))
        {
            fadeProgress = f;
            Color c = fadingImage.color;
            c.a = f;
            fadingImage.color = c;
            yield return null;
        }

        fadingImage.enabled = false;


        if (turnUIOnAfter)
        {
            GlobalGameManager.Instance.ToggleUI(true);
        }
    }

    private IEnumerator FadeToBlack(float fadeStartTime, bool fadeToClearFlag)
    {
        fadingImage.enabled = true;
        fadeStartTime += 1;

        for (float f = 0f; f <= fadeStartTime; f += ((0.1f * fadeMultiplier) * Time.deltaTime))
        {
            fadeProgress = f;
            Color c = fadingImage.color;
            c.a = f;
            fadingImage.color = c;
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