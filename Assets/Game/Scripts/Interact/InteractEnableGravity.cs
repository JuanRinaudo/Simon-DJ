using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractEnableGravity : AbstractInteractable
{

    public GameObject targetObject;

    public override void Interact(InteractionType interactionType, GameObject interactionParent = null)
    {
        Rigidbody rigidbody = targetObject.GetComponent<Rigidbody>();
        rigidbody.useGravity = true;
    }

}
