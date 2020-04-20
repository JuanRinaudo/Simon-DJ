using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractEnableObject : AbstractInteractable
{

    public GameObject gameObjectToEnable;
    public bool reversible = false;

    private bool initialState;

    void Start()
    {
        initialState = gameObjectToEnable.activeSelf;
    }

    void Update()
    {

    }

    override public void Interact(InteractionType interactionType, GameObject interactionParent = null)
    {
        if (reversible || gameObjectToEnable.activeSelf == initialState)
        {
            gameObjectToEnable.SetActive(!gameObjectToEnable.activeSelf);
        }
    }

}