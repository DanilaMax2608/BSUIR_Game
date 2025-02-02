using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PlacementSpot : MonoBehaviour, IInteractable
{
    [SerializeField] private string requiredObjectName; // Имя требуемого объекта
    [SerializeField] private TextMeshProUGUI messageText; // UI текст для сообщений
    [SerializeField] private float displayTime = 3f; // Время отображения текста
    [SerializeField] private float typingSpeed = 0.1f; // Скорость печати текста
    [SerializeField] private Transform placementPoint; // Точка для размещения объекта

    private bool isOccupied = false;
    private bool isMessageShown = false;
    private Coroutine typingCoroutine;
    private Coroutine displayCoroutine;

    [System.Obsolete]
    public void Interact()
    {
        Bag playerBag = FindObjectOfType<Bag>();
        GameObject requiredObject = playerBag.GetObject(requiredObjectName);

        if (!isOccupied)
        {
            if (requiredObject != null)
            {
                playerBag.RemoveObject(requiredObject);
                requiredObject.transform.position = placementPoint.position;
                requiredObject.transform.rotation = placementPoint.rotation;
                requiredObject.transform.SetParent(placementPoint);
                requiredObject.SetActive(true);

                // Отключаем BoxCollider у места размещения
                Collider placeCollider = GetComponent<Collider>();
                if (placeCollider != null)
                {
                    placeCollider.enabled = false;
                }

                // Отключаем BoxCollider у размещенного объекта
                Collider objectCollider = requiredObject.GetComponent<Collider>();
                if (objectCollider != null)
                {
                    objectCollider.enabled = false;
                }

                isOccupied = true;
            }
            else
            {
                ShowMessage("У вас нет " + requiredObjectName + ".");
            }
        }
        else
        {
            // Если объект уже размещен, можно добавить дополнительную логику, если необходимо
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
