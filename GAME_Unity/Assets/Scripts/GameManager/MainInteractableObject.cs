using UnityEngine;

public class MainInteractableObject : MonoBehaviour
{
    public string eventName; // �������� �������, ���������� � ���� ��������
    private EventManager eventManager;

    void Start()
    {
        eventManager = FindObjectOfType<EventManager>();
    }

    public void Interact()
    {
        // ���������� ������ ��������� � ���������� ������
        eventManager.CompleteTask(eventName);
    }
}
