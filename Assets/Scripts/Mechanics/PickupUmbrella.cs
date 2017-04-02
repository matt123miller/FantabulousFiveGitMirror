using UnityEngine;
using System.Collections;

public class PickupUmbrella : MonoBehaviour {

    GameObject floatButton;

	// Use this for initialization
	void Start () {
        floatButton = GameObject.Find("FloatButton");
        floatButton.SetActive(false);
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
                if (bone.name == "EthanRightHand")
                {
                    hand = GameObject.Find("EthanRightHand");
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
