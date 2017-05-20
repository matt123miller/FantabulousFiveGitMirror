using UnityEngine;
using System.Collections;

public class PickupUmbrella : MonoBehaviour {

    public GameObject floatButton;

	// Use this for initialization
	void Start () {
        //floatButton = GameObject.Find("FloatButton");

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider _other)
    {
        if (_other.gameObject.tag == "Player")
        {
            GameObject hand = null;
            Transform[] characterBones = _other.GetComponentsInChildren<Transform>();
            foreach (Transform bone in characterBones)
            {
                if (bone.name.Contains("RightHand"))
                {
                    hand = bone.gameObject;
                    break;
                }
            }

            if (hand != null)
            {
                transform.parent = hand.transform;
                transform.localPosition = new Vector3(0, 0, 0);
                floatButton.SetActive(true);
            }

        }
    }
}
