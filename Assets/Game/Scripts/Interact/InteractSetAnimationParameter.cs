using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSetAnimationParameter : AbstractInteractable
{

    [System.Serializable]
    public enum ParameterTypes
    {
        FLOAT,
        INT,
        BOOL,
        TRIGGER
    }

    [System.Serializable]
    public struct InteractAnimateSetup
    {
        public ParameterTypes parameter;
        public InteractionType interaction;
        public string key;
        public float floatValue;
    }

    public Animator animator;
    public InteractAnimateSetup[] configuration;

    public override void Interact(InteractionType interactionType, GameObject interactionParent = null)
    {
        for(int setupIndex = 0; setupIndex < configuration.Length; ++setupIndex)
        {
            InteractAnimateSetup setup = configuration[setupIndex];
            if(setup.interaction == interactionType)
            {
                switch (setup.parameter)
                {
                    case ParameterTypes.BOOL:
                        animator.SetBool(setup.key, setup.floatValue == 1);
                        break;
                    case ParameterTypes.FLOAT:
                        animator.SetFloat(setup.key, setup.floatValue);
                        break;
                    case ParameterTypes.INT:
                        animator.SetInteger(setup.key, (int)setup.floatValue);
                        break;
                    case ParameterTypes.TRIGGER:
                        animator.SetTrigger(setup.key);
                        break;
                } 
            }
        }
    }

}
