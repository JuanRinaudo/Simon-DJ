using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSecuence : AbstractInteractable
{

    public AbstractInteractable[] secuence;

    public override void Interact(InteractionType interactionType, GameObject interactionParent = null)
    {
        for(int interactIndex = 0; interactIndex < secuence.Length; ++interactIndex)
        {
            AbstractInteractable interaction = secuence[interactIndex];
            interaction.Interact(interactionType, interactionParent);
        }
    }

}
