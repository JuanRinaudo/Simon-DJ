using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;

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
    public Character[] characters;
    public int characterCount;
}

public class Game : MonoBehaviour
{

    public static bool godMode = false;
    public static bool vrDisabled = false;

    public static Game instance;

    public static GameData data;

    [Header("VR Loaders")]
    public XRLoader openVRLoader;
    public XRLoader mockLoader;

    [Header("Game")]
    public StepPlayer stepPlayer;

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
        XRGeneralSettings.Instance.Manager.loaders.Clear();
        XRGeneralSettings.Instance.Manager.loaders.Add(openVRLoader);

        try {
            XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
            XRGeneralSettings.Instance.Manager.StartSubsystems();

            var xrDisplaySubsystems = new List<XRDisplaySubsystem>();
            SubsystemManager.GetInstances(xrDisplaySubsystems);
            bool success = xrDisplaySubsystems[0].running;
        }
        catch
        {
            //Initialization was not successfull, load mock instead.
            print("Failed to load openvr, loading mock HMD");

            //Deinitialize OpenVR
            XRGeneralSettings.Instance.Manager.loaders.Clear();
            XRGeneralSettings.Instance.Manager.StopSubsystems();
            XRGeneralSettings.Instance.Manager.DeinitializeLoader();

            Game.vrDisabled = true;
        }

        if (vrDisabled)
        {

        }
        else
        {
            XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
            XRGeneralSettings.Instance.Manager.StartSubsystems();
        }

        data = new GameData();
        data.playTime = 0;
        data.health = 1;
        data.perfects = 0;
        data.goods = 0;
        data.misses = 0;
        data.lastStep = StepValues.PERFECT;
        data.characters = new Character[20];
        data.characterCount = 0;

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
