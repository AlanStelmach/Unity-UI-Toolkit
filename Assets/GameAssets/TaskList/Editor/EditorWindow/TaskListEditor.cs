using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class TaskListEditor : EditorWindow
{
    VisualElement container;
    TextField taskText;
    Button addTaskButton;
    ScrollView taskListScrollView;
    public const string path = "Assets/GameAssets/TaskList/Editor/EditorWindow/";
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

        VisualTreeAsset visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path + "TaskListEditor.uxml");
        container.Add(visualTreeAsset.Instantiate()); // Adding uxml template

        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(path + "TaskListEditor.uss");
        container.styleSheets.Add(styleSheet); // Adding uss styles

        taskText = container.Q<TextField>("taskText"); // Connecting UI elements with code objects
        addTaskButton = container.Q<Button>("addTaskButton");
        taskListScrollView = container.Q<ScrollView>("taskList");

        // Debug.Log(taskText); // Logs
        // Debug.Log(addTaskButton);
        // Debug.Log(taskListScrollView);

        addTaskButton.clicked += AddTask;
    }

    public void AddTask()
    {
        Debug.Log("Task Added");
    }
}
