using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/*
public class CreateHandles
{
#if UNITY_EDITOR
    [MenuItem("Assets/Create/World Handle/Create Scene Handle From Scene")]
    public static void CreateFromScene()
    {
        SceneHandleSO sceneHandle = ScriptableObject.CreateInstance<SceneHandleSO>();

        AssetDatabase.CreateAsset(sceneHandle, "Assets/_Ressources/ScriptableObjects/World/Handles/SceneHandles/" + Selection.activeObject.name + "Handle.asset");
        AssetDatabase.SaveAssets();

        sceneHandle.scene.ScenePath = AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID());
        sceneHandle.sceneName = Selection.activeObject.name;

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = sceneHandle;
    }

    [MenuItem("Assets/Create/World Handle/Create Scene Handle From Scene", true)]
    public static bool ValidateCreateFromScene()
    {
        return Selection.activeObject.GetType() == typeof(SceneAsset);
    }
}

#endif

*/


