using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractCounterInteraction : AbstractInteractable
{

    [Header("Interaction")]
    public AbstractInteractable[] interactionScripts;
    public InteractionType type;
    [Min(2)]
    public int interactionCount = 2;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void Interact(InteractionType interactionType, GameObject interactionParent = null)
    {
        interactionCount--;
        if(interactionCount <= 0)
        {
            RunInteractionList(interactionScripts, interactionType, interactionParent);
        }
    }

}
