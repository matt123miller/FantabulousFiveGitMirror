using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour {

    SaveLoad saveLoadScript;
    PlayerData data;
    SceneTransitionManager transitionManager;

    void Start()
    {
        saveLoadScript = GameObject.Find("GameManager").GetComponent<SaveLoad>();
        transitionManager = GameObject.Find("GameManager").GetComponent<SceneTransitionManager>();
    }

    public void LoadSavedLevel(bool inGame)
    {
        data = saveLoadScript.Load();
        if(data!= null)
        {
            transitionManager.LoadTargetLevel(data.SceneToLoad);
        }
        else if (data == null && inGame)
        {
            transitionManager.ReloadCurrentLevel();
        }
    }

}
