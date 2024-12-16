using System.Collections.Generic;
using UnityEngine;

public class Task
{
    public string description;
    public List<string> requiredItems; // Items needed to complete this task
    public bool isCompleted;
    public System.Action onComplete; // Event that triggers when task completes

    public Task(string description, List<string> requiredItems, System.Action onComplete)
    {
        this.description = description;
        this.requiredItems = requiredItems;
        this.isCompleted = false;
        this.onComplete = onComplete;
    }

    public bool CheckRequirements(List<string> playerInventory)
    {
        // Check if the player has all required items for this task
        foreach (var item in requiredItems)
        {
            if (!playerInventory.Contains(item))
            {
                return false;
            }
        }
        return true;
    }
}