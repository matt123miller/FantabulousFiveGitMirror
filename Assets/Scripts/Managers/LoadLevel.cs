using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour {

    SaveLoad saveLoadScript;
    PlayerData data;
    SceneTransitionManager transitionManager;

    void Start()
    {
        saveLoadScript = GameObject.Find("GameManager").GetComponent<SaveLoad>();
        transitionManager = GameObject.Find("GameManager").GetComponent<SceneTransitionManager>();
    }

    public void LoadSavedLevel()
    {
        data = saveLoadScript.Load();
        if(data!= null)
        {
            transitionManager.LoadTargetLevel(data.SceneToLoad);
        }

    }

}
