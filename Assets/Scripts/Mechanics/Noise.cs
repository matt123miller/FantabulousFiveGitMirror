using UnityEngine;
using System.Collections;

public class Noise : MonoBehaviour
{

    public float currentNoise;
    public float maxNoise = 100f;
    public GameObject noiseBar;
    private WitchCountdown witchCountDownScript;

    public void Start()
    {
        currentNoise = 0;
        SetNoiseBar(currentNoise);
        witchCountDownScript = GameObject.Find("MechanicsScripts").GetComponent<WitchCountdown>();
        //InvokeRepeating("DecreaseNoise", 1f, 1f);
    }


    //public void DecreaseNoise()
    //{
    //    currentNoise += 2;
    //    SetNoiseBar(CalculateNoiseVal(currentNoise)); //making sure that noise value is set to value between 0 - 1 before setting UI
    //}

    public void AddToNoise(float moreNoise)
    {
        currentNoise += moreNoise;
        SetNoiseBar(CalculateNoiseVal(currentNoise));
        if (currentNoise >= 100)
        {
            TriggerWitch();
            ResetNoiseBar();
        }

    }

    public void SetNoiseBar(float noiseVal)
    {
        //noiseBar needs to be a value between 0 - 1
        noiseBar.transform.localScale = new Vector3(noiseVal, noiseBar.transform.localScale.y, noiseBar.transform.localScale.z);
    }

    //makes sure that noise value is between 0 - 1 for noiseBar
    public float CalculateNoiseVal(float currentNoiseVal)
    {
        float calculatedNoise = currentNoiseVal / maxNoise;
        return calculatedNoise;
    }

    public void TriggerWitch()
    {
        witchCountDownScript.startGetReadyCountdown();
    }

    public void ResetNoiseBar()
    {
        currentNoise = 0;
        noiseBar.transform.localScale = new Vector3(0, noiseBar.transform.localScale.y, noiseBar.transform.localScale.z);
    }
}
