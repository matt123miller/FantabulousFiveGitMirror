
# Fantabulous Five - Prototype Mobile Game


```
[![Put a little welcome video here.](https://img.youtube.com/vi/VIDEO ID/0.jpg)](https://www.youtube.com/watch?v=VIDEO ID)
```

# Contents

Here will be links to each section I hope


## Introduction

This project tasked our team with collaborating on the design and development of a theoretically release worthy prototype in eight months. Working together within a multidisciplinary team, we possessed a range of skills including 3d modelling and animation, programming and game design, supporting each other throughout the process. 

This page will document the designs and planned outcome of the project, followed by an evaluation of the prototype and my role in its creation. I fulfilled the roles of programmer and game designer, this page evaluates the project from those perspectives. Where relevant I have included code snippets of interesting parts. You're more than welcome to browse the code in full by [checking the repo here.](https://github.com/matt123miller/FantabulousFiveGitMirror/tree/master/Assets/Scripts)

This page will include links to Youtube videos, accessible by clicking the thumbnail like the one shown below. Please click that now!

[![welcome video here.](https://img.youtube.com/vi/rVGuKL1mfjE/0.jpg)](https://www.youtube.com/watch?v=rVGuKL1mfjE)

First, the programming team.
* Myself, Matt 
* Tish, also our Producer

The art team, responsible for creating, and texturing all our 3D models. Also, where appropriate, animating and rigging them.
* Ana
* Maytham
* Aron 

Before we began brainstorming ideas for the project, we knew we wanted to achieve certain goals.

* Developed in Unity using C#
* Targeting Android phones, as we each had access to Android devices
* Create immersive interactions made possible by the device format
* Use the gyroscope and microphone where possible for interesting interactions

We quickly settled on the following idea.

The game is set in the house of the (presumably evil) Witch. Bought to life by some unknown sorcery, players embark on a journey to escape the house without arousing the suspicion of the Witch. Each level takes place in a different room, each a different part of the house. The level developed for this early prototype is the Witch's study, a centre for her magical work. 

The following video was recorded by Tish and gives a quick overview of the game and each mechanic created.

[![Overview video.](https://img.youtube.com/vi/G2uLNMtMSz0/0.jpg)](https://www.youtube.com/watch?v=G2uLNMtMSz0)

## Technical Project Management

* Unity
* Git source control
* Train everyone in using Git and SourceTree
* Cloud Build
* Setup the project structure, build settings etc.
* Be on hand to all team members for technical support regarding Git and SourceTree
 
[![Overview video.](https://img.youtube.com/vi/l7lVipuXxNo/0.jpg)](https://www.youtube.com/watch?v=l7lVipuXxNo)

## Code Quality and Style

* Try to maintian good code practices and patterns and SRP etc.

Video goes here

## Manager Objects
## What about them MATT????!?!?!?1
Talk about patterns, singletons pro cons, we could've used DontDestroyOnLoad() but didn't because we had a lot of things relying on start() etc. In hindsight I would've hooked into the sceneLoaded event https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager-sceneLoaded.html

I'm making a whole video about events later 
## Asynchronous Scene Loading

This is on of the first systems I created. Finishing a foundational part of the experience early on allowed me to focus development time on other mechanics as they arose. Finishing this early also meant that I could create a solid foundation to build upon, allowing other systems to hook into this as necessary. It was intentionally written to only expose the minimum methods required to hook into the scene loading functionality.Thef ollowing video discusses the  process in detail and includes a discussion of the related system I wrote for fading the screen to and from black (though and colour could be used)

[![Overview video.](https://img.youtube.com/vi/SaFemLB8ilM/0.jpg)](https://www.youtube.com/watch?v=SaFemLB8ilM)

```csharp
https://github.com/matt123miller/FantabulousFiveGitMirror/blob/master/Assets/Scripts/Managers/SceneTransitionManager.cs

public class SceneTransitionManager : MonoBehaviour
{
    /*
    ...
    ...
    ...
    */

    /// <summary>
    /// Asynchronously begins loading the chosen level in the build settings
    /// Assumes that the class calling the method knows which scene it wants to go to.
    /// </summary>
    /// <param name="targetScene"></param>
    public void LoadTargetLevel(int targetScene)
    {
        if (targetScene >= SceneManager.sceneCountInBuildSettings)
        {
            // returns to main menu.
            StartCoroutine(AsyncLoadLevel(0));
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
        
        if (targetScene >= SceneManager.sceneCountInBuildSettings)
        {
            // There are no scenes left. Maybe return to main menu?
            // Handle this error however you like.
            StartCoroutine(AsyncLoadLevel(0)); // returns to the main menu

        }
        else 
        {
            StartCoroutine(AsyncLoadLevel(targetScene));
        }
    }

    public void ReloadCurrentLevel()
    {
        int targetScene = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(AsyncLoadLevel(targetScene));
    }

    private IEnumerator AsyncLoadLevel(int targetScene)
    {
        fader.BeginFadeToBlack(false);

        while (fader.fadeProgress < 0.95)
        {
            yield return null;
        }
                
        fader.ToggleLoadingUIOn(true);
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene);

        while (!asyncLoad.isDone)
        {
            // Here you put your loading screen code.
            fader.UpdateSlider(asyncLoad.progress);

            yield return null;
        }
        
        // You don't need to turn the text and slider back off or call BeginFadeToClear() here as the old scene will now be destroyed.
        // The new scene that was just loaded asynchonously will replace it and should have a SceneManager object in it to handle fading etc.
    }
}
```
```csharp
https://github.com/matt123miller/FantabulousFiveGitMirror/blob/master/Assets/Scripts/Camera/ScreenFade.cs

public class ScreenFade : MonoBehaviour
{
    /*
    ...
    ...
    ...
    */

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
```

I also created a simple prefab that will load the next level when players come into contact with it. It's bright pink to aid testing but the final version would of course be invisible, relying only on the trigger box.

[![https://gyazo.com/8c4fd1075ec9c0f68fc44ff6ce0730cb](https://i.gyazo.com/8c4fd1075ec9c0f68fc44ff6ce0730cb.png)](https://gyazo.com/8c4fd1075ec9c0f68fc44ff6ce0730cb)
## A discussion of multithreading versus coroutines

[![Overview video.](https://img.youtube.com/vi/P0kKQ6deo-I/0.jpg)](https://www.youtube.com/watch?v=P0kKQ6deo-I)

## User Interface
This includes the in game HUD, the main menu and strategies for creating and maintaining sane UI practices. 

* Focus on easy to use, extendable UI.
* Take advantage of panels for their organisational benefits, simplicity and flexibility.
* Multiple screen resolutions considered.

[![Overview video.](https://img.youtube.com/vi/_euQi67Bq6A/0.jpg)](https://www.youtube.com/watch?v=_euQi67Bq6A)


## Gyroscope Functionality


This video discusses quaternions and rotating the camera with the users phone, one of my favourite mechanics for both it's mechnical utility and novelty.


[![Rotating the phone.](https://img.youtube.com/vi/eXCnm6KLY2w/0.jpg)](https://www.youtube.com/watch?v=eXCnm6KLY2w)

```csharp
// found at the rest of this class at
// https://github.com/matt123miller/FantabulousFiveGitMirror/blob/master/Assets/Scripts/DeviceScripts/DeviceRotation.cs
public static class DeviceRotation
{
    /*
    ...
    ...
    ...
    */

    // This is the real magic!
    private static Quaternion ReadGyroscopeRotation()
    {
        return new Quaternion(0.5f, 0.5f, -0.5f, 0.5f) * Input.gyro.attitude * new Quaternion(0, 0, 1, 0);
    }
}
```

## Free Rotating Camera


```csharp

    [RequireComponent(typeof(Camera))]
    public class TiltCamera : MonoBehaviour
    {
        
        [SerializeField]
        string tiltString;
        // This class is applied to the camera, which is a child of the character controller
        // As such all rotations are localRotation, in relation to the parent object (character controller);
        [Tooltip("The speed the camera will return back to normal, higher is quicker."), Range(0.1f, 1)]
        public float lerpMultiplier = 0.5f;
        public AnimationCurve curve;
        public bool tilting = false;
        
        private Quaternion _resetRotation;
        private Quaternion _negatePhoneRotation;
        private Quaternion _currentRotation;
        private Quaternion _desiredRotation;


        // Use this for initialization
        void Start()
        {
            // Grab the current rotations as starting points.
            _resetRotation = transform.localRotation;
            _currentRotation = transform.localRotation;
            _desiredRotation = transform.localRotation;
            

            // Will this simple fix solve the first tilt on mobile?
            Input.gyro.enabled = true;
        }

        // Update is called once per frame
        void Update()
        {
            // Phone Input.
            if (CrossPlatformInputManager.GetButton(tiltString))
                TiltPhone();
            else if (CrossPlatformInputManager.GetButtonUp(tiltString))
                ResetRotation();
            
        }

        /// <summary>
        /// This rotates the camera to match the orientation of the phone
        /// </summary>
        public void TiltPhone()
        {
            // Helps to stop us getting caught in a bad change of states. Resets in ResetRotation().
            if (!tilting)
            {
                // Save our current rotation before doing anything else. This is where we'll return later.
                _resetRotation = transform.localRotation;
                // This is the opposite of the phones rotation when entering the tilt mode. 
                // We are aiming to negate by this value later.
                _negatePhoneRotation = Quaternion.Inverse(DeviceRotation.GetRotation());
                tilting = true;
                //debugText.enabled = true;
            }
            
            // None! This is 1 rotation offest by another. No idea how it works.
            // Why do you offset the right by the left? Who knows. It's magic.
            _desiredRotation = _negatePhoneRotation * DeviceRotation.GetRotation();

            // Set rotation at the end, assumes _desiredRotation has been set in 1 of the above if statements.
            transform.localRotation = _desiredRotation;
            // Cache it back into the conveniently shorter variable name.
            _currentRotation = transform.localRotation;
            //debugText.text = _desiredRotation.ToString();
        }

        public void ResetRotation()
        {
            // Include anything special that needs to be reset separate to the coroutine.
            tilting = false;
            //debugText.enabled = false;

            StartCoroutine("LerpCameraBack");
        }

        // This works perfectly in all my test so far.
        private IEnumerator LerpCameraBack()
        {
            // How do I find the return angle and rotate back to normal? So scared.
            // Quaternions are the devil
            _currentRotation = transform.localRotation;
            // We will return to this point. desiredRotaiton doesn't change throughout the whole coroutine.
            _desiredRotation = _resetRotation;
            float lerpCompletion = 0;
            
            while (lerpCompletion < 0.99) 
            {
                lerpCompletion += Time.smoothDeltaTime * lerpMultiplier;
                var curveValue = curve.Evaluate(lerpCompletion);

                transform.localRotation = Quaternion.Slerp(_currentRotation, _desiredRotation, curveValue);

                yield return null;
            }

            // The reset rotation has now finished, set any states and variables that need setting.

            // Sets the rotation back, compensate for any remaining angle changes.
            transform.localRotation = _desiredRotation;
        }
    }
```


In the video above discussing quaternions and phone rotation I don't describe the benefits of [animation curves](https://docs.unity3d.com/ScriptReference/AnimationCurve.html). Animation curves can be utilised for all manner of things that require changing a float value over time.

[![https://gyazo.com/b12ed89ba2d704cb963b50ccf8f486cd](https://i.gyazo.com/b12ed89ba2d704cb963b50ccf8f486cd.png)](https://gyazo.com/b12ed89ba2d704cb963b50ccf8f486cd)

# Write more about animation curves

## Balance Beam

Make a short video, add a code snippet.

## Immersive Environmental Interaction

### Magical Portrait System

[![Magical Porttrait System.](https://img.youtube.com/vi/fJjtDoapKW8/0.jpg)](https://www.youtube.com/watch?v=fJjtDoapKW8)

Just like Harry Potter.
```
Find image of the portraits
```

```csharp
public class MovingPortraits : MonoBehaviour
{
    // TODO Maybe turn the render camera off when it's far away from the player for optimisation?
    public Camera _renderCamera;
    private Transform _player;
    private float _invSqrColliderDistance;

    public Transform pictureTexture;

    [Range(0, 5)]
    [Header("Edit me!")]
    public int textureScale = 3;

    [Header("Not me!")]
    public int texWidth;
    public int texHeight;

    void Awake()
    {
        // Get my components
        _renderCamera = GetComponentInChildren<Camera>();

        // Create and set the render texture.
        RenderTexture rTex = new RenderTexture(texWidth, texHeight, 0);
        _renderCamera.targetTexture = rTex;
        pictureTexture.GetComponent<MeshRenderer>().material.mainTexture = rTex;

    }

    void Start()
    {
        
    }
    
    // This is called whenever you change a value in the inspector
    // Therefore it's used to alter the resolution width and height as users change the slider value.
    void OnValidate()
    {
        texWidth = (int)Mathf.Pow(2, 5 + textureScale);
        texHeight = (int)Mathf.Pow(2, 5 + textureScale);

    }
}
```

### Tapping Objects

[![Magical Porttrait System.](https://img.youtube.com/vi/ShIh0fCPSZI/0.jpg)](https://www.youtube.com/watch?v=ShIh0fCPSZI)

The video was unclear, when showing the set of "drawers" in our main scene, it was using the class and functionality I described in the previous scene. The code is shown below.

```csharp
// Find the class here
// This ensures that whatever object this class is attached to has an Animator and EventTrigger.
// This is useful to reduce the knowledge and memory requirements for others.
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EventTrigger))]
public class Tappable : MonoBehaviour , ITouchInteractive {

	public Color newColour = new Color(0,1,0);
    private Color _oldColour;

    public Animator animator;
    public string triggerName;
    public bool withinRange = false;

	// Use this for initialization
	void Start () {

        _oldColour = GetComponent<MeshRenderer>().material.color;
        animator = GetComponent<Animator>();
        if(GetComponent<SphereCollider>())
            GetComponent<SphereCollider>().radius = 1;
	}

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        withinRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        withinRange = false;
    }

    public void Interact()
    {
        if (!withinRange)
            return;

        print("Tap that");
        GetComponent<MeshRenderer>().material.color = newColour;

        animator.SetTrigger(triggerName);
    }

	public void Interact(Vector3 fingerPosition)
    {
        // Not showing in the EventHandler inspector?
	}

    public void FinishInteraction()
    {
        if (!withinRange)
            return;

        print("Finish tap");
        GetComponent<MeshRenderer>().material.color = _oldColour;
    }
}
```

Before deciding to use Event Triggers, raycasting was used as described in the video. It was hoped to have a variety of different interactions be powered by tapping and so I wrote a small set of systems to accomplish this using inheritance and polymorphism. [You can find the relevant code here](https://github.com/matt123miller/FantabulousFiveGitMirror/tree/master/Assets/Scripts/Touch%20Interaction), though the DragRigidbody.cs file was provided by Unity in the Standard Assets package.
## Artificial Intelligence

The code for the AI is quite long, so I won't include snippets here. However please feel free to read the code [for the controller](https://github.com/matt123miller/FantabulousFiveGitMirror/blob/master/Assets/Scripts/Character/AICharacterControl.cs) and for the [AIs vision.](https://github.com/matt123miller/FantabulousFiveGitMirror/blob/master/Assets/Scripts/Character/AISight.cs)

Lots to say here
Started with what we wanted the AI to achieve
Different states
Balance of an FSM vs a single class. Added complexity, FSM doesn't scale well
Method based
Interacts with noise
Scare the AI away
Editor configurable


## A little level design

Video flying around the level

## Known Problems



## Sources used

I acknowledge the use of the following sources when my own development for this prototype. 

[Unity documentation](https://docs.unity3d.com/Manual/index.html)

[Microsoft's C# documentation](https://docs.microsoft.com/en-gb/dotnet/)

Code for reading the [current rotation of the phone]() was provided by [codingChris on the Unity forums](http://answers.unity3d.com/comments/842596/view.html)

[Texture pack used for one wooden texture.](https://opengameart.org/content/free-texture-resource-pack-wood-structure-walls-and-textile?page=1)

[5 dark wood textures](https://opengameart.org/content/dark-wood-textures)

