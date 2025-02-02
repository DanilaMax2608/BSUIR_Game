using TMPro;
using UnityEngine;
using System.Collections;

public class TaskUI : MonoBehaviour
{
    public TextMeshProUGUI taskText; // ����� ��� ����������� ������
    public float typingSpeed = 0.05f; // �������� ������
    public float displayTime = 3f; // ����� ����������� ������
    public float moveDuration = 2f; // ����� �������� ������
    public float acceleration = 2f; // ��������� �������� ������

    private Vector3 originalPosition; // �������� ������� ������
    private Coroutine typingCoroutine;
    private Coroutine displayCoroutine;
    private Coroutine moveCoroutine;

    void OnEnable()
    {
        EventManager.onTaskUpdated += UpdateTaskText;
        originalPosition = taskText.rectTransform.anchoredPosition; // ��������� �������� ������� ������
    }

    void OnDisable()
    {
        EventManager.onTaskUpdated -= UpdateTaskText;
    }

    private void UpdateTaskText(string taskDescription)
    {
        StopAllCoroutines(); // ������������� ��� ������� ��������
        taskText.rectTransform.anchoredPosition = originalPosition; // ���������� ����� �� �������� �������
        typingCoroutine = StartCoroutine(TypeTaskText(taskDescription));
    }

    public IEnumerator TypeTaskText(string taskDescription)
    {
        taskText.text = ""; // ������� ������� �����
        foreach (char letter in taskDescription.ToCharArray())
        {
            taskText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        displayCoroutine = StartCoroutine(HideTextAfterDelay(displayTime));
    }

    private IEnumerator HideTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        moveCoroutine = StartCoroutine(MoveTextOffScreen());
    }

    private IEnumerator MoveTextOffScreen()
    {
        RectTransform rectTransform = taskText.rectTransform;
        Vector3 startPosition = rectTransform.anchoredPosition;
        Vector3 endPosition = startPosition + new Vector3(0, -Screen.height, 0); // �������� ���������� ��� �������� ����
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;
            float accelerationFactor = Mathf.Lerp(0f, acceleration, t);
            rectTransform.anchoredPosition = Vector3.Lerp(startPosition, endPosition, t * accelerationFactor);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ��������, ��� ����� ����������� �� �������
        rectTransform.anchoredPosition = endPosition;
    }
}
