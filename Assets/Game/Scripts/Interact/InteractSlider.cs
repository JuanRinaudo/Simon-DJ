using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class InteractSlider : AbstractInteractable
{

    private Vector3 lastParentPosition;
    private GameObject parent;
    private float deltaDistance;

    public bool verticalHorizontalSlider = true;

    public GameObject slider;
    public Transform minTransform;
    public Transform maxTransform;
    [Range(0, 1)]
    public float value;

    private Transform parentTransform;
    private Transform sliderTransform;
    public AudioSource audioSource;

    public AudioMixer mixer;
    public float minValue = 0;
    public float maxValue = 1;
    public string parameterName;

    private void Awake()
    {
        sliderTransform = slider.GetComponent<Transform>();

        PositionSlider();

        if(verticalHorizontalSlider)
        {
            deltaDistance = maxTransform.position.z - minTransform.position.z;
        }
        else
        {
            deltaDistance = maxTransform.position.x - minTransform.position.x;
        }
    }

    override public void Interact(InteractionType interactionType, GameObject interactionParent = null)
    {
        if(interactionType == InteractionType.CLICK_DOWN)
        {
            parent = interactionParent;
            parentTransform = parent.GetComponent<Transform>();
            lastParentPosition = parentTransform.position;
        }
        else if(interactionType == InteractionType.CLICK_UP)
        {
            parent = null;
        }
    }

    private void Update()
    {
        value = Mathf.Clamp(value, 0, 1);
        if(parameterName.Length > 0)
        {
            mixer.SetFloat(parameterName, value * (maxValue - minValue) + minValue);
        }

        PositionSlider();

        if (parent != null)
        {
            Vector3 delta = parentTransform.position - lastParentPosition;
            if (verticalHorizontalSlider)
            {
                value += delta.z * (1 / deltaDistance);
            }
            else
            {
                value += delta.x * (1 / deltaDistance);
            }

            lastParentPosition = parentTransform.position;
        }
    }

    private void PositionSlider()
    {
        sliderTransform.position = Vector3.Lerp(minTransform.position, maxTransform.position, value);
    }

    public Vector3 GetValueOffset(float value)
    {
        return transform.InverseTransformPoint(Vector3.Lerp(minTransform.position, maxTransform.position, value));
    }

}
