using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UnityStandardAssets.CrossPlatformInput
{
    public class Floating : MonoBehaviour
    {
        [SerializeField]
        string floatString;

        GameObject playerObj;
        Rigidbody playerRigid;
        MicrophoneInput microphoneInputScript;
        ThirdPersonCharacter characterScript;
        public bool isFloating = false;
        bool canGoHigher = false;
        int canGoHighterCooldown;
        int cooldownLength;
        public float ceilingHeight;
        public float forceModifier;
        float maxForce;
        public float maxForceValue;
        public float characterHeight;
        public Text tempTimerText;
        public float timer;
        public float timerLength;
        public float scriptCooldown;
        public bool isCoolingDown;
        float scriptCooldownLength;
        public EventTrigger floatButton;



        // Use this for initialization
        void Start()
        {
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
            maxForceValue = 100;
            maxForce = maxForceValue;
            characterHeight = 0;
            timerLength = 20.0f;
            timer = timerLength;
            scriptCooldownLength = 15.0f;
            scriptCooldown = scriptCooldownLength;
        }

        // Update is called once per frame
        void Update()
        {

            if (CrossPlatformInputManager.GetButton(floatString))
            {
                FloatingTriggered();
            }
            else if (CrossPlatformInputManager.GetButtonUp(floatString))
            {
                TurnOffFloating();
            }
            if (isFloating)
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
                // playerRigid.drag = playerRigid.velocity.magnitude < 0f ? 15 : 5;
                
                if (playerObj.transform.position.y < 4.5f)
                {
                    canGoHigher = true;
                }
                else
                {
                    canGoHigher = false;
                }
                // You can do this
                //canGoHigher = playerObj.transform.position.y < 4.5f

                if (canGoHigher && canGoHighterCooldown <= 0 && loudness > 0.1f)
                {
                    float forceToAdd = loudness * forceModifier;
                    canGoHigher = false;
                    canGoHighterCooldown = cooldownLength;
                    playerRigid.AddForce(playerObj.transform.forward.x * forceToAdd / 2, forceToAdd, playerObj.transform.forward.z * forceToAdd / 2);

                }

                // characterScript.HandleAirborneMovement();
                canGoHighterCooldown--;
                timer -= Time.deltaTime;
                tempTimerText.text = timer.ToString();

                if (timer <= 0)
                {
                    TurnOffFloating();
                    tempTimerText.text = "Time's Up";
                    isCoolingDown = true;
                }
            }//isfloating

            //timers to cooldown mechanic
            if (isCoolingDown)
            {
                isFloating = false;
                scriptCooldown -= Time.deltaTime;
                tempTimerText.text = "Cooldown: " + scriptCooldown + "s";
                TurnOffFloating();
                //  floatButton.enabled = false;
            }

            if (scriptCooldown <= 0)
            {
                isCoolingDown = false;
                scriptCooldown = scriptCooldownLength;
                tempTimerText.text = "";
                Reset();
                //floatButton.enabled = true;
            }
        }


        public void FloatingTriggered()
        {
            if (!isCoolingDown && !isFloating)
            {
                isFloating = true;
                microphoneInputScript.StartInput();
            }
        }

        public void TurnOffFloating()
        {
            if (isFloating)
            {
                isFloating = false;
                microphoneInputScript.StopInput();
                Reset();
                isCoolingDown = true;
            }

        }

        public void AdaptForceFromHeight(float _characterHeight)
        {
            if (_characterHeight <= 1)
            {
                forceModifier = maxForce;
            }
            else
            {
                forceModifier = maxForce / _characterHeight;
            }

        }
        public void Reset()
        {
            timer = timerLength;
            maxForce = maxForceValue;
            characterHeight = 0;
            forceModifier = 0;
            playerRigid.drag = 0;
        }
    }
}
