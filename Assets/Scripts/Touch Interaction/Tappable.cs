using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

// Why should tappable objects require a Rigidbody
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EventTrigger))]
public class Tappable : MonoBehaviour , ITouchInteractive {

	public Color newColour = new Color(0,1,0);
    private Color _oldColour;

    public Animator animator;
    public string triggerName;
    public bool withinRange = false;

	// Use this for initialization
	void Start () {

        _oldColour = GetComponent<MeshRenderer>().material.color;
        animator = GetComponent<Animator>();
        if(GetComponent<SphereCollider>())
            GetComponent<SphereCollider>().radius = 1;
	}

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        withinRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        withinRange = false;
    }

    public void Interact()
    {
        if (!withinRange)
            return;

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
        if (!withinRange)
            return;

        print("Finish tap");
        GetComponent<MeshRenderer>().material.color = _oldColour;
    }
}
