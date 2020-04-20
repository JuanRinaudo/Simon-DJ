using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSetParent : AbstractInteractable
{

    public Transform transformToSet;
    public Transform parentToSet;

    override public void Interact(InteractionType interactionType, GameObject interactionParent = null)
    {
        transformToSet.SetParent(parentToSet);
    }

}
