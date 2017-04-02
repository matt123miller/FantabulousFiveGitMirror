using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Floating : MonoBehaviour {

    GameObject playerObj;
    Rigidbody playerRigid;
    MicrophoneInput microphoneInputScript;
    ThirdPersonCharacter characterScript;
    bool isFloating = false;
    bool canGoHigher = false;
    int canGoHighterCooldown;
    int cooldownLength;
    public float ceilingHeight;
    public float forceModifier;
    public float maxForce;
    public float characterHeight;
    public Text tempTimerText;
    public float timer;
    public float timerLength;
    public float scriptCooldown;
    public bool isCoolingDown;
    float scriptCooldownLength;
    public EventTrigger floatButton;

 

    // Use this for initialization
    void Start () {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        playerRigid = playerObj.GetComponent<Rigidbody>();
        characterScript = playerObj.GetComponent<ThirdPersonCharacter>();
        microphoneInputScript = GameObject.Find("GameManager").GetComponent<MicrophoneInput>();
        tempTimerText = GameObject.Find("Witch Prompt Text").GetComponent<Text>();
        floatButton = GameObject.Find("FloatButton").GetComponent<EventTrigger>();
        cooldownLength = 10;
        canGoHighterCooldown = 0;
        ceilingHeight = 4.5f;
        forceModifier = 0;
        maxForce = 100;
        characterHeight = 0;
        timerLength = 20.0f;
        timer = timerLength;
        scriptCooldownLength = 30.0f;
        scriptCooldown = scriptCooldownLength;
	}
	
	// Update is called once per frame
	void Update () {

 
        if(isFloating)
        {
            float loudness = microphoneInputScript.loudness;
            characterHeight = playerObj.transform.position.y;
            AdaptForceFromHeight(characterHeight);

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
            
            if(canGoHigher && canGoHighterCooldown <= 0 && loudness > 0.1f)
            {
                float forceToAdd = loudness * forceModifier;
                canGoHigher = false;
                canGoHighterCooldown = cooldownLength;
                playerRigid.AddForce(playerObj.transform.forward.x  * forceToAdd / 2, forceToAdd, playerObj.transform.forward.z * forceToAdd / 2 );

            }

           // characterScript.HandleAirborneMovement();
            canGoHighterCooldown--;
            timer -= Time.deltaTime;
            tempTimerText.text = timer.ToString();

            if(timer <= 0)
            {
                TurnOffFloating();
                tempTimerText.text = "Time's Up";
                timer = timerLength;
                isCoolingDown = true;
            }
        }//isfloating

        //timers to cooldown mechanic
        if (isCoolingDown)
        {
            isFloating = false;
            scriptCooldown -= Time.deltaTime;
            floatButton.enabled = false;
        }

        if (scriptCooldown <= 0)
        {
            isCoolingDown = false;
            scriptCooldown = scriptCooldownLength;
            tempTimerText.text = "";
            floatButton.enabled = true;
        }
    }


    public void FloatingTriggered()
    {
        if(!isCoolingDown)
        {
            isFloating = true;
            microphoneInputScript.StartInput();
        }
    }

    public void TurnOffFloating()
    {
        isFloating = false;
        microphoneInputScript.StopInput();
        isCoolingDown = true;
    }

    public void AdaptForceFromHeight(float _characterHeight)
    {
        if(characterHeight <= 1)
        {
            forceModifier = maxForce;
        }
        else
        {
            forceModifier = maxForce / characterHeight;
        }

    }
}
