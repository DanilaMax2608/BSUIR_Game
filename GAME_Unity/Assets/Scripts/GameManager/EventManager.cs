using UnityEngine;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    public List<GameEvent> events; // Список событий
    private int currentEventIndex = 0;

    public delegate void OnTaskUpdated(string taskDescription);
    public static event OnTaskUpdated onTaskUpdated;

    private Dictionary<string, bool> completedEvents = new Dictionary<string, bool>();

    void Start()
    {
        UpdateTask();
    }

    public void CompleteTask(string eventName)
    {
        if (!completedEvents.ContainsKey(eventName))
        {
            completedEvents[eventName] = true;
        }

        currentEventIndex++;
        if (currentEventIndex < events.Count)
        {
            UpdateTask();
        }
    }

    private void UpdateTask()
    {
        if (currentEventIndex < events.Count)
        {
            onTaskUpdated?.Invoke(events[currentEventIndex].taskDescription);
        }
    }

    public bool IsEventCompleted(string eventName)
    {
        bool isCompleted = completedEvents.ContainsKey(eventName) && completedEvents[eventName];
        return isCompleted;
    }
}
