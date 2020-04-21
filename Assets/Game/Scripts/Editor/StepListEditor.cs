using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StepList))]
public class StepListEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        StepList config = (StepList)target;

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Index", GUILayout.Width(128));
        GUILayout.Label("Start time", GUILayout.Width(128));
        GUILayout.Label("Perfect time", GUILayout.Width(128));
        GUILayout.Label("Value/Duration", GUILayout.Width(128));
        GUILayout.Label("Component", GUILayout.Width(128));
        EditorGUILayout.EndHorizontal();

        for (int i = 0; i < config.steps.Length; i++)
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

            ref Step step = ref config.steps[i];

            GUILayout.Label(i.ToString(), GUILayout.Width(128));
            step.startTime = EditorGUILayout.FloatField(step.startTime, GUILayout.Width(128));
            step.perfectTime = EditorGUILayout.FloatField(step.perfectTime, GUILayout.Width(128));
            GUI.enabled = ComponentHasValue(step.component);
            step.value = EditorGUILayout.FloatField(GUI.enabled ? step.value : -1, GUILayout.Width(128));
            GUI.enabled = true;
            step.component = (DJComponent)EditorGUILayout.EnumPopup(step.component, GUILayout.Width(128));

            if (GUILayout.Button("↑", GUILayout.Width(32)))
            {
                MoveStep(config, i, -1);
            }
            if (GUILayout.Button("↓", GUILayout.Width(32)))
            {
                MoveStep(config, i, +1);
            }
            if (GUILayout.Button("+", GUILayout.Width(32)))
            {
                AddStep(config, i + 1);
            }
            if (GUILayout.Button("-", GUILayout.Width(32)))
            {
                RemoveStep(config, i);
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.LabelField("Step Count: " + config.steps.Length);

        if (GUILayout.Button("+ Unit", EditorStyles.miniButton))
        {
            AddStep(config);
        }

        EditorGUILayout.EndVertical();

        EditorUtility.SetDirty(target);
    }

    private bool ComponentHasValue(DJComponent component)
    {
        return component == DJComponent.LEFT_PITCH || component == DJComponent.RIGHT_PITCH ||
            component == DJComponent.LEFT_VOLUME || component == DJComponent.RIGHT_VOLUME ||
            component == DJComponent.LEFT_TURNTABLE || component == DJComponent.RIGHT_TURNTABLE ||
            component == DJComponent.MIXER;
    }

    private void AddStep(StepList config, int index = -1)
    {
        int newSize = config.steps.Length + 1;

        if (index == -1)
        {
            index = newSize - 1;
        }

        Step[] newSteps = new Step[newSize];

        for (int i = 0; i < config.steps.Length; i++)
        {
            int delta = i >= index ? 1 : 0;
            newSteps[i + delta] = config.steps[i];
        }
        newSteps[index] = new Step();

        config.steps = newSteps;
    }

    private void RemoveStep(StepList config, int index = -1)
    {
        int newSize = config.steps.Length - 1;

        if (index == -1)
        {
            index = config.steps.Length;
        }

        Step[] newSteps = new Step[newSize];

        for (int i = 0; i < newSize; i++)
        {
            int delta = i >= index ? 1 : 0;
            newSteps[i] = config.steps[i + delta];
        }

        config.steps = newSteps;
    }

    private void MoveStep(StepList config, int index, int delta)
    {
        int newIndex = Mathf.Clamp(index + delta, 0, config.steps.Length);
        
        Step tempStep = config.steps[index];
        
        config.steps[index] = config.steps[newIndex];
        config.steps[newIndex] = tempStep;
    }
}
