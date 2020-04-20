using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectTriggerInteraction : MonoBehaviour
{

    [Header("Interaction")]
    public AbstractInteractable[] interactionScripts;
    public GameObject targetGameObject;
    public bool interactOnEnter = false;
    public bool interactOnStay = false;
    public bool interactOnExit = false;

    public bool disableOnInteraction = false;

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    private void TryInteraction(GameObject other, InteractionType interactionType)
    {
        if(other == targetGameObject)
        {
            if(disableOnInteraction)
            {
                gameObject.SetActive(false);
            }

            AbstractInteractable.RunInteractionList(interactionScripts, interactionType, other);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (interactOnEnter)
        {
            TryInteraction(other.gameObject, InteractionType.ENTER);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (interactOnExit)
        {
            TryInteraction(other.gameObject, InteractionType.EXIT);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (interactOnStay)
        {
            TryInteraction(other.gameObject, InteractionType.STAY);
        }
    }

}
