using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

[Serializable]
public class PlayerData {

    private string characterSelected;
    private string checkPoint;
    private int sceneToLoad;
    private string musicOnBool;
    private string sfxOnBool;

    public PlayerData(string _characterSelected, string _checkPoint, int _sceneToLoad, string _musicOn, string _sfxOn)
    {
        CharacterSelected = _characterSelected;
        CheckPoint = _checkPoint;
        SceneToLoad = _sceneToLoad;
        MusicOnBool = _musicOn;
        SfxOnBool = _sfxOn;
    }

    public string CharacterSelected
    {
        get
        {
            return characterSelected;
        }

        set
        {
            characterSelected = value;
        }
    }

    public string CheckPoint
    {
        get
        {
            return checkPoint;
        }

        set
        {
            checkPoint = value;
        }
    }

    public int SceneToLoad
    {
        get
        {
            return sceneToLoad;
        }

        set
        {
            sceneToLoad = value;
        }
    }

    public string MusicOnBool
    {
        get
        {
            return musicOnBool;
        }

        set
        {
            musicOnBool = value;
        }
    }

    public string SfxOnBool
    {
        get
        {
            return sfxOnBool;
        }

        set
        {
            sfxOnBool = value;
        }
    }

    public Vector3 convertCheckPointToVector(string _checkpointString)
    {
        // Remove the parentheses
        if (_checkpointString.StartsWith("(") && _checkpointString.EndsWith(")"))
        {
            _checkpointString = _checkpointString.Substring(1, _checkpointString.Length - 2);
        }

        // split the items
        string[] sArray = _checkpointString.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }

   
}
