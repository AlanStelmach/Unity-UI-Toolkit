using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace GameAssets.Tasks
{
    public class TaskListEditor : EditorWindow
    {
        VisualElement container;
        TextField taskText;
        Button addTaskButton;
        ScrollView taskListScrollView;
        ObjectField savedTasksObjectField;
        Button loadSavedTasks;
        TaskListSO taskListSO;
        Button saveProgressButton;

        public const string path = "Assets/GameAssets/TaskList/Editor/EditorWindow/";
        [MenuItem("GameAssets/Task List")]
        public static void ShowWindow()
        {
            TaskListEditor window = GetWindow<TaskListEditor>();
            window.titleContent = new GUIContent("Task List");
            window.minSize = new Vector2(500, 800); // Creating a window
            window.maxSize = new Vector2(500, 800);
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
            savedTasksObjectField = container.Q<ObjectField>("savedTasksObjectField");
            loadSavedTasks = container.Q<Button>("loadTasksButton");
            saveProgressButton = container.Q<Button>("saveProgressButton");

            // Debug.Log(taskText); // Logs
            // Debug.Log(addTaskButton);
            // Debug.Log(taskListScrollView);

            addTaskButton.clicked += AddTask; // Adding method/function to the click event
            taskText.RegisterCallback<KeyDownEvent>(AddTask); // Adding "Enter" key event
            loadSavedTasks.clicked += LoadTasks;
            savedTasksObjectField.objectType = typeof(TaskListSO); // Setting object type of TaskListSO
            saveProgressButton.clicked += SaveProgress;
        }

        void AddTask() // Adding method
        {
            Debug.Log("Task Added");
            if (!string.IsNullOrEmpty(taskText.value))
            {
                taskListScrollView.Add(CreateToggle(taskText.value));  // Adding new task to list
                SaveTask(taskText.value);
                taskText.value = "";
                taskText.Focus();
            }
        }

        Toggle CreateToggle(string value)
        {
            Toggle taskItem = new Toggle();
            taskItem.text = value;
            return taskItem;
        }

        void AddTask(KeyDownEvent e) // Reacting on key event
        {
            if (Event.current.Equals(Event.KeyboardEvent("Return")))
            {
                AddTask();
            }
        }

        void LoadTasks()
        {
            taskListSO = savedTasksObjectField.value as TaskListSO;

            if(taskListSO != null)
            {
                taskListScrollView.Clear();
                List<string> tasks = taskListSO.GetTasks();
                foreach (string task in tasks)
                {
                    Toggle taskToggle = CreateToggle(task);
                    taskListScrollView.Add(taskToggle);
                }
            }
        }

        void SaveTask(string saveTask)
        {
            taskListSO.AddTask(saveTask);
            EditorUtility.SetDirty(taskListSO);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        void SaveProgress()
        {
            if (taskListSO != null)
            {
                List<string> tasks = new List<string>();
                foreach(Toggle task in taskListScrollView.Children())
                {
                    if(!task.value)
                    {
                        tasks.Add(task.text);
                    }
                }

                taskListSO.AddTasks(tasks);
                EditorUtility.SetDirty(taskListSO);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                LoadTasks();
            }
        }
    }
}
