using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    [Tooltip("To begin immediately use 0, increase it to delay the start time")]
    [SerializeField]
    private float _fadeStartTime = 0;
    [SerializeField]
    private float _fadeMultiplier = 5;
    public float fadeProgress = 0;
    public bool turnUIOnAfter = true;

	// These are the components manipulated by the class.
    private Image _fadingImage;
    private Text _loadingText;
    private Slider _loadingSlider;
    private Image _loadingSliderImage;


    void OnEnable()
    {
        _fadingImage = GetComponent<Image>();
        _loadingSlider = GetComponentInChildren<Slider>();
        _loadingText = GetComponentInChildren<Text>();
        _loadingSliderImage = _loadingSlider.gameObject.GetComponentInChildren<Image>();

        _fadingImage.enabled = true;
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
        _loadingText.gameObject.SetActive(set);
        _loadingText.enabled = set;
        _loadingSlider.gameObject.SetActive(set);
        _loadingSlider.enabled = set;
        _loadingSliderImage.gameObject.SetActive(set);
        _loadingSliderImage.enabled = set;
    }

    public void UpdateSlider(float progress)
    {
        _loadingSlider.value = progress;
        _loadingText.color = new Color(_loadingText.color.r, _loadingText.color.g, _loadingText.color.b, Mathf.PingPong(Time.time, 1));
    }

    public void BeginFadeToBlack(bool fadeToClearFlag)
    {
        GlobalGameManager.Instance.ToggleUI(false);
        StartCoroutine(FadeToBlack(_fadeStartTime, fadeToClearFlag));
    }

    public void BeginFadeToClear()
    {
        StartCoroutine(FadeToClear(_fadeStartTime));
    }

    private IEnumerator FadeToClear(float fadeStartTime)
    {
        _fadingImage.enabled = true;
        fadeStartTime += 1;

        for (float f = fadeStartTime; f >= 0; f -= ((0.1f * _fadeMultiplier) * Time.deltaTime))
        {
            fadeProgress = f;
            Color c = _fadingImage.color;
            c.a = f;
            _fadingImage.color = c;
            yield return null;
        }

        _fadingImage.enabled = false;


        if (turnUIOnAfter)
        {
            GlobalGameManager.Instance.ToggleUI(true);
        }
    }

    private IEnumerator FadeToBlack(float fadeStartTime, bool fadeToClearFlag)
    {
        _fadingImage.enabled = true;
        fadeStartTime += 1;

        for (float f = 0f; f <= fadeStartTime; f += ((0.1f * _fadeMultiplier) * Time.deltaTime))
        {
            fadeProgress = f;
            Color c = _fadingImage.color;
            c.a = f;
            _fadingImage.color = c;
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