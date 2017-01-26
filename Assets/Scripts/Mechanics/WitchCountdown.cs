using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WitchCountdown : MonoBehaviour {

    Text witchPromptText;
    WitchKeepStill keepStillScript;

	// Use this for initialization
	void Start () {
        witchPromptText = GameObject.Find("Witch Prompt Text").GetComponent<Text>();
        witchPromptText.text = " ";
        keepStillScript = GameObject.Find("MechanicsScripts").GetComponent<WitchKeepStill>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void startGetReadyCountdown()
    {
        StartCoroutine(GetReadyForWitch());
    }

    //Get Ready Countdown timer and then fires co-routine for keeping still
    public IEnumerator GetReadyForWitch()
    {
        for (int i = 4; i >= 1; i--)
        {
            if(i == 4)
            {
                witchPromptText.text = "THE WITCH IS COMING!";
            }
            else
            {
                witchPromptText.text = i.ToString();
            }

            yield return new WaitForSecondsRealtime(1f);
        }

        witchPromptText.text = "STAY STILL!";

       StartCoroutine(keepStillScript.KeepStillTime());
    }


}
