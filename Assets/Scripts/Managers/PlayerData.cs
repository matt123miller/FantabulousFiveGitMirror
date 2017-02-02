using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

[Serializable]
public class PlayerData {

    private string _characterSelected;
    private string _checkPoint;
    private int _sceneToLoad;

    public PlayerData(string characterSelected, string checkPoint, int sceneToLoad)
    {
        CharacterSelected = characterSelected;
        CheckPoint = checkPoint;
        SceneToLoad = sceneToLoad;
    }

    public string CharacterSelected
    {
        get
        {
            return _characterSelected;
        }

        set
        {
            _characterSelected = value;
        }
    }

    public string CheckPoint
    {
        get
        {
            return _checkPoint;
        }

        set
        {
            _checkPoint = value;
        }
    }

    public int SceneToLoad
    {
        get
        {
            return _sceneToLoad;
        }

        set
        {
            _sceneToLoad = value;
        }
    }

    public Vector3 convertCheckPointToVector(string checkpointString)
    {
        // Remove the parentheses
        if (checkpointString.StartsWith("(") && checkpointString.EndsWith(")"))
        {
            checkpointString = checkpointString.Substring(1, checkpointString.Length - 2);
        }

        // split the items
        string[] sArray = checkpointString.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }
}
