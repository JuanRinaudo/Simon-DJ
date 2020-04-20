using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDrawerSmooth : AbstractInteractable
{

    [Header("Interaction")]
    public Transform drawerTransform;
    public Transform drawerKnobTransform;
    private GameObject interactionParent;
    private Vector3 finalLocation;
    private float fullOpenMagnitude;
    public float positionMaxDelta;
    [HideInInspector] public float deltaPosition;
    private float lastDeltaAngle = -1;

    public bool drawerLocked = false;

    [Header("SFX")]
    public Transform audioPosition;
    public AudioClip openDoor;
    public AudioClip closeDoor;
    public AudioClip lockedDoor;

    [HideInInspector] public bool opened = false;
    private bool opening = false;
    private bool closing = false;
    private Vector3 startingPosition;

    private void Awake()
    {
        startingPosition = drawerTransform.localPosition;

        Vector3 normalizedDirection = transform.up;
        normalizedDirection.y = 0;
        finalLocation = drawerKnobTransform.position + normalizedDirection * positionMaxDelta;
        fullOpenMagnitude = (finalLocation - drawerKnobTransform.position).sqrMagnitude;
    }

    override public void Interact(InteractionType interactionType, GameObject interactionParent = null)
    {
        if (enabled)
        {
            if(interactionType == InteractionType.CLICK_DOWN)
            {
                this.interactionParent = interactionParent;
                if (drawerLocked)
                {
                    float tweenTime = 0.15f;
                    LeanTween.rotateLocal(drawerTransform.gameObject, new Vector3(90, 1, 0), tweenTime).setEase(LeanTweenType.easeInCubic).setRepeat(2).setOnComplete(
                        () => { LeanTween.rotateLocal(drawerTransform.gameObject, new Vector3(90, 0, 0), tweenTime).setEase(LeanTweenType.easeOutCubic); }
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
            float targetDeltaAngle = (1 - Mathf.Clamp((finalLocation - interactionParent.transform.position).sqrMagnitude / fullOpenMagnitude, 0f, 1f)) * positionMaxDelta;
            if (lastDeltaAngle == -1)
            {
                deltaPosition = targetDeltaAngle;
            }
            else
            {
                deltaPosition = Mathf.Lerp(lastDeltaAngle, targetDeltaAngle, 0.2f);
            }
            lastDeltaAngle = deltaPosition;

            drawerTransform.localPosition = new Vector3(startingPosition.x, startingPosition.y, startingPosition.z + deltaPosition);
            opened = deltaPosition > positionMaxDelta * 0.2f;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(drawerKnobTransform != null)
        {
            Vector3 normalizedDirection = transform.up;
            normalizedDirection.y = 0;
            normalizedDirection.Normalize();
            Gizmos.color = Color.red;
            Gizmos.DrawLine(drawerKnobTransform.position, drawerKnobTransform.position + normalizedDirection * positionMaxDelta);
        }
    }

}