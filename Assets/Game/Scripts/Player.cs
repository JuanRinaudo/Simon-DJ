using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;
using TMPro;

public class Player : MonoBehaviour
{

    public Transform centerPosition;

    //public GameObject leftHand;
    //public GameObject rightHand;
    public Camera camera;

    public float rotationSensibility = 10;
    public float movementSpeed = 10;

    //public LayerMask boundsMask;

    //private GameObject clickedObject;

    public GameObject desktopCursor;

    //private InteractableObject castedInteractObject;
    //public Material interactHighlightMaterial;

    public TextMeshPro desktopInstructions;
    public TextMeshPro vrInstructions;

    //private float recenterTimer = 0;

    private Vector3 desktopOffset = new Vector3(0, 1.5f, -0.2f);

    private void Awake()
    {
        transform.position = centerPosition.position;

        if (Game.vrDisabled || !XRSettings.enabled)
        {
            desktopInstructions.gameObject.SetActive(true);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            camera.transform.localPosition += desktopOffset;
            desktopCursor.SetActive(true);
        }
        else
        {
            vrInstructions.gameObject.SetActive(true);

            CenterPlayer();
        }
    }

    //private void Update()
    //{
    //    if (Game.vrDisabled)
    //    {
    //        //float horizontal = Input.GetAxis("Horizontal");
    //        //float vertical = Input.GetAxis("Vertical");

    //        //Vector3 positionDelta = camera.transform.TransformVector(new Vector3(horizontal, 0, vertical)) * Time.deltaTime * movementSpeed;
    //        //Vector3 newPosition = camera.transform.position + positionDelta;
    //        //if (!Physics.Raycast(new Ray(camera.transform.position, newPosition), positionDelta.magnitude * 2, boundsMask))
    //        //{
    //        //    camera.transform.position = newPosition;
    //        //}
    //        if (Input.GetKeyDown(KeyCode.Space))
    //        {
    //            CenterPlayer();
    //        }

    //        float y = Input.GetAxis("Mouse X");
    //        float x = Input.GetAxis("Mouse Y");

    //        Vector3 rotateValue = new Vector3(x, y * -1, 0) * Time.deltaTime * rotationSensibility;
    //        camera.transform.eulerAngles = camera.transform.eulerAngles - rotateValue;

    //        RaycastHit rayHit;
    //        if (Physics.Raycast(camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out rayHit))
    //        {
    //            desktopCursor.transform.position = rayHit.point;
    //        }

    //        if (castedInteractObject != null)
    //        {
    //            MeshRenderer[] renderers = castedInteractObject.GetComponentsInChildren<MeshRenderer>();
    //            for (int rendererIndex = 0; rendererIndex < renderers.Length; ++rendererIndex)
    //            {
    //                MeshRenderer renderer = renderers[rendererIndex];
    //                Material[] materials = renderer.sharedMaterials;
    //                System.Array.Resize(ref materials, 1);
    //                renderer.sharedMaterials = materials;
    //            }
    //        }

    //        castedInteractObject = rayHit.collider.GetComponent<InteractableObject>();
    //        if (castedInteractObject)
    //        {
    //            MeshRenderer[] renderers = castedInteractObject.GetComponentsInChildren<MeshRenderer>();
    //            for (int rendererIndex = 0; rendererIndex < renderers.Length; ++rendererIndex)
    //            {
    //                MeshRenderer renderer = renderers[rendererIndex];
    //                Material[] materials = renderer.sharedMaterials;
    //                System.Array.Resize(ref materials, renderer.sharedMaterials.Length + 1);
    //                materials[renderer.sharedMaterials.Length] = interactHighlightMaterial;
    //                renderer.sharedMaterials = materials;
    //            }
    //        }

    //        bool clickDown = Input.GetButtonDown("Fire1");
    //        bool clickUp = Input.GetButtonUp("Fire1");
    //        if (clickDown)
    //        {
    //            TryObjectClickDownInteractions(rayHit.collider.gameObject);
    //        }
    //        if (clickUp)
    //        {
    //            TryObjectClickUpInteractions(clickedObject);
    //        }
    //    }
    //    else
    //    {
    //        if(teleport.state)
    //        {
    //            recenterTimer += Time.deltaTime;
    //            if(recenterTimer > 1.0f)
    //            {
    //                CenterPlayer();
    //                recenterTimer = Mathf.NegativeInfinity;
    //            }
    //        }
    //        else
    //        {
    //            recenterTimer = 0;
    //        }
    //    }
    //}

    //private void TryObjectGrabInteractions(GameObject targetObject)
    //{
    //    InteractableObject[] interacteableObjects = targetObject.GetComponents<InteractableObject>();
    //    for (int interactIndex = 0; interactIndex < interacteableObjects.Length; ++interactIndex)
    //    {
    //        interacteableObjects[interactIndex].Grabbed(desktopCursor);
    //    }
    //}

    //private void TryObjectReleaseInteractions(GameObject targetObject)
    //{
    //    InteractableObject[] interacteableObjects = targetObject.GetComponents<InteractableObject>();
    //    for (int interactIndex = 0; interactIndex < interacteableObjects.Length; ++interactIndex)
    //    {
    //        interacteableObjects[interactIndex].Released(desktopCursor);
    //    }
    //}

    //private void TryObjectClickDownInteractions(GameObject targetObject)
    //{
    //    clickedObject = targetObject;

    //    InteractableObject[] interacteableObjects = targetObject.GetComponents<InteractableObject>();
    //    for (int interactIndex = 0; interactIndex < interacteableObjects.Length; ++interactIndex)
    //    {
    //        interacteableObjects[interactIndex].ClickDown(desktopCursor);
    //    }
    //}

    //private void TryObjectClickUpInteractions(GameObject targetObject)
    //{
    //    InteractableObject[] interacteableObjects = targetObject.GetComponents<InteractableObject>();
    //    for (int interactIndex = 0; interactIndex < interacteableObjects.Length; ++interactIndex)
    //    {
    //        interacteableObjects[interactIndex].ClickUp(desktopCursor);
    //    }
    //}

    private void CenterPlayer()
    {
        if (Game.vrDisabled)
        {
            transform.position = centerPosition.position;
            camera.transform.localPosition = desktopOffset;
        }
        else
        {
            Vector3 worldZero = centerPosition.position;
            Vector3 cameraOffsetXY = new Vector3(worldZero.x - camera.transform.localPosition.x, worldZero.y, worldZero.z - camera.transform.localPosition.z);
            transform.position = cameraOffsetXY;
        }
    }

}
