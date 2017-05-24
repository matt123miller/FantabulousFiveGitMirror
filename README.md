
# Fantabulous Five - Prototype Mobile Game


This page will include links to Youtube videos, accessible by clicking the thumbnail like the one shown below.

```
[![Put a little welcome video here.](https://img.youtube.com/vi/VIDEO ID/0.jpg)](https://www.youtube.com/watch?v=VIDEO ID)
```

# Contents

Here will be links to each section I hope


## Introduction

This project tasked our team with collaborating on the design, development of a theoretically release worthy prototype in eight months. Working together within a multidisciplinary team, we possessed a range of skills including 3d modelling and animation, programming and game design, supporting each other throughout the process. 

This page will document the designs and planned outcome of the project, followed by an evaluation of the prototype and my role in its creation. I fulfilled the roles of programmer and game designer, this page evaluates the project from those perspectives.


First, the programming team.
* Myself, Matt 
* Tish, also our Producer

The art team, responsible for creating, and texturing all our 3D models. Also, where appropriate, animating and rigging them.
* Ana
* Maytham
* Aron 

Before we began brainstroming ideas for the project, we knew we wanted to achieve certain goals.

* Developed in Unity using C#
* Targeting Android phones, as we each had access to Android devices
* Create immersive interactions made possible by the device format
* Use the gyroscope and microphone where possible for interesting interactions

We quickly settled on the following idea.

The game is set in the house of the (presumably evil) Witch. Bought to life by some unknown sorcery, players embark on a journey to escape the house without arousing the suspicion of the Witch. Each level takes place in a different room, each a different part of the house. The level developed for this early prototype is the Witch's study, a centre for her magical work.

## Technical Project Management

* Git source control
* Train everyone in using Git and SourceTree
* Cloud Build
* Setup the project structure, build settings etc.
* Be on hand to all team members for technical support
* Try to maintian good code practices and patterns and SRP etc.

Video goes here

## Code Quality and Style

## Manager Objects


## Asynchronous Scene Loading

### Fading Screen

## User Interface

Focus on easy to use, extendable UI.

Panel all the things

Multiple screen resolutions considered

### Main Menu

### Game HUD

## Gyroscope



```csharp
// found at https://github.com/matt123miller/FantabulousFiveGitMirror/blob/master/Assets/Scripts/DeviceScripts/DeviceRotation.cs
public static class DeviceRotation
{
    // Call DeviceRotation.GetRotation() for all your tilt phone needs.
    // You will have to offset for the phones idea of forward though. 
    private static bool _gyroInitialized = false;

    public static bool HasGyroscope
    {
        get
        {
            return SystemInfo.supportsGyroscope;
        }
    }

    public static Quaternion GetRotation()
    {
        if (!_gyroInitialized)
        {
            InitGyro();
        }

        return HasGyroscope
            ? ReadGyroscopeRotation()
            : Quaternion.identity;
    }

    private static void InitGyro()
    {
        if (HasGyroscope)
        {
            Input.gyro.enabled = true;                // enable the gyroscope
            Input.gyro.updateInterval = 0.0167f;      // set the update interval to it's highest value (60 Hz)
        }
        _gyroInitialized = true;
    }

    // This is the real magic. I have no idea how it works but it does!
    private static Quaternion ReadGyroscopeRotation()
    {
        return new Quaternion(0.5f, 0.5f, -0.5f, 0.5f) * Input.gyro.attitude * new Quaternion(0, 0, 1, 0);
    }
}
```

### Free Rotating Camera

### Balance Beam

## Immersive Environmental Interaction

### Magical Portrait System

Just like Harry Potter
```
Find image of the portraits
```

### Tapping Objects

## Artificial Intelligence

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

