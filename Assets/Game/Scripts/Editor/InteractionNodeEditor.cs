#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

public class InteractionNodeEditor : EditorWindow
{

    private bool nodeSelected;
    private int selectedIndex;
    
    private AbstractInteractable[] currentInteractables;

    private AbstractInteractable[] hoveredIntearactors;
    private int interactorIndex = 0;

    private static float padding = 5;
    private static Vector2 cameraOffset = new Vector2();
    private static float cameraZoom = 1;

    private static Color selectedColor = new Color(1, 1, 1, 1);
    private static Color normalColor = new Color(1, 1, 1, .6f);

    [MenuItem("Tools/VaRi/Interaction Node Editor")]
    private static void Init()
    {
        InteractionNodeEditor window = GetWindow<InteractionNodeEditor>();
        window.Show();
    }

    void OnGUI()
    {
        Matrix4x4 tempSave = GUI.matrix;

        Matrix4x4 Translation = Matrix4x4.TRS(new Vector3(0, 25, 0), Quaternion.identity, Vector3.one);
        Matrix4x4 Scale = Matrix4x4.Scale(new Vector3(cameraZoom, cameraZoom, cameraZoom));
        GUI.matrix = Translation * Scale * Translation.inverse;
        
        DrawGUI();
        
        GUI.matrix = tempSave;

        DrawTopGUI();
    }

    void DrawGUI()
    {
        Event currentEvent = Event.current;

        hoveredIntearactors = null;
        interactorIndex = -1;

        if (Selection.activeGameObject != null)
        {
            currentInteractables = Selection.activeGameObject.GetComponentsInChildren<AbstractInteractable>();
        }

        if (currentInteractables != null && currentInteractables.Length > 0)
        {
            for (int interactableIndex = 0; interactableIndex < currentInteractables.Length; ++interactableIndex)
            {
                AbstractInteractable interactable = currentInteractables[interactableIndex];

                if (interactable.node.name == null || interactable.node.textStyle == null || interactable.node.backgroundStyle == null)
                {
                    interactable.node = new NodeData();
                    ResetNode(ref interactable.node, interactable);
                }

                GUI.color = interactableIndex == selectedIndex ? selectedColor : normalColor;

                Rect offsetedRect = OffsetRect(interactable.node.rect, cameraOffset);
                GUI.Box(offsetedRect, "", interactable.node.backgroundStyle);
                Rect paddedOffsetedRect = AddPadding(offsetedRect, padding * 3, padding * 3, padding, padding);
                GUI.Label(paddedOffsetedRect, interactable.node.name, interactable.node.textStyle);

                Rect inspectorRect = OffsetRect(paddedOffsetedRect, 0, interactable.node.titleSize.y);
                Editor objectEditor = Editor.CreateEditor(interactable);
                GUILayout.BeginArea(inspectorRect);
                objectEditor.OnInspectorGUI();

                ref NodeData nodeData = ref currentInteractables[interactableIndex].node;
                Rect lastRect = GUILayoutUtility.GetLastRect();
                float targetHeight = lastRect.y + lastRect.height + nodeData.titleSize.y + padding * 2;
                if (lastRect.height != 1 && nodeData.rect.height != targetHeight)
                {
                    nodeData.rect.height = targetHeight;
                }

                GUILayout.EndArea();

                AbstractInteractable[] interactors = TryToGetAbstractInteractors(interactable);
                for (int interactorIndex = 0; interactorIndex < interactors.Length; ++interactorIndex)
                {
                    float interactionSize = 15;
                    Rect interactionRect = new Rect(paddedOffsetedRect.x + paddedOffsetedRect.width, paddedOffsetedRect.y + interactorIndex * (interactionSize + padding), interactionSize, interactionSize);
                    GUI.color = Color.white;
                    if (interactionRect.Contains(currentEvent.mousePosition))
                    {
                        GUI.color = Color.blue;
                        hoveredIntearactors = interactors;
                        this.interactorIndex = interactorIndex;
                    }
                    GUI.Box(interactionRect, "", interactable.node.backgroundStyle);

                    if (interactors[interactorIndex] != null)
                    {
                        ref NodeData interactionNodeData = ref interactors[interactorIndex].node;
                        Handles.DrawLine(new Vector2(interactionNodeData.rect.x, interactionNodeData.rect.y) + cameraOffset,
                            new Vector2(interactionRect.x + interactionSize * .5f, interactionRect.y + interactionSize * .5f));
                    }
                }
            }

            switch (currentEvent.type)
            {
                case EventType.MouseDown:
                    if (currentEvent.button == 0 || currentEvent.button == 1)
                    {
                        for (int interactableIndex = 0; interactableIndex < currentInteractables.Length; ++interactableIndex)
                        {
                            AbstractInteractable interactable = currentInteractables[interactableIndex];
                            if (OffsetRect(interactable.node.rect, cameraOffset).Contains(currentEvent.mousePosition))
                            {
                                nodeSelected = true;
                                selectedIndex = interactableIndex;
                                break;
                            }
                            else
                            {
                                nodeSelected = false;
                                selectedIndex = -1;
                            }
                        }
                    }
                    break;
                case EventType.MouseUp:
                    if (hoveredIntearactors != null)
                    {
                        if (currentEvent.button == 0)
                        {
                            hoveredIntearactors[interactorIndex] = currentInteractables[selectedIndex];
                        }
                        if (currentEvent.button == 1)
                        {
                            hoveredIntearactors[interactorIndex] = null;
                        }
                    }
                    break;
                case EventType.MouseDrag:
                    if (currentEvent.button == 2 || (!nodeSelected && currentEvent.button == 0))
                    {
                        cameraOffset += currentEvent.delta;
                    }
                    else if (nodeSelected)
                    {
                        ref NodeData selectedNode = ref currentInteractables[selectedIndex].node;
                        if (currentEvent.button == 1)
                        {
                            selectedNode.rect = new Rect(selectedNode.rect.x + currentEvent.delta.x, selectedNode.rect.y + currentEvent.delta.y, selectedNode.rect.width, selectedNode.rect.height);
                        }
                    }
                    break;
                case EventType.ScrollWheel:
                    cameraZoom += currentEvent.delta.y * 0.1f;
                    break;
            }
        }

        Repaint();
    }

