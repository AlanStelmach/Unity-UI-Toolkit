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
        ProgressBar taskProgressBar;
        ToolbarSearchField toolbarSearchField;
        Label notification;

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
            container = rootVisualElement; // Root element of GUI

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
            taskProgressBar = container.Q<ProgressBar>("taskProgressBar");
            toolbarSearchField = container.Q<ToolbarSearchField>("searchBox");
            notification = container.Q<Label>("notification");

            // Debug.Log(taskText); // Logs
            // Debug.Log(addTaskButton);
            // Debug.Log(taskListScrollView);

            addTaskButton.clicked += AddTask; // Adding method/function to the click event
            taskText.RegisterCallback<KeyDownEvent>(AddTask); // Adding "Enter" key event
            loadSavedTasks.clicked += LoadTasks;
            savedTasksObjectField.objectType = typeof(TaskListSO); // Setting object type of TaskListSO
            saveProgressButton.clicked += SaveProgress;
            toolbarSearchField.RegisterValueChangedCallback(OnSearchTextCahnged);

            UpdateNotifications("Please load a task list.");
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
                UpdateProgress();
                UpdateNotifications("Task added!");
            }
        }

        TaskItem CreateToggle(string value) // Creating element of task list
        {
            TaskItem taskItem = new TaskItem(value);
            taskItem.GetTaskLabel().text = value;
            taskItem.GetTaskToggle().RegisterValueChangedCallback(UpdateProgress);
            return taskItem;
        }

        void AddTask(KeyDownEvent e) // Reacting on key event
        {
            if (Event.current.Equals(Event.KeyboardEvent("Return")))
            {
                AddTask();
            }
        }

        void LoadTasks() // Loading tasks as a element of TaskListSO type
        {
            taskListSO = savedTasksObjectField.value as TaskListSO;

            if(taskListSO != null)
            {
                taskListScrollView.Clear();
                List<string> tasks = taskListSO.GetTasks();
                foreach (string task in tasks)
                {
                    TaskItem taskToggle = CreateToggle(task);
                    taskListScrollView.Add(taskToggle);
                }
                UpdateProgress();
                UpdateNotifications("Task list loaded " + taskListSO.name);
            }
            else
            {
                UpdateNotifications("Failed to load!");
            }    
        }

        void SaveTask(string saveTask) // Saving task
        {
            taskListSO.AddTask(saveTask);
            EditorUtility.SetDirty(taskListSO);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        void SaveProgress() // Saving progress after completing the task
        {
            if (taskListSO != null)
            {
                List<string> tasks = new List<string>();
                foreach(TaskItem task in taskListScrollView.Children())
                {
                    if(!task.GetTaskToggle().value)
                    {
                        tasks.Add(task.GetTaskLabel().text);
                    }
                }

                taskListSO.AddTasks(tasks);
                EditorUtility.SetDirty(taskListSO);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                LoadTasks();
                UpdateNotifications("Tasks saved.");
            }
        }

        void UpdateProgress()
        {
            int count = 0;
            int completed = 0;

            foreach(TaskItem task in taskListScrollView.Children())
            {
                if(task.GetTaskToggle().value)
                {
                    completed++;
                }
                count++;
            }

            if(count > 0)
            {
                float progress = completed / (float)count;
                taskProgressBar.value = progress;
                taskProgressBar.title = string.Format("{0} %", Mathf.Round(progress * 1000) / 10f);
                UpdateNotifications("Progress updated! Please save.");
            }
            else
            {
                taskProgressBar.value = 1;
                taskProgressBar.title = string.Format("{0} %", 100);
            }
        }

        void UpdateProgress(ChangeEvent<bool> e)
        {
            UpdateProgress();
        }

        void OnSearchTextCahnged(ChangeEvent<string> e)
        {
            string searchText = e.newValue.ToUpper();
            foreach(TaskItem task in taskListScrollView.Children())
            {
                string taskText = task.GetTaskLabel().text.ToUpper();

                if(!string.IsNullOrEmpty(searchText) && taskText.Contains(searchText))
                {
                    task.GetTaskLabel().AddToClassList("highlight");
                }
                else
                {
                    task.GetTaskLabel().RemoveFromClassList("highlight");
                }    
            }
        }

        void UpdateNotifications(string text)
        {
            if(!string.IsNullOrEmpty(text))
            {
                notification.text = text;
            }
        }
    }
}
