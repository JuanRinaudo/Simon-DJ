#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameDebugger : EditorWindow
{

    [MenuItem("Tools/VaRi/VR Game Debugger")]
    private static void Init()
    {
        GameDebugger window = GetWindow<GameDebugger>();
        window.Show();
    }

    private void OnGUI()
    {
        
    }

}
#endif