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

    public PlayerData(string _characterSelected, string _checkPoint, int _sceneToLoad)
    {
        characterSelected = _characterSelected;
        checkPoint = _checkPoint;
        sceneToLoad = _sceneToLoad;
    }


}
