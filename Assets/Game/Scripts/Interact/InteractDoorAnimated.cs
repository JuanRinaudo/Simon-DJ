using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDoorAnimated : AbstractInteractable
{

    [Header("Interaction")]
    public Transform doorTransform;
    public float speed = 1;
    [HideInInspector]
    public float deltaAngle;
    public float targetAngle = 20;
    public bool openFoward = true;
    public InteractDoor linkedDoor;

    [Header("SFX")]
    public AudioSource source;
    public AudioClip openDoor;
    public AudioClip closeDoor;

    [HideInInspector] public bool opened = false;
    private bool opening = false;
    private bool closing = false;
    private Vector3 startingRotation;

    private void Awake()
    {
        startingRotation = doorTransform.localRotation.eulerAngles;
    }

    override public void Interact(InteractionType interactionType, GameObject interactionParent = null)
    {
        if (enabled && !linkedDoor.opened)
        {
            running = true;
            linkedDoor.enabled = false;
            if (!opening)
            {
                opened = true;
                opening = true;
                closing = false;
                if (source != null)
                {
                    source.PlayOneShot(openDoor);
                }
            }
            else
            {
                opening = false;
                closing = true;
                if (source != null)
                {
                    source.PlayOneShot(closeDoor);
                }
            }
        }
    }

    private void Update()
    {
        if (running)
        {
            if (opening)
            {
                if (deltaAngle < targetAngle)
                {
                    deltaAngle = Mathf.Clamp(deltaAngle + Time.deltaTime * speed, 0f, targetAngle);
                }
                else
                {
                    running = false;
                }
            }
            if (closing)
            {
                if (deltaAngle > 0f)
                {
                    deltaAngle = Mathf.Clamp(deltaAngle - Time.deltaTime * speed, 0f, targetAngle);
                }
                else
                {
                    running = false;
                    opened = false;
                    linkedDoor.enabled = true;
                }
            }

            doorTransform.localRotation = Quaternion.Euler(startingRotation.x, startingRotation.y + deltaAngle * (openFoward ? -1 : 1), startingRotation.z);
        }
    }

}
