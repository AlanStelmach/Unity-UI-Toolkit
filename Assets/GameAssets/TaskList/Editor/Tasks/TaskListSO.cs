using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAssets.Tasks 
{
    [CreateAssetMenu(menuName = "Task List", fileName = "New Task List")] // Element of Asset Menu
    public class TaskListSO : ScriptableObject
    {
        [SerializeField] List<string> tasks = new List<string>(); // List of tasks

        public List<string> GetTasks() // Getter
        {
            return tasks;
        }

        public void AddTask(string savedTask)
        {
            tasks.Add(savedTask);
        }

        public void AddTasks(List<string> savedTasks) // Save method
        {
            tasks.Clear();
            tasks = savedTasks;
        }
    }
}
