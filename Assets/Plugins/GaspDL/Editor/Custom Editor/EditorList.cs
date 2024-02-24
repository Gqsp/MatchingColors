using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class EditorList
{
    public static void DisplayList(SerializedProperty list)
    {
        //EditorGUILayout.PropertyField(list);
        EditorGUI.indentLevel++;
        for (int i = 0; i < list.arraySize; i++)
        {
            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
            GUILayout.Space(10f);
        }
        EditorGUI.indentLevel--;
    }
}
