using UnityEngine;
using UnityEditor;


/*
[CustomPropertyDrawer(typeof(PassageHandleSO))]
public class PassageHandlePropertyDrawer : PropertyDrawer
{
    private UnityEditor.Editor editor = null;

    static readonly RectOffset boxPadding = EditorStyles.helpBox.padding;
    static readonly float padSize = 2f;
    static readonly float lineHeight = EditorGUIUtility.singleLineHeight;
    static readonly float paddedLine = lineHeight + padSize;
    static readonly float footerHeight = 10f;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property, label, true);

        if (property.objectReferenceValue != null)
        {
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none);
        }

        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;
            if (!editor)
                UnityEditor.Editor.CreateCachedEditor(property.objectReferenceValue, null, ref editor);
            editor.OnInspectorGUI();
        }

        EditorGUI.indentLevel--;
    }
}
*/