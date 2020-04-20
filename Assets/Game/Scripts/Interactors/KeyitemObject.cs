using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyitemObject : MonoBehaviour
{

    [Header("Interaction")]
    public Transform lockTransform;
    public AbstractInteractable[] interactionScripts;
    public bool interactOnEnter = false;
    public bool interactOnStay = false;
    public bool interactOnExit = false;

    public bool canInteractIfRunning = false;

    public GameObject keyObject;

    private void TryInteraction(GameObject gameObject, InteractionType interactionType)
    {
        bool interactionsRunning = false;
        for (int scriptIndex = 0; scriptIndex < interactionScripts.Length; ++scriptIndex)
        {
            if(interactionScripts[scriptIndex].running)
            {
                interactionsRunning = true;
                break;
            }
        }

        if (gameObject == keyObject && (canInteractIfRunning || !interactionsRunning))
        {
            AbstractInteractable.RunInteractionList(interactionScripts, interactionType);

            if (lockTransform)
            {
                keyObject.transform.SetParent(lockTransform);
                keyObject.transform.localPosition = Vector3.zero;
                keyObject.transform.localRotation = lockTransform.localRotation;
                InteractableObject interactableObject = keyObject.GetComponent<InteractableObject>();
                interactableObject.grabeable = false;
                interactableObject.grabParent.GetComponent<InteractionBehaviour>().ReleaseHandItem();
                Rigidbody keyRigidbody = keyObject.GetComponent<Rigidbody>();
                keyRigidbody.useGravity = false;
                keyRigidbody.velocity = Vector3.zero;
                keyRigidbody.angularVelocity = Vector3.zero;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(interactOnEnter)
        {
            TryInteraction(other.gameObject, InteractionType.ENTER);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(interactOnExit)
        {
            TryInteraction(other.gameObject, InteractionType.EXIT);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(interactOnStay)
        {
            TryInteraction(other.gameObject, InteractionType.STAY);
        }
    }

}
