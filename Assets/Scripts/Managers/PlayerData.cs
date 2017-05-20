using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;


public class PlayerData : MonoBehaviour {

    private string characterSelected;
    private string checkPoint;
    private int sceneToLoad;
    private string musicOnBool;
    private string sfxOnBool;
    private float noiseAmount;
    private SaveLoad saveLoadScript;

    public PlayerData(string _characterSelected, string _checkPoint, int _sceneToLoad, string _musicOn, string _sfxOn, float _noiseAmount)
    {
        CharacterSelected = _characterSelected;
        CheckPoint = _checkPoint;
        SceneToLoad = _sceneToLoad;
        MusicOnBool = _musicOn;
        SfxOnBool = _sfxOn;
        NoiseAmount = _noiseAmount;
    }

    void Start()
    {
        GameObject.DontDestroyOnLoad(gameObject);
        //subscribing to scene loaded event to ensure only one player data object is in the main menu scene
        //handles returning to main menu scene after being in game
        SceneManager.sceneLoaded += RemoveExtraPlayerDataObj;
        saveLoadScript = GameObject.Find("GameManager").GetComponent<SaveLoad>();

        ResetValues();
        MusicOnBool = true.ToString();
        SfxOnBool = true.ToString();

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            saveLoadScript.Load();
        }

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

    public float NoiseAmount
    {
        get
        {
            return noiseAmount;
        }

        set
        {
            noiseAmount = value;
        }
    }

    public void RemoveExtraPlayerDataObj(Scene _scene, LoadSceneMode _mode)
    {
        if(_scene.buildIndex == 0)
        {
            GameObject[] playerDataObjs = GameObject.FindGameObjectsWithTag("PlayerData");

            if(playerDataObjs.Length > 1)
            {
                for(int i = 1; i < playerDataObjs.Length; i++)
                {
                    Destroy(playerDataObjs[i]);
                }
            }
        }
    }

    public Vector3 ConvertCheckPointToVector(string _checkpointString)
    {
        if (_checkpointString != null && _checkpointString != "None")
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

        return new Vector3(0,0,0);
    }

   public void ResetValues()
    {
        CharacterSelected = "Not Selected";
        CheckPoint = "None";
        SceneToLoad = -1;
        NoiseAmount = 0;
    }
}


