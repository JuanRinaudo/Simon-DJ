using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class InteractLetter : AbstractInteractable
{

    public GameObject textGameObject;

    public RectTransform textTransform;
    private Vector3 initialTextPosition;
    private float minPosition;
    private float maxPosition;

    public float sensibility = 0.01f;
    public AudioSource audioSource;
    public SteamVR_Action_Vector2 trackpadAction = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("InteractionTrackpadTouch");

    void Start()
    {
        minPosition = 0;
        if(textTransform != null)
        {
            initialTextPosition = textTransform.anchoredPosition3D;
            maxPosition = textTransform.sizeDelta.y - 0.01f;
        }
        else
        {
            initialTextPosition = Vector3.zero;
            maxPosition = 0;
        }
    }

    void Update()
    {
        if(running && textTransform != null)
        {
            textTransform.anchoredPosition = new Vector3(initialTextPosition.x, Mathf.Clamp(textTransform.anchoredPosition.y + trackpadAction.axis.y * sensibility, minPosition, maxPosition), initialTextPosition.z);
        }
    }

    override public void Interact(InteractionType interactionType, GameObject interactionParent = null)
    {
        if(interactionType == InteractionType.RELEASE)
        {
            if(running)
            {
                running = false;
            }
        }
        else
        {
            running = !running;
        }

        if(running)
        {
            textTransform.anchoredPosition = initialTextPosition;
        }

        if(textGameObject != null)
        {
            textGameObject.gameObject.SetActive(running);
        }

        if (running)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }

}
