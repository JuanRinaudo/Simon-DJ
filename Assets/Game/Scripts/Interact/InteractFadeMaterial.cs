using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractFadeMaterial : AbstractInteractable
{

    [Header("Interaction")]
    public AbstractInteractable[] interactionsOnComplete;

    public Material material;
    public Color fadeToColor = Color.white;
    public float time = 1;

    public override void Interact(InteractionType interactionType, GameObject interactionParent = null)
    {
        Color startColor = material.color * 1.0f;
        LeanTween.value(0, 1, time).setOnUpdate((float t) =>
        {
            material.color = Color.Lerp(startColor, fadeToColor, t);
        }).setOnComplete(() =>
        {
            RunInteractionList(interactionsOnComplete, InteractionType.PARENT, gameObject);
        });
    }

}
