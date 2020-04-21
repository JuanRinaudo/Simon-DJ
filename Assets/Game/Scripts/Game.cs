using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public enum Hand
{
    LEFT = 0,
    RIGHT = 1
}

public enum StepValues
{
    PERFECT,
    GOOD,
    MISS
}

public struct GameData
{
    public float playTime;
    public float health;
    public int perfects;
    public int goods;
    public int misses;
    public StepValues lastStep;
    public InteractableObject[] currentGrabItem;
    public Character[] characters;
    public int characterCount;
}

public class Game : MonoBehaviour
{

    public static bool godMode = false;
    public static bool vrDisabled = false;

    public static Game instance;

    public static GameData data;

    public StepPlayer stepPlayer;
    [HideInInspector]
    public InteractMenuButton[] menuButtons;
    [HideInInspector]
    public InteractionBehaviour[] handInteractions;

    void Awake()
    {
        if(instance)
        {
            Destroy(gameObject);
        }
        else {
            instance = this;
            Init();
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Init()
    {
        data.playTime = 0;
        data.health = 1;
        data.perfects = 0;
        data.goods = 0;
        data.misses = 0;
        data.lastStep = StepValues.PERFECT;
        data.currentGrabItem = new InteractableObject[2];
        data.characters = new Character[20];
        data.characterCount = 0;

        handInteractions = new InteractionBehaviour[2];
        menuButtons = new InteractMenuButton[System.Enum.GetNames(typeof(MenuButtonType)).Length];

#if UNITY_EDITOR
        stepPlayer.PlaySteps(0);
#endif
    }

    public static void StartNewGame()
    {
        data.health = 1;
        data.perfects = 0;
        data.goods = 0;
        data.misses = 0;
        data.lastStep = StepValues.PERFECT;
    }

    public static void ClearPlayerHands()
    {
        for(int handIndex = 0; handIndex < 2; ++handIndex)
        {

        }
    }
    
    void Update()
    {
        data.health = Mathf.Clamp(data.health, 0, 1);
        data.playTime += Time.deltaTime;

        if(data.health <= 0)
        {
            for (int buttonIndex = 0; buttonIndex < menuButtons.Length; ++buttonIndex)
            {
                menuButtons[buttonIndex].ResetState();
            }

            stepPlayer.StopSteps();
        }
    }

    public static Color GetStepColor(StepValues stepValue)
    {
        switch(stepValue)
        {
            case StepValues.PERFECT:
                return Color.green;
            case StepValues.GOOD:
                return Color.blue;
            case StepValues.MISS:
                return Color.red;
        }

        return Color.white;
    }

}
