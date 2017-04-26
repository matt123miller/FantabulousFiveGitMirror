using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour {

    private SaveLoad saveLoadScript;
    private PlayerData playerData;
    private SceneTransitionManager transitionManager;

    void Start()
    {
        saveLoadScript = GameObject.Find("GameManager").GetComponent<SaveLoad>();
        transitionManager = GameObject.Find("GameManager").GetComponent<SceneTransitionManager>();
        playerData = GameObject.Find("PlayerData").GetComponent<PlayerData>();
    }

    public void LoadSavedLevel(bool _inGame)
    {
        //data = saveLoadScript.Load();
        //if(data!= null)
        //{
        //    
        //}
        //else if (data == null && inGame)
        //{
        //    transitionManager.ReloadCurrentLevel();
        //}
        if (_inGame)
        {
            transitionManager.ReloadCurrentLevel();

        }
        else
        {
            if(playerData.SceneToLoad != -1)
            {
                transitionManager.LoadTargetLevel(playerData.SceneToLoad);
            }
        }
  
    }

}
