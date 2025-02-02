using UnityEngine;

public class InteractionRequirement : MonoBehaviour
{
    public string[] requiredEvents; // ������ � ���������� ����������� �������
    private EventManager eventManager;

    void Start()
    {
        eventManager = FindObjectOfType<EventManager>();
    }

    // ���������, ��������� �� ��� ����������� �������
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
