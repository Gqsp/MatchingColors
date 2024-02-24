using UnityEngine;
using GaspDL.SaveSystem;

public class App: MonoBehaviour
{
    public static bool IsEditor;

    public const string ApplicationPath = "Architecture/App";

    public static Backdrop Backdrop;
    public static SaveManager SaveManager;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        var app = Object.Instantiate(Resources.Load(ApplicationPath)) as GameObject;

        if (app == null)
            throw new System.ApplicationException();

        Object.DontDestroyOnLoad(app);
        app.name = "Application";

        Backdrop = app.GetComponent<Backdrop>();
        SaveManager = app.GetComponent<SaveManager>();
    }
}