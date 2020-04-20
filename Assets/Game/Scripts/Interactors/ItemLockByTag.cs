using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLockByTag : MonoBehaviour
{

    [Header("Interaction")]
    public Transform lockTransform;
    public string lockTag;

    public AbstractInteractable[] onPlaceInteractions;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(lockTag) && lockTransform)
        {
            other.GetComponent<InteractableObject>().grabParent.GetComponent<InteractionBehaviour>().ReleaseHandItem();
            other.transform.position = lockTransform.position;
            other.transform.rotation = lockTransform.rotation;
            other.transform.parent = lockTransform;
            Rigidbody keyRigidbody = other.GetComponent<Rigidbody>();
            keyRigidbody.useGravity = false;
            keyRigidbody.velocity = Vector3.zero;
            keyRigidbody.angularVelocity = Vector3.zero;

            AbstractInteractable.RunInteractionList(onPlaceInteractions, InteractionType.PARENT, other.gameObject);
        }
    }
  

}
