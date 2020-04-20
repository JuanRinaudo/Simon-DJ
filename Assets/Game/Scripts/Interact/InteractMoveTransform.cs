using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractMoveTransform : AbstractInteractable
{
    
    public Transform moveTarget;
    public Transform moveTo;

    public override void Interact(InteractionType interactionType, GameObject interactionParent = null)
    {
        moveTarget.position = moveTo.position;
    }

}
