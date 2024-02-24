using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GaspDL;

public class SceneHandle : ScriptableObject
{
    [HideInInspector]
    public string Title;
    public SceneReference scene;
}
