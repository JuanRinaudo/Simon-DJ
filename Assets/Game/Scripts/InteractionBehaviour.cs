using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class InteractionBehaviour : MonoBehaviour
{

    private List<InteractableObject> interacteableObjects = new List<InteractableObject>();
    private GameObject clickedObject;

    public FixedJoint parentJoint;
    private Rigidbody jointRigidbody;
    public SteamVR_Behaviour_Pose handPose;

    private InteractableObject closestGrabObject;
    private InteractableObject closestInteractObject;

    public Material grabHighlightMaterial;
    public Material interactHighlightMaterial;

    private float lastTriggerTime = 0;

    public SteamVR_Action_Boolean grabGrip = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");
    public SteamVR_Action_Boolean interactButton = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("InteractUI");

    private SteamVR_Input_Sources inputSource;
    public Hand handAsigned;

    private Vector3 lastPosition;

    void Awake()
    {
        if(handAsigned == Hand.LEFT)
        {
            inputSource = SteamVR_Input_Sources.LeftHand;
        }
        else
        {
            inputSource = SteamVR_Input_Sources.RightHand;
        }
        jointRigidbody = parentJoint.GetComponent<Rigidbody>();

        Game.instance.handInteractions[(int)handAsigned] = this;
    }

    public void AddObject(InteractableObject interactable)
    {
        if(interacteableObjects.IndexOf(interactable) == -1)
        {
            interacteableObjects.Add(interactable);
        }
    }

    public void RemoveObject(InteractableObject interactable)
    {
        interacteableObjects.Remove(interactable);
    }

    private void CheckHandItem()
    {
        //if(Game.handItems[(int)handAsigned] != null)
        //{
        //    if (interacteableObjects.Count == 0 && parentJoint.connectedBody == null && lastTriggerTime > 0.05f)
        //    {
        //        Game.handItems[(int)handAsigned].ChangeItem(HandItems.Cellphone);
        //    }
        //    else
        //    {
        //        Game.handItems[(int)handAsigned].ChangeItem(HandItems.Hand);
        //    }
        //}
    }
    
    private InteractableObject GetClosestGrabObject()
    {
        float distance = Mathf.Infinity;
        int priority = int.MinValue;
        InteractableObject closestObject = null;
        for (int objectIndex = 0; objectIndex < interacteableObjects.Count; ++objectIndex)
        {
            InteractableObject testObject = interacteableObjects[objectIndex];
            InteractableObject[] interactableScripts = testObject.GetComponents<InteractableObject>();
            for (int interactIndex = 0; interactIndex < interactableScripts.Length; ++interactIndex)
            {
                InteractableObject testScript = interactableScripts[interactIndex];
                if (testScript.grabeable)
                {
                    float testDistance = Vector3.Distance(transform.position, testObject.transform.position);
                    if (testDistance < distance || testScript.grabPriority > priority)
                    {
                        distance = testDistance;
                        priority = testScript.grabPriority;
                        closestObject = testObject;
                        break;
                    }
                }
            }
        }
        return closestObject;
    }

    private InteractableObject GetClosestClickeableObject()
    {
        float distance = Mathf.Infinity;
        int priority = int.MinValue;
        InteractableObject closestObject = null;
        for (int objectIndex = 0; objectIndex < interacteableObjects.Count; ++objectIndex)
        {
            InteractableObject testObject = interacteableObjects[objectIndex];
            InteractableObject[] interactableScripts = testObject.GetComponents<InteractableObject>();
            for (int interactIndex = 0; interactIndex < interactableScripts.Length; ++interactIndex)
            {
                InteractableObject testScript = interactableScripts[interactIndex];
                if (testScript.interactOnClickDown)
                {
                    float testDistance = Vector3.Distance(transform.position, testObject.transform.position);
                    if (testDistance < distance || testScript.interactionPriority > priority)
                    {
                        distance = testDistance;
                        priority = testScript.interactionPriority;
                        closestObject = testObject;
                        break;
                    }
                }
            }
        }
        return closestObject;
    }

    private void TryObjectGrabInteractions(GameObject targetObject)
    {
        InteractableObject[] interacteableObjects = targetObject.GetComponents<InteractableObject>();
        for(int interactIndex = 0; interactIndex < interacteableObjects.Length; ++interactIndex)
        {
            interacteableObjects[interactIndex].Grabbed(gameObject);
        }
    }

    private void TryObjectReleaseInteractions(GameObject targetObject)
    {
        InteractableObject[] interacteableObjects = targetObject.GetComponents<InteractableObject>();
        for (int interactIndex = 0; interactIndex < interacteableObjects.Length; ++interactIndex)
        {
            interacteableObjects[interactIndex].Released(gameObject);
        }
    }

    private void TryObjectClickDownInteractions(GameObject targetObject)
    {
        clickedObject = targetObject;

        InteractableObject[] interacteableObjects = targetObject.GetComponents<InteractableObject>();
        for (int interactIndex = 0; interactIndex < interacteableObjects.Length; ++interactIndex)
        {
            interacteableObjects[interactIndex].ClickDown(gameObject);
        }
    }

    private void TryObjectClickUpInteractions(GameObject targetObject)
    {
        InteractableObject[] interacteableObjects = targetObject.GetComponents<InteractableObject>();
        for (int interactIndex = 0; interactIndex < interacteableObjects.Length; ++interactIndex)
        {
            interacteableObjects[interactIndex].ClickUp(gameObject);
        }
    }

    void Update()
    {
        if (interacteableObjects.Count == 0)
        {
            lastTriggerTime += Time.deltaTime;
        }
        CheckHandItem();

        // TODO(Juan): Refactor this to be better performant, probably with a custom outlineable shader
        if (closestGrabObject != null)
        {
            MeshRenderer[] renderers = closestGrabObject.GetComponentsInChildren<MeshRenderer>();
            for(int rendererIndex = 0; rendererIndex < renderers.Length; ++rendererIndex)
            {
                MeshRenderer renderer = renderers[rendererIndex];
                Material[] materials = renderer.sharedMaterials;
                System.Array.Resize(ref materials, 1);
                renderer.sharedMaterials = materials;
            }
        }
        if (closestInteractObject != null)
        {
            MeshRenderer[] renderers = closestInteractObject.GetComponentsInChildren<MeshRenderer>();
            for (int rendererIndex = 0; rendererIndex < renderers.Length; ++rendererIndex)
            {
                MeshRenderer renderer = renderers[rendererIndex];
                Material[] materials = renderer.sharedMaterials;
                System.Array.Resize(ref materials, 1);
                renderer.sharedMaterials = materials;
            }
        }

        closestGrabObject = GetClosestGrabObject();
        closestInteractObject = GetClosestClickeableObject();

        // TODO(Juan): Refactor this to be better performant, probably with a custom outlineable shader
        if (closestGrabObject != null && parentJoint.connectedBody == null)
        {
            MeshRenderer[] renderers = closestGrabObject.GetComponentsInChildren<MeshRenderer>();
            for (int rendererIndex = 0; rendererIndex < renderers.Length; ++rendererIndex)
            {
                MeshRenderer renderer = renderers[rendererIndex];
                Material[] materials = renderer.sharedMaterials;
                System.Array.Resize(ref materials, renderer.sharedMaterials.Length + 1);
                materials[renderer.sharedMaterials.Length] = grabHighlightMaterial;
                renderer.sharedMaterials = materials;
            }
        }
        if (closestInteractObject != null)
        {
            MeshRenderer[] renderers = closestInteractObject.GetComponentsInChildren<MeshRenderer>();
            for (int rendererIndex = 0; rendererIndex < renderers.Length; ++rendererIndex)
            {
                MeshRenderer renderer = renderers[rendererIndex];
                Material[] materials = renderer.sharedMaterials;
                System.Array.Resize(ref materials, renderer.sharedMaterials.Length + 1);
                materials[renderer.sharedMaterials.Length] = interactHighlightMaterial;
                renderer.sharedMaterials = materials;
            }
        }

        if (grabGrip.GetLastStateDown(inputSource))
        {
            if(interacteableObjects.Count > 0)
            {
                if(closestGrabObject != null) {
                    Rigidbody targetRigidbody = closestGrabObject.rigidbody;
                    Game.data.currentGrabItem[(int)handAsigned] = closestGrabObject;
                    parentJoint.connectedBody = targetRigidbody;

                    TryObjectGrabInteractions(closestGrabObject.gameObject);
                }
            }
        }
        else if(grabGrip.GetLastStateUp(inputSource) && Game.data.currentGrabItem[(int)handAsigned] != null)
        {
            ReleaseHandItem();
        }

        if(interactButton.GetLastStateDown(inputSource))
        {
            if (closestInteractObject != null)
            {
                TryObjectClickDownInteractions(closestInteractObject.gameObject);
            }
        }
        else if(interactButton.GetLastStateUp(inputSource))
        {
            if(clickedObject != null)
            {
                TryObjectClickUpInteractions(clickedObject);
                clickedObject = null;
            }
        }

        lastPosition = transform.position;
    }

    public void ReleaseHandItem(bool doReleaseAction = true)
    {
        if(parentJoint.connectedBody != null) {
            parentJoint.connectedBody = null;

            Vector3 velocity = transform.position - lastPosition;
            Rigidbody currentRigidbody = Game.data.currentGrabItem[(int)handAsigned].rigidbody;
            currentRigidbody.velocity = handPose.GetVelocity();
            currentRigidbody.angularVelocity = handPose.GetAngularVelocity();

            if(doReleaseAction)
            {
                TryObjectReleaseInteractions(Game.data.currentGrabItem[(int)handAsigned].gameObject);
            }

            Game.data.currentGrabItem[(int)handAsigned] = null;
        }
    }

    public void ClearInteractableObjects()
    {
        interacteableObjects.Clear();
        lastTriggerTime = 1;
        CheckHandItem();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<InteractableObject>() != null)
        {
            lastTriggerTime = 0;
        }

        InteractableObject interaction = other.GetComponent<InteractableObject>();
        if (other.CompareTag(UnityTags.INTERACTABLE) && interaction != null)
        {
            AddObject(interaction);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractableObject interaction = other.GetComponent<InteractableObject>();
        if (other.CompareTag(UnityTags.INTERACTABLE) && interaction != null)
        {
            RemoveObject(interaction);
        }
    }

}
