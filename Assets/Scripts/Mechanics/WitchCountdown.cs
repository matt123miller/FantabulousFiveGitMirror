using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WitchCountdown : MonoBehaviour {

    private Text _witchPromptText;
    private WitchKeepStill _keepStillScript;

	// Use this for initialization
	void Start () {
        _witchPromptText = GameObject.Find("Witch Prompt Text").GetComponent<Text>();
        _witchPromptText.text = " ";
        _keepStillScript = GameObject.Find("MechanicsScripts").GetComponent<WitchKeepStill>();
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
                _witchPromptText.text = "THE WITCH IS COMING!";
            }
            else
            {
                _witchPromptText.text = i.ToString();
            }

            yield return new WaitForSecondsRealtime(1f);
        }

        _witchPromptText.text = "STAY STILL!";

       StartCoroutine(_keepStillScript.KeepStillTime());
    }


}
