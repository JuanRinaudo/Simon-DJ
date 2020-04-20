using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAddForce : AbstractInteractable
{

    public Rigidbody targetRigidbody;
    public Vector3 relativeAddPosition;
    public Vector3 forceToAdd;

    public override void Interact(InteractionType interactionType, GameObject interactionParent = null)
    {
        targetRigidbody.AddForceAtPosition(forceToAdd, relativeAddPosition);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 position = targetRigidbody.transform.position + relativeAddPosition;
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(position, 0.05f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(position, position + forceToAdd.normalized);
    }

}
