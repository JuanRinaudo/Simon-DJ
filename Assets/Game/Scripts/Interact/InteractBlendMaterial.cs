using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBlendMaterial : AbstractInteractable
{

    public Material material;
    public float timeDelay = 0;
    public float timeMultiplier = 1;

    private float blendDelay = 0;
    private int blendWeightID;
    private float blendWeight;

    private void Awake()
    {
        blendDelay = 0;
        blendWeightID = Shader.PropertyToID("_BlendWeight");
        material.SetFloat(blendWeightID, 0);
    }

    override public void Interact(InteractionType interactionType, GameObject interactionParent = null)
    {
        running = true;
    }

    private void Update()
    {
        if (running) {
            if (blendDelay < timeDelay)
            {
                blendDelay += Time.deltaTime;
            }
            else
            {
                blendWeight = Mathf.Clamp(blendWeight + Time.deltaTime * timeMultiplier, 0, 1);
                material.SetFloat(blendWeightID, blendWeight);
            }

            if(blendWeight == 1)
            {
                running = false;
            }
        }
    }

}
