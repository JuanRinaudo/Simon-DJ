using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionType
{
    PARENT,
    GRAB,
    RELEASE,
    CLICK_DOWN,
    CLICK_UP,
    ENTER,
    EXIT,
    STAY
}

[System.Serializable]
public struct NodeData
{
    public string name;
    public Rect rect;
    public Vector2 titleSize;
    public GUIStyle textStyle;
    public GUIStyle backgroundStyle;
}

public abstract class AbstractInteractable : MonoBehaviour
{

#if UNITY_EDITOR
    [HideInInspector] public NodeData node;
    [HideInInspector] public List<AbstractInteractable> parentInteractors;
#endif

    [HideInInspector] public bool running = false;

    abstract public void Interact(InteractionType interactionType, GameObject interactionParent = null);

    public static void RunInteractionList(AbstractInteractable[] interactions, InteractionType type, GameObject parent = null)
    {
        for (int scriptIndex = 0; scriptIndex < interactions.Length; ++scriptIndex)
        {
            interactions[scriptIndex].Interact(type, parent);
        }
    }

}
