using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CharacterInstantiate : MonoBehaviour
{

    private GameObject playerPrefab;
    private GameObject player;
    private string characterSelected;
    private GameObject spawnPoint;
    private Scene scene;

<<<<<<< HEAD
	// Use this for initialization
	void Awake () {
=======
    // Use this for initialization
    void Awake()
    {
>>>>>>> 3c971a98ee7672be9fa3e8005ba27e8644841f31

        scene = SceneManager.GetActiveScene();

        if (scene.name != "MainMenu" || scene.name != "Tish Test")
        {
            string resourcesString = "Prefabs/Scene Requirements/Character/";
            spawnPoint = GameObject.FindWithTag("SpawnPoint");
            characterSelected = PlayerPrefs.GetString("CharacterSelected");
            resourcesString += characterSelected;
            playerPrefab = (GameObject)Resources.Load(resourcesString, typeof(GameObject));
            if (spawnPoint)
            {
                player = Instantiate(playerPrefab, spawnPoint.transform.position, Quaternion.identity) as GameObject;
                GlobalGameManager.Instance.PlayerTransform = player.transform;
            }
        }

    }

}
