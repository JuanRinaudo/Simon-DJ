using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class InteractTurntable : AbstractInteractable
{

    private Vector3 lastParentPosition;
    private GameObject parent;
    private float deltaDistance;

    private Transform parentTransform;
    private Transform diskTransform;
    public GameObject turntableDisk;

    public float lastTurntime = 0;
    
    public float turnspeed;
    private float value;

    public float sensibility = 1000;

    public AudioSource audioSource;
    public float maxValue = 3;

    private void Awake()
    {
        diskTransform = turntableDisk.GetComponent<Transform>();
    }

    override public void Interact(InteractionType interactionType, GameObject interactionParent = null)
    {
        if (interactionType == InteractionType.CLICK_DOWN)
        {
            parent = interactionParent;
            parentTransform = parent.GetComponent<Transform>();
            lastParentPosition = parentTransform.position;
        }
        else if (interactionType == InteractionType.CLICK_UP)
        {
            parent = null;
        }
    }

    private void Update()
    {
        lastTurntime += Time.deltaTime;
        if(lastTurntime > StepPlayer.MISS_TURNTABLE_DELTA)
        {
            lastTurntime = 0;
            Game.instance.stepPlayer.TurntableMiss(this);
        }

        if (parent != null)
        {
            float circleRadius = Vector3.Distance(parentTransform.position, diskTransform.position);
            turnspeed = (Vector3.Distance(parentTransform.position, lastParentPosition) / circleRadius) * sensibility;
            lastParentPosition = parentTransform.position;

            if(turnspeed > 0.1f)
            {
                lastTurntime = 0;
            }

            audioSource.pitch = turnspeed * maxValue;
        }
        else
        {
            value += Time.deltaTime * turnspeed;
            value = (value + 100) % 1;
            turnspeed = Mathf.Lerp(turnspeed, .6666f, 0.9f);
            audioSource.pitch = turnspeed * (maxValue * 2) - maxValue;
        }

        PositionTurntable();

    }

    private void PositionTurntable()
    {
        diskTransform.localRotation = Quaternion.Euler(0, 360 * value, 0);
    }

}
