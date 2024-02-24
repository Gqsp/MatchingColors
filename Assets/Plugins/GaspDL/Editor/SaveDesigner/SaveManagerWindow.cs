using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

public class SaveManagerWindow : EditorWindow
{
    

    AnimBool useDebugSettings;

    // Save Data
    //public FakeSave loadedFakeSave;
    public string SaveFileName;

    // Settings
    public bool startInCurrentScene;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/My Window")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(SaveManagerWindow));
    }

    private void OnEnable()
    {
        useDebugSettings = new AnimBool(true);
        useDebugSettings.valueChanged.AddListener(Repaint);
    }

    void OnGUI()
    {
        GUILayout.Label("Save Settings", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        useDebugSettings.target = EditorGUILayout.Toggle("Use Debug Settings", useDebugSettings.target);

        if (EditorGUILayout.BeginFadeGroup(useDebugSettings.faded))
        {
            EditorGUI.indentLevel++;

            #region "File Selection"
            EditorGUILayout.Space();
            GUILayout.Label("Save File", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            
            SaveFileName = EditorGUILayout.TextField("File Name :", SaveFileName, GUILayout.MaxWidth(350));
            GUI.enabled = !string.IsNullOrEmpty(SaveFileName);
            /*
            if (GUILayout.Button(loadedFakeSave == null ? "Create Save Data" : "Update Save Data", GUILayout.MaxWidth(150), GUILayout.MinWidth(120)))
            {
                if (loadedFakeSave == null)
                {
                    CreateSaveFile();
                } else
                {
                    SaveSaveFile();
                }
                
            }
            */
            GUI.enabled = true;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            /*
            loadedFakeSave = EditorGUILayout.ObjectField("Fake Save : ", loadedFakeSave, typeof(FakeSave), true, GUILayout.MaxWidth(350)) as FakeSave;

            GUI.enabled = loadedFakeSave != null;
            */
            if (GUILayout.Button("Load Data", GUILayout.MaxWidth(150), GUILayout.MinWidth(120)))
            {
                LoadSaveFile();
            }

            GUI.enabled = true;
            GUILayout.EndHorizontal();

            #endregion

            EditorGUILayout.Space();

            GUILayout.Label("Settings", EditorStyles.boldLabel);

            EditorGUILayout.Space();

            EditorGUI.indentLevel++;


        }

        
        EditorGUILayout.EndFadeGroup();
    }

    public void LoadSaveFile()
    {

    }

    public void SaveSaveFile()
    {

    }

    public void CreateSaveFile()
    {

    }
}