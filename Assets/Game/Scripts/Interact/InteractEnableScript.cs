using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractEnableScript : AbstractInteractable
{

    public MonoBehaviour behaviourToEnable;
    public bool reversible = false;

    private bool initialState;

    void Start()
    {
        initialState = behaviourToEnable.enabled;
    }

    void Update()
    {

    }

    override public void Interact(InteractionType interactionType, GameObject interactionParent = null)
    {
        if (reversible || behaviourToEnable.enabled == initialState)
        {
            behaviourToEnable.enabled = !behaviourToEnable.enabled;
        }
    }

}