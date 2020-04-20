using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public enum Hand
{
    LEFT = 0,
    RIGHT = 1
}

public struct GameData
{
    public float playTime;
    public float health;
    public int perfects;
    public int goods;
    public int misses;
    public InteractableObject[] currentGrabItem;
    public Character[] characters;
    public int characterCount;
}

public class Game : MonoBehaviour
{

    public static bool godMode = false;

    public static Game instance;

    public static GameData data;
    
    public static InteractionBehaviour[] handInteractions;

    public StepPlayer stepPlayer;
    public StepList stepList;

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
        data.currentGrabItem = new InteractableObject[2];
        data.characters = new Character[20];
        data.characterCount = 0;

        handInteractions = new InteractionBehaviour[2];

        stepPlayer.PlaySteps(stepList);
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
    }

}
