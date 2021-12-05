using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class TaskListEditor : EditorWindow
{
    VisualElement container;
    [MenuItem("GameAssets/Task List")]
    public static void ShowWindow()
    {
        TaskListEditor window = GetWindow<TaskListEditor>();
        window.titleContent = new GUIContent("Task List");
        window.minSize = new  Vector2(500, 500); // Creating a window
    }

    public void CreateGUI()
    {
        container = rootVisualElement;

        VisualTreeAsset visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/GameAssets/TaskList/Editor/EditorWindow/TaskListEditor.uxml");
        container.Add(visualTreeAsset.Instantiate()); // Adding uxml template

        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/GameAssets/TaskList/Editor/EditorWindow/TaskListEditor.uss");
        container.styleSheets.Add(styleSheet); // Adding uss styles
    }
}
