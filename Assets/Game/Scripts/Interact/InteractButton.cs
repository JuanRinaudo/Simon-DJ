using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractButton : AbstractInteractable
{

    public override void Interact(InteractionType interactionType, GameObject interactionParent = null)
    {
        if(interactionType == InteractionType.CLICK_DOWN)
        {
            Game.instance.stepPlayer.ButtonPressed(this);
        }
    }

}
