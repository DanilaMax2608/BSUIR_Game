using UnityEngine;

public class MainInteractableObject : MonoBehaviour
{
    public string eventName; // Название события, связанного с этим объектом
    private EventManager eventManager;

    void Start()
    {
        eventManager = FindObjectOfType<EventManager>();
    }

    public void Interact()
    {
        // Отправляем сигнал менеджеру о завершении задачи
        eventManager.CompleteTask(eventName);
    }
}
