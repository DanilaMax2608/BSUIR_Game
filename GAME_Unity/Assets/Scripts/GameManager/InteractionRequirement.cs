using UnityEngine;

public class InteractionRequirement : MonoBehaviour
{
    public string[] requiredEvents; // Массив с названиями необходимых событий
    private EventManager eventManager;

    void Start()
    {
        eventManager = FindObjectOfType<EventManager>();
    }

    // Проверяет, выполнены ли все необходимые события
    public bool AreRequirementsMet()
    {
        if (eventManager == null)
        {
            return false;
        }

        foreach (string eventName in requiredEvents)
        {
            if (!eventManager.IsEventCompleted(eventName))
            {
                return false;
            }
        }
        return true;
    }
}
