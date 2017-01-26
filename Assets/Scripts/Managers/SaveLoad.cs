using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveLoad : MonoBehaviour {

    public void Save(Vector3 _checkPoint)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
        Debug.Log(Application.persistentDataPath.ToString() + "/playerInfo.dat");
        string characterSelected = PlayerPrefs.GetString("CharacterSelected");
        GameObject[] checkPoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        int sceneToLoad = SceneManager.GetActiveScene().buildIndex;

        PlayerData data = new PlayerData(characterSelected, _checkPoint.ToString(), sceneToLoad);

        bf.Serialize(file, data);
        file.Close();
    }

    public PlayerData Load()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();
            return data;
        }

        return null;
    }


}
