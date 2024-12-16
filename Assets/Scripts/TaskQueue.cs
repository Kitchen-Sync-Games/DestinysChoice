using System.Collections.Generic;
using UnityEngine;

public class TaskQueue : MonoBehaviour
{
    public List<Task> tasks = new List<Task>();
    private int currentIndex = 0;
    private List<string> playerInventory = new List<string>(); // Mock player inventory

    void Start()
    {
        // Initialize tasks here (for demonstration purposes)
        tasks.Add(new Task("Find the flashlight", new List<string> { "flashlight" }, () => Debug.Log("Flashlight found!")));
        tasks.Add(new Task("Unlock the door", new List<string> { "lockpick" }, () => Debug.Log("Door unlocked!")));
        
        ProcessCurrentTask();
    }

    public void ProcessCurrentTask()
    {
        if (currentIndex >= tasks.Count) return;

        Task currentTask = tasks[currentIndex];

        if (currentTask.CheckRequirements(playerInventory))
        {
            CompleteTask(currentTask);
        }
    }

    public void CompleteTask(Task task)
    {
        task.isCompleted = true;
        task.onComplete?.Invoke();

        // Move to the next task
        currentIndex++;
        ProcessCurrentTask();
    }

    public void AddToInventory(string item)
    {
        playerInventory.Add(item);
        ProcessCurrentTask(); // Check if new item allows for task completion
    }
}