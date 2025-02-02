using TMPro;
using UnityEngine;
using System.Collections;

public class TaskUI : MonoBehaviour
{
    public TextMeshProUGUI taskText; // Текст для отображения задачи
    public float typingSpeed = 0.05f; // Скорость печати
    public float displayTime = 3f; // Время отображения текста
    public float moveDuration = 2f; // Время движения текста
    public float acceleration = 2f; // Ускорение движения текста

    private Vector3 originalPosition; // Исходная позиция текста
    private Coroutine typingCoroutine;
    private Coroutine displayCoroutine;
    private Coroutine moveCoroutine;

    void OnEnable()
    {
        EventManager.onTaskUpdated += UpdateTaskText;
        originalPosition = taskText.rectTransform.anchoredPosition; // Сохраняем исходную позицию текста
    }

    void OnDisable()
    {
        EventManager.onTaskUpdated -= UpdateTaskText;
    }

    private void UpdateTaskText(string taskDescription)
    {
        StopAllCoroutines(); // Останавливаем все текущие корутины
        taskText.rectTransform.anchoredPosition = originalPosition; // Возвращаем текст на исходную позицию
        typingCoroutine = StartCoroutine(TypeTaskText(taskDescription));
    }

    public IEnumerator TypeTaskText(string taskDescription)
    {
        taskText.text = ""; // Очищаем текущий текст
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
        Vector3 endPosition = startPosition + new Vector3(0, -Screen.height, 0); // Изменяем координаты для движения вниз
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;
            float accelerationFactor = Mathf.Lerp(0f, acceleration, t);
            rectTransform.anchoredPosition = Vector3.Lerp(startPosition, endPosition, t * accelerationFactor);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Убедимся, что текст остановился за экраном
        rectTransform.anchoredPosition = endPosition;
    }
}
