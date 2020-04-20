using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDrawer : AbstractInteractable
{

    [Header("Interaction")]
    public Transform drawerTransform;
    public float openSpeed = 1;
    private GameObject interactionParent;
    private Vector3 finalLocation;
    private float fullOpenMagnitude;
    public float positionMaxDelta;
    [HideInInspector] public float deltaPosition;
    private float lastDeltaAngle = -1;

    public bool drawerLocked = false;

    [Header("SFX")]
    public Transform audioPosition;
    public AudioClip openDrawer;
    public AudioClip closeDrawer;
    public AudioClip lockedDrawer;

    [HideInInspector] public bool opened = false;
    private bool opening = false;
    private bool closing = false;
    private Vector3 startingPosition;

    private void Awake()
    {
        startingPosition = drawerTransform.localPosition;

        Vector3 normalizedDirection = transform.up;
        normalizedDirection.y = 0;
        finalLocation = normalizedDirection * positionMaxDelta;
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
                    Vector3 startPosition = drawerTransform.localPosition;
                    LeanTween.moveLocal(drawerTransform.gameObject, drawerTransform.localPosition + new Vector3(0, 0, 0.01f), tweenTime).setEase(LeanTweenType.easeInCubic).setRepeat(2).setOnComplete(
                        () => { LeanTween.moveLocal(drawerTransform.gameObject, startPosition, tweenTime).setEase(LeanTweenType.easeOutCubic); }
                    );

                    if(audioPosition != null)
                    {
                        Sound.PlayOneShot(audioPosition, lockedDrawer);
                    }
                }
                else
                {
                    running = true;
                    if (!opening)
                    {
                        opening = true;
                        closing = false;

                        if (audioPosition != null)
                        {
                            Sound.PlayOneShot(audioPosition, openDrawer);
                        }
                    }
                    else
                    {
                        opening = false;
                        closing = true;

                        if (audioPosition != null)
                        {
                            Sound.PlayOneShot(audioPosition, closeDrawer);
                        }
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
            if (opening)
            {
                if (deltaPosition < positionMaxDelta)
                {
                    deltaPosition = Mathf.Clamp(deltaPosition + Time.deltaTime * openSpeed, 0f, positionMaxDelta);
                }
                else
                {
                    running = false;
                }
            }
            if (closing)
            {
                if (deltaPosition > 0f)
                {
                    deltaPosition = Mathf.Clamp(deltaPosition - Time.deltaTime * openSpeed, 0f, positionMaxDelta);
                }
                else
                {
                    running = false;
                    opened = false;
                }
            }

            drawerTransform.localPosition = new Vector3(startingPosition.x, startingPosition.y, startingPosition.z + deltaPosition);
        }
    }

}