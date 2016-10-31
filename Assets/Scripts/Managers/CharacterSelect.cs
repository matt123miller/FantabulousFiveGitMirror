using UnityEngine;
using System.Collections;

public class CharacterSelect : MonoBehaviour {

    private GameObject[] m_characterList;
    private SceneTransitionManager manager;

    // Use this for initialization
    void Start () {

        GameObject.FindWithTag("ScreenFader").GetComponent<ScreenFade>().turnUIOnAfter = true;

        m_characterList = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
          
            m_characterList[i] = transform.GetChild(i).gameObject;

        }
	
       
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ConfirmCharacter(GameObject characterSelected)
    {
        PlayerPrefs.SetString("CharacterSelected", characterSelected.name);
    }


}
