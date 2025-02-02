using UnityEngine;

public class TaskTrigger : MonoBehaviour
{
    [SerializeField] private string taskDescription; // Описание задания
    [SerializeField] private TaskUI taskUI; // Ссылка на скрипт TaskUI
    [SerializeField] private InteractableObject door; // Ссылка на скрипт InteractableObject

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, есть ли на объекте скрипт PlayerMovement
        if (other.GetComponent<PlayerMovement>() != null)
        {
            if (taskUI != null)
            {
                taskUI.StartCoroutine(taskUI.TypeTaskText(taskDescription));
            }

            // Закрываем дверь, если она открыта
            if (door != null && door.isOpen)
            {
                door.CloseDoor();
            }

            // Уничтожаем триггер после активации задания
            Destroy(gameObject);
        }
    }
}
