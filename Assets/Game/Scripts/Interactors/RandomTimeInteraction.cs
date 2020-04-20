using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTimeInteraction : MonoBehaviour
{

    [Header("Interaction")]
    public AbstractInteractable[] interactionScripts;
    public int interactCounter = -1; // NOTE(Juan): Loop if -1
    public float timerSecBase;
    public float timerSecRandomDelta;

    [HideInInspector] public float interactTime = 0;

    void Start()
    {
        CalculateRandomInteractTime();
    }

    void Update()
    {
        interactTime = Mathf.Max(interactTime - Time.deltaTime, 0);
        if (interactTime <= 0)
        {
            interactCounter--;
            enabled = interactCounter > 0 || interactCounter == -1;
            if (enabled)
            {
                CalculateRandomInteractTime();
            }

            for(int scriptIndex = 0; scriptIndex < interactionScripts.Length; ++scriptIndex)
            {
                interactionScripts[scriptIndex].Interact(InteractionType.PARENT, gameObject);
            }
        }
    }

    private void CalculateRandomInteractTime() {
        interactTime = timerSecBase + Random.Range(0f, timerSecRandomDelta);
    }

}
