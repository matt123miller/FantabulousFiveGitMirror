using UnityEngine;
using System.Collections;



public interface ITouchInteractive {

    void Interact();

    void Interact(Vector3 touchPosition);

    void FinishInteraction();
}


