using UnityEngine;
using System.Collections;

public class Floating : MonoBehaviour {

    GameObject playerObj;
    Rigidbody playerRigid;
    MicrophoneInput microphoneInputScript;
    bool isFloating = false;
    bool canGoHigher = false;
    int canGoHighterCooldown;
    int cooldownLength;
    public float ceilingHeight;
    public float forceModifier;
	// Use this for initialization
	void Start () {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        playerRigid = playerObj.GetComponent<Rigidbody>();
        microphoneInputScript = GameObject.Find("GameManager").GetComponent<MicrophoneInput>();
        cooldownLength = 30;
        canGoHighterCooldown = 0;
        ceilingHeight = 4.5f;
        forceModifier = 100;
	}
	
	// Update is called once per frame
	void Update () {

 
        if(isFloating)
        {
            float loudness = microphoneInputScript.loudness;
            print("Loudness: " + loudness);
            if (playerRigid.velocity.magnitude < 0f)
            {
                playerRigid.drag = 15;
            }
            else
            {
                playerRigid.drag = 5;
            }
            if (playerObj.transform.position.y < 4.5f)
            {
                canGoHigher = true;
            }
            else
            {
                canGoHigher = false;
            }
            
            if(canGoHigher && canGoHighterCooldown <= 0 && loudness > 1f)
            {
                canGoHigher = false;
                canGoHighterCooldown = cooldownLength;
                playerRigid.AddRelativeForce(0, loudness * forceModifier, 0);

            }

            canGoHighterCooldown--;
        }
	}


    public void FloatingTriggered()
    {
        isFloating = true;
        microphoneInputScript.StartInput();
    }

    public void TurnOffFloating()
    {
        isFloating = false;
        microphoneInputScript.StopInput();
    }
}
