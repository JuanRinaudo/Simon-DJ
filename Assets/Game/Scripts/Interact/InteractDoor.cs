using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDoor : AbstractInteractable
{

    [Header("Interaction")]
    public Transform doorTransform;
    public float openSpeed = 50;
    [HideInInspector]
    public float deltaAngle;
    public bool openFoward = true;

    public bool doorLocked = false;

    [Header("SFX")]
    public AudioSource source;
    public AudioClip openDoor;
    public AudioClip closeDoor;
    public AudioClip lockedDoor;

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
        if (enabled)
        {
            if(doorLocked)
            {
                float tweenTime = 0.15f;
                LeanTween.rotateLocal(doorTransform.gameObject, new Vector3(90, 1, 0), tweenTime).setEase(LeanTweenType.easeInCubic).setRepeat(2).setOnComplete(
                    () => { LeanTween.rotateLocal(doorTransform.gameObject, new Vector3(90, 0, 0), tweenTime).setEase(LeanTweenType.easeOutCubic); }
                );
                if (source != null)
                {
                    source.PlayOneShot(lockedDoor);
                }
            }
            else
            {
                running = true;
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
    }

    private void Update()
    {
        if (running)
        {
            if (opening)
            {
                if (deltaAngle < 90f)
                {
                    deltaAngle = Mathf.Clamp(deltaAngle + Time.deltaTime * openSpeed, 0f, 90f);
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
                    deltaAngle = Mathf.Clamp(deltaAngle - Time.deltaTime * openSpeed, 0f, 90f);
                }
                else
                {
                    running = false;
                    opened = false;
                }
            }

            doorTransform.localRotation = Quaternion.Euler(startingRotation.x, startingRotation.y + deltaAngle * (openFoward ? -1 : 1), startingRotation.z);
        }
    }

}