using UnityEngine;
using System.Collections;



public interface TouchInteractive {

    void Interact();

    void Interact(Vector3 touchPosition);

    void FinishInteraction();
}


