#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InteractionDebugger : EditorWindow
{

    private GameObject parentGameObject = null;
    private InteractionType interactionType = InteractionType.PARENT;
    private AbstractInteractable[] currentInteractables;

    private Vector2 scrollPosition;
    
    [MenuItem("Tools/VaRi/VR Interaction Debugger")]
    private static void Init()
    {
        InteractionDebugger window = GetWindow<InteractionDebugger>();
        window.Show();
    }

    void OnGUI()
    {
        if (Selection.activeGameObject != null)
        {
            currentInteractables = Selection.activeGameObject.GetComponentsInChildren<AbstractInteractable>();
        }

        GUILayout.Label("General Configuration", EditorStyles.boldLabel);
        interactionType = (InteractionType)EditorGUILayout.EnumPopup("Interaction type: ", interactionType);
        parentGameObject = (GameObject)EditorGUILayout.ObjectField(parentGameObject, typeof(GameObject), true);

        GUILayout.Space(16);

        GUILayout.Label("Interactables", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Count: ", EditorStyles.boldLabel);
        GUILayout.Label(currentInteractables != null ? currentInteractables.Length.ToString() : "0");
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(16);

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        if (currentInteractables != null) {
            for (int interactableIndex = 0; interactableIndex < currentInteractables.Length; ++interactableIndex)
            {
                AbstractInteractable interactable = currentInteractables[interactableIndex];

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(interactable.gameObject.name + " - " + interactable.GetType().Name, EditorStyles.boldLabel);
                if (GUILayout.Button("Interact", GUILayout.Width(100)))
                {
                    interactable.Interact(interactionType, parentGameObject);
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndScrollView();
    }

}
#endif