using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{

    [Header("Grab")]
    [HideInInspector] public GameObject grabParent;
    public Rigidbody rigidbody;
    public bool grabeable;
    public int grabPriority = 0;

    [Header("Interaction")]
    public AbstractInteractable[] interactionScripts;
    public bool interactOnGrab = false;
    public bool interactOnRelease = false;
    public bool interactOnClickDown = false;
    public bool interactOnClickUp = false;
    public int interactionPriority = 0;

    private void Awake()
    {
        
    }

    public void Grabbed(GameObject parent)
    {
        grabParent = parent;

        if (interactOnGrab)
        {
            AbstractInteractable.RunInteractionList(interactionScripts, InteractionType.GRAB, parent);
        }
    }

    public void Released(GameObject parent)
    {
        grabParent = null;

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if(rigidbody)
        {
            rigidbody.useGravity = true;
        }

        if (interactOnRelease)
        {
            AbstractInteractable.RunInteractionList(interactionScripts, InteractionType.RELEASE, parent);
        }
    }

    public void ClickDown(GameObject parent)
    {
        if (interactOnClickDown)
        {
            AbstractInteractable.RunInteractionList(interactionScripts, InteractionType.CLICK_DOWN, parent);
        }
    }

    public void ClickUp(GameObject parent)
    {
        if (interactOnClickUp)
        {
            AbstractInteractable.RunInteractionList(interactionScripts, InteractionType.CLICK_UP, parent);
        }
    }

}
