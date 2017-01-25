using UnityEngine;
using System.Collections;
using System;

// Why should tappable objects require a Rigidbody
[RequireComponent(typeof(Animator))]
public class Tappable : MonoBehaviour , TouchInteractive {

	public Color newColour = new Color(0,1,0);
    private Color oldColour;

    public Animator animator;
    public string triggerName;

	// Use this for initialization
	void Start () {

        oldColour = GetComponent<MeshRenderer>().material.color;
        animator = GetComponent<Animator>();
	}
	


    public void Interact()
    {
		print("Tap that");
		GetComponent<MeshRenderer>().material.color = newColour;

        animator.SetTrigger(triggerName);
    }

	public void Interact(Vector3 fingerPosition)
    {
        // Not showing in the EventHandler inspector?
	}

    public void FinishInteraction()
    {
        print("Finish tap");
        GetComponent<MeshRenderer>().material.color = oldColour;
    }
}
