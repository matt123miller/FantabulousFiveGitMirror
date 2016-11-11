using UnityEngine;
using System.Collections;

public class CharacterSelect : MonoBehaviour {

	
    public void ConfirmCharacter(GameObject characterSelected)
    {
        PlayerPrefs.SetString("CharacterSelected", characterSelected.name);
    }


}
