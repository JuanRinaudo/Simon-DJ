using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDoorSmooth : AbstractInteractable
{

    [Header("Interaction")]
    public Transform doorTransform;
    public Transform doorKnobTransform;
    private GameObject interactionParent;
    private Vector3 finalLocation;
    private float fullOpenMagnitude;
    [HideInInspector] public float deltaAngle;
    private float lastDeltaAngle = -1;
    public bool openFoward = true;

    public bool doorLocked = false;

    [Header("SFX")]
    public Transform audioPosition;
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

        Vector3 normalizedDirection = transform.up * (openFoward ? -1 : 1) + transform.right;
        normalizedDirection.y = 0;
        finalLocation = doorKnobTransform.position + normalizedDirection * 0.5f;
        fullOpenMagnitude = (finalLocation - doorKnobTransform.position).sqrMagnitude;
    }

    override public void Interact(InteractionType interactionType, GameObject interactionParent = null)
    {
        if (enabled)
        {
            if(interactionType == InteractionType.CLICK_DOWN)
            {
                this.interactionParent = interactionParent;
                if (doorLocked)
                {
                    float tweenTime = 0.15f;
                    LeanTween.rotateLocal(doorTransform.gameObject, new Vector3(90, 1, 0), tweenTime).setEase(LeanTweenType.easeInCubic).setRepeat(2).setOnComplete(
                        () => { LeanTween.rotateLocal(doorTransform.gameObject, new Vector3(90, 0, 0), tweenTime).setEase(LeanTweenType.easeOutCubic); }
                    );
                    Sound.PlayOneShot(audioPosition, lockedDoor);
                }
                else
                {
                    running = true;
                    if (!opening)
                    {
                        opening = true;
                        closing = false;
                        Sound.PlayOneShot(audioPosition, openDoor);
                    }
                    else
                    {
                        opening = false;
                        closing = true;
                        Sound.PlayOneShot(audioPosition, closeDoor);
                    }
                }
            }
            else
            {
                running = false;
            }
        }
    }

    private void Update()
    {
        if (running)
        {
            float targetDeltaAngle = (1 - Mathf.Clamp((finalLocation - interactionParent.transform.position).sqrMagnitude / fullOpenMagnitude, 0f, 1f)) * 90f;
            if (lastDeltaAngle == -1)
            {
                deltaAngle = targetDeltaAngle;
            }
            else
            {
                deltaAngle = Mathf.Lerp(lastDeltaAngle, targetDeltaAngle, 0.2f);
            }
            lastDeltaAngle = deltaAngle;

            doorTransform.localRotation = Quaternion.Euler(startingRotation.x, startingRotation.y + deltaAngle * (openFoward ? -1 : 1), startingRotation.z);
            opened = deltaAngle > 45f;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(doorKnobTransform != null)
        {
            Vector3 normalizedDirection = transform.up * (openFoward ? -1 : 1) + transform.right;
            normalizedDirection.y = 0;
            normalizedDirection.Normalize();
            Gizmos.color = Color.red;
            Gizmos.DrawLine(doorKnobTransform.position, doorKnobTransform.position + normalizedDirection * 0.5f);
        }
    }

}