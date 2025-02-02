using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PlacementSpot : MonoBehaviour, IInteractable
{
    [SerializeField] private string requiredObjectName; // ��� ���������� �������
    [SerializeField] private TextMeshProUGUI messageText; // UI ����� ��� ���������
    [SerializeField] private float displayTime = 3f; // ����� ����������� ������
    [SerializeField] private float typingSpeed = 0.1f; // �������� ������ ������
    [SerializeField] private Transform placementPoint; // ����� ��� ���������� �������

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

                // ��������� BoxCollider � ����� ����������
                Collider placeCollider = GetComponent<Collider>();
                if (placeCollider != null)
                {
                    placeCollider.enabled = false;
                }

                // ��������� BoxCollider � ������������ �������
                Collider objectCollider = requiredObject.GetComponent<Collider>();
                if (objectCollider != null)
                {
                    objectCollider.enabled = false;
                }

                isOccupied = true;
            }
            else
            {
                ShowMessage("� ��� ��� " + requiredObjectName + ".");
            }
        }
        else
        {
            // ���� ������ ��� ��������, ����� �������� �������������� ������, ���� ����������
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
