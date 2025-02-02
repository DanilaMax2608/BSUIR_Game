using UnityEngine;

public class TaskTrigger : MonoBehaviour
{
    [SerializeField] private string taskDescription; // �������� �������
    [SerializeField] private TaskUI taskUI; // ������ �� ������ TaskUI
    [SerializeField] private InteractableObject door; // ������ �� ������ InteractableObject

    private void OnTriggerEnter(Collider other)
    {
        // ���������, ���� �� �� ������� ������ PlayerMovement
        if (other.GetComponent<PlayerMovement>() != null)
        {
            if (taskUI != null)
            {
                taskUI.StartCoroutine(taskUI.TypeTaskText(taskDescription));
            }

            // ��������� �����, ���� ��� �������
            if (door != null && door.isOpen)
            {
                door.CloseDoor();
            }

            // ���������� ������� ����� ��������� �������
            Destroy(gameObject);
        }
    }
}
