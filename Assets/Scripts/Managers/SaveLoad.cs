using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveLoad : MonoBehaviour {

    private PlayerData playerData;

    void Start()
    {
        playerData = GameObject.Find("PlayerData").GetComponent<PlayerData>();
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
        //Debug.Log(Application.persistentDataPath.ToString() + "/playerInfo.dat");
        //string characterSelected = PlayerPrefs.GetString("CharacterSelected");
        //GameObject[] checkPoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        //int sceneToLoad = SceneManager.GetActiveScene().buildIndex;
        //string musicOn = SoundManager.Instance.MusicOn.ToString();
        //string sfxOn = SoundManager.Instance.SfxOn.ToString();

        Data data = new Data(playerData.CharacterSelected, playerData.CheckPoint, playerData.SceneToLoad, playerData.MusicOnBool, playerData.SfxOnBool);

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            Data savedData = (Data)bf.Deserialize(file);
            file.Close();

            if(savedData != null)
            {
                playerData.CharacterSelected = savedData.characterSelected;
                playerData.CheckPoint = savedData.checkPoint;
                playerData.SceneToLoad = savedData.sceneToLoad;
                playerData.MusicOnBool = savedData.musicOnBool;
                playerData.SfxOnBool = savedData.sfxOnBool;
            }
         
        }
    }

    public void Delete()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            File.Delete(Application.persistentDataPath + "/playerInfo.dat");
        }
    }

    [System.Serializable]
    public class Data
    {
        [SerializeField]
        public string characterSelected;
        public string checkPoint;
        public int sceneToLoad;
        public string musicOnBool;
        public string sfxOnBool;

        public Data(string _characterSelected, string _checkPoint, int _sceneToLoad, string _musicOn, string _sfxOn)
        {
            characterSelected = _characterSelected;
            checkPoint = _checkPoint;
            sceneToLoad = _sceneToLoad;
            musicOnBool = _musicOn;
            sfxOnBool = _sfxOn;
        }
    }
}



