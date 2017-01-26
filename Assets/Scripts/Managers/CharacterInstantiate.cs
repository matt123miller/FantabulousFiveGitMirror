using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CharacterInstantiate : MonoBehaviour
{

    private GameObject _playerPrefab;
    private GameObject _player;
    private string _characterSelected;
    private GameObject _spawnPoint;
    private Scene _scene;


	
    // Use this for initialization
    void Awake()
    {
        _scene = SceneManager.GetActiveScene();

        if (_scene.name != "MainMenu" || _scene.name != "Tish Test")
        {
            string resourcesString = "Prefabs/Scene Requirements/Character/";
            _spawnPoint = GameObject.FindWithTag("SpawnPoint");
            _characterSelected = PlayerPrefs.GetString("CharacterSelected");
            resourcesString += _characterSelected;
            _playerPrefab = (GameObject)Resources.Load(resourcesString, typeof(GameObject));
            if (_spawnPoint)
            {
                _player = Instantiate(_playerPrefab, _spawnPoint.transform.position, Quaternion.identity) as GameObject;
                //GlobalGameManager.Instance.PlayerTransform = _player.transform;
            }
        }

    }

}
