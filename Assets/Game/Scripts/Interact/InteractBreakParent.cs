using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBreakParent : AbstractInteractable
{

    public Transform transformToBreak;
    public FixedJoint jointToBreak;
    public MeshCollider meshCollider;

    override public void Interact(InteractionType interactionType, GameObject interactionParent = null)
    {
        transformToBreak.parent = null;
        DestroyImmediate(jointToBreak);
        meshCollider.enabled = true;
    }

}
