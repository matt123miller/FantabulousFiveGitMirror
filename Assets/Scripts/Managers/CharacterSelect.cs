using UnityEngine;
using System.Collections;

public class CharacterSelect : MonoBehaviour {

    private PlayerData playerData;

    void Start()
    {
        playerData = GameObject.Find("PlayerData").GetComponent<PlayerData>();
    }
	
    public void ConfirmCharacter(GameObject characterSelected)
    {
        //PlayerPrefs.SetString("CharacterSelected", characterSelected.name);
        playerData.CharacterSelected = characterSelected.name;
    }


}
