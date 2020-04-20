using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDelayedInteraction : AbstractInteractable
{

    [Header("Interaction")]
    public AbstractInteractable[] interactionScripts;
    [Min(0.01f)]
    public float delay = 0.1f;
    private float timerDelay;
    [HideInInspector] public GameObject parent;
    [HideInInspector] public InteractionType type;

    public bool disableOnDispatch;
    
    void Update()
    {
        if(running) {
            timerDelay = Mathf.Max(timerDelay - Time.deltaTime, 0);
            if(timerDelay <= 0)
            {
                if(disableOnDispatch)
                {
                    enabled = false;
                }
                running = false;

                RunInteractionList(interactionScripts, type, parent);
            }
        }
    }

    public override void Interact(InteractionType interactionType, GameObject interactionParent = null)
    {
        running = true;
        timerDelay = delay;
        type = interactionType;
        parent = interactionParent;
    }

}