    void DrawTopGUI()
    {
        GUIStyle topBarStyle = new GUIStyle();
        topBarStyle.fixedHeight = 30;
        topBarStyle.normal.background = TextureUtils.CreateTexture2D(1, 1, Color.black);

        GUI.color = Color.white;
        GUILayout.BeginHorizontal(topBarStyle);
        GUILayout.Label("Nodes: " + currentInteractables.Length, GUILayout.Height(30));
        if (GUILayout.Button("Reset Camera", GUILayout.Width(100), GUILayout.Height(26)))
        {
            cameraOffset = new Vector2(0, 0);
            cameraZoom = 1;
        }
        if (GUILayout.Button("Reset Nodes", GUILayout.Width(100), GUILayout.Height(26)))
        {
            for (int interactableIndex = 0; interactableIndex < currentInteractables.Length; ++interactableIndex)
            {
                AbstractInteractable interactable = currentInteractables[interactableIndex];
                ResetNode(ref interactable.node, interactable);
            }
        }
        GUILayout.EndHorizontal();
    }

    private Rect AddPadding(Rect rect, float leftPadding, float rightPadding, float topPadding, float bottomPadding)
    {
        Rect paddedRect = new Rect(rect.x + leftPadding, rect.y + topPadding, rect.width - rightPadding - leftPadding, rect.height - bottomPadding - topPadding);
        return paddedRect;
    }

    private Rect AddPadding(Rect rect, float rectPadding = Mathf.Infinity)
    {
        if(rectPadding == Mathf.Infinity)
        {
            rectPadding = padding;
        }

        Rect paddedRect = new Rect(rect.x + rectPadding, rect.y + rectPadding, rect.width - rectPadding * 2, rect.height - rectPadding * 2);
        return paddedRect;
    }

    private Rect OffsetRect(Rect rect, float x, float y)
    {
        Rect offsetedRect = new Rect(rect.x + x, rect.y + y, rect.width, rect.height);
        return offsetedRect;
    }

    private Rect OffsetRect(Rect rect, Vector2 offset)
    {
        Rect offsetedRect = new Rect(rect.x + offset.x, rect.y + offset.y, rect.width, rect.height);
        return offsetedRect;
    }

    private void ResetNode(ref NodeData node, AbstractInteractable interactable)
    {
        node.name = interactable.GetType().Name;
        node.backgroundStyle = EditorStyles.helpBox;
        node.textStyle = GUI.skin.textField;

        Vector2 textSize = node.textStyle.CalcSize(new GUIContent(node.name));
        node.titleSize = new Vector2(textSize.x + padding * 2, textSize.y + padding * 2);
        node.rect = new Rect(50, 50, 340, 1);
    }

    private AbstractInteractable[] TryToGetAbstractInteractors(AbstractInteractable interactable)
    {
        FieldInfo[] fields = interactable.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
        for (int fieldIndex = 0; fieldIndex < fields.Length; ++fieldIndex)
        {
            if (fields[fieldIndex].FieldType == typeof(AbstractInteractable[]))
            {
                return (AbstractInteractable[])(fields[fieldIndex].GetValue(interactable));
            }
        }

        return new AbstractInteractable[0];
    }

}
#endif