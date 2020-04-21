using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum DJComponent
{
    LEFT_TURNTABLE,
    LEFT_VOLUME,
    LEFT_PITCH,
    MIXER,
    RIGHT_TURNTABLE,
    RIGHT_VOLUME,
    RIGHT_PITCH,
    BUTTON_1,
    BUTTON_2,
    BUTTON_3,
    BUTTON_4,
    BUTTON_5,
    BUTTON_6,
    BUTTON_7,
    BUTTON_8,
}

[System.Serializable]
public struct Step
{
    public float startTime;
    public float perfectTime;
    public float value;
    public DJComponent component;
}

[CreateAssetMenu(menuName = "Data/StepList", fileName = "StepList")]
public class StepList : ScriptableObject
{

    public Step[] steps;
    
}
