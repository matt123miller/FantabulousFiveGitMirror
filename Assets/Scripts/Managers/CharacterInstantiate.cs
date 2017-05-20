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
    private PlayerData playerData;
    private string resourcesString;
    private Vector3? checkPoint;
    private Noise noiseScript;



    // Use this for initialization
    void Awake()
    {
        _scene = SceneManager.GetActiveScene();
        playerData = GameObject.Find("PlayerData").GetComponent<PlayerData>();
        resourcesString = "Prefabs/Scene Requirements/Character/";
        _spawnPoint = GameObject.FindWithTag("SpawnPoint");
        InstantiateCharacter();
    }

    void Start()
    {
    
    }

    public void InstantiateCharacter()
    {
        if (playerData == null)
        {
            _characterSelected = PlayerPrefs.GetString("CharacterSelected");
            SoundManager.Instance.MusicOn = SoundManager.Instance.convertSoundStrToBool(PlayerPrefs.GetString("MusicOn"));
            SoundManager.Instance.SfxOn = SoundManager.Instance.convertSoundStrToBool(PlayerPrefs.GetString("SFXOn"));
        }
        else
        {
            _characterSelected = playerData.CharacterSelected;
            checkPoint = playerData.ConvertCheckPointToVector(playerData.CheckPoint);
            SoundManager.Instance.MusicOn = SoundManager.Instance.convertSoundStrToBool(playerData.MusicOnBool);
            SoundManager.Instance.SfxOn = SoundManager.Instance.convertSoundStrToBool(playerData.SfxOnBool);
        }

        resourcesString += _characterSelected;


        if (_scene.name != "MainMenu" || _scene.name != "CharacterSelect")
        {
            _playerPrefab = (GameObject)Resources.Load(resourcesString, typeof(GameObject));

            if(checkPoint != new Vector3(0,0,0))
            {
                _player = Instantiate(_playerPrefab, checkPoint.Value, _spawnPoint.transform.rotation) as GameObject;
            }
            else if (_spawnPoint)
            {
                _player = Instantiate(_playerPrefab, _spawnPoint.transform.position, _spawnPoint.transform.rotation) as GameObject;
                //GlobalGameManager.Instance.PlayerTransform = _player.transform;
            }
        }
    }


}
