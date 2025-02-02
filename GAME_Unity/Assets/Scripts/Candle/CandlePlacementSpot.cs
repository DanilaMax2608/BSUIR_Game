using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CandlePlacementSpot : MonoBehaviour, IInteractable
{
    [SerializeField] private string requiredObjectName; // Имя требуемого объекта
    [SerializeField] private TextMeshProUGUI messageText; // UI текст для сообщений
    [SerializeField] private float displayTime = 3f; // Время отображения текста
    [SerializeField] private float typingSpeed = 0.1f; // Скорость печати текста

    private bool isOccupied = false;
    private bool isMessageShown = false;
    private Coroutine typingCoroutine;
    private Coroutine displayCoroutine;

    public void Interact()
    {
        Bag playerBag = FindObjectOfType<Bag>();
        GameObject requiredObject = playerBag.GetObject(requiredObjectName);

        if (requiredObject != null)
        {
            requiredObject.transform.SetParent(transform);
            requiredObject.transform.position = transform.position;
            requiredObject.SetActive(true);
            requiredObject.GetComponent<CandlePickable>().enabled = false; // Отключаем скрипт CandlePickable
            requiredObject.GetComponent<Candle>().enabled = true; // Включаем скрипт Candle
            //requiredObject.GetComponent<Candle>().Initialize(); // Инициализируем скрипт Candle
            isOccupied = true;
            playerBag.RemoveObject(requiredObject);
            GetComponent<Collider>().enabled = false; // Отключаем Collider у места размещения
        }
        else
        {
            ShowMessage("У вас нет " + requiredObjectName + ".");
        }
    }

    private void ShowMessage(string message)
    {
        if (!isMessageShown)
        {
            isMessageShown = true;
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }

            if (displayCoroutine != null)
            {
                StopCoroutine(displayCoroutine);
            }

            typingCoroutine = StartCoroutine(TypeText(message));
            displayCoroutine = StartCoroutine(HideMessageAfterDelay(displayTime));
        }
    }

    private IEnumerator TypeText(string message)
    {
        messageText.text = "";
        foreach (char letter in message)
        {
            messageText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private IEnumerator HideMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        messageText.text = "";
        isMessageShown = false;
    }
}
