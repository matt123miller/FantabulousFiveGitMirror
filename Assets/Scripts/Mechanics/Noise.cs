using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Noise : MonoBehaviour
{

    public float currentNoise;
    public float maxNoise = 100f;
    public Slider noiseBar;
    private WitchCountdown _witchCountDownScript;

    public void Start()
    {
        _witchCountDownScript = GameObject.Find("MechanicsScripts").GetComponent<WitchCountdown>();
        noiseBar = GetComponent<Slider>();

        currentNoise = 0;
        SetNoiseBar(currentNoise);
        //InvokeRepeating("DecreaseNoise", 1f, 1f);
    }


    //public void DecreaseNoise()
    //{
    //    currentNoise += 2;
    //    SetNoiseBar(CalculateNoiseVal(currentNoise)); //making sure that noise value is set to value between 0 - 1 before setting UI
    //}

    public void AddToNoise(float moreNoise)
    {
       // print("Noise increasing by " + moreNoise);
        currentNoise += moreNoise;
        SetNoiseBar(CalculateNoiseVal(currentNoise));
        if (currentNoise >= maxNoise) // Should this be maxNoise?
        {
            TriggerWitch();
            ResetNoiseBar();
        }

    }

    public void SetNoiseBar(float noiseVal)
    {
        //noiseBar needs to be a value between 0 - 1
        noiseBar.value = noiseVal;
    }

    //makes sure that noise value is between 0 - 1 for noiseBar
    public float CalculateNoiseVal(float currentNoiseVal)
    {
        return currentNoiseVal / maxNoise;
    }

    public void TriggerWitch()
    {
        _witchCountDownScript.startGetReadyCountdown();
    }

    public void ResetNoiseBar()
    {
        currentNoise = 0;
        //noiseBar.transform.localScale = new Vector3(0, noiseBar.transform.localScale.y, noiseBar.transform.localScale.z);
        SetNoiseBar(currentNoise);
    }
}
