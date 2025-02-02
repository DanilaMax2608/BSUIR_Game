using UnityEngine;

public class PickableObject : MonoBehaviour, IPickable
{
    private MainInteractableObject mainInteractableObject;
    private BoxCollider boxCollider;
    private InteractionRequirement interactionRequirement; // ����� ��������� ��� �������� �������
    private CandlePlace candlePlace; // ������ �� CandlePlace ��� �������� ��������� ������

    void Start()
    {
        mainInteractableObject = GetComponent<MainInteractableObject>();
        boxCollider = GetComponent<BoxCollider>();
        interactionRequirement = GetComponent<InteractionRequirement>();

        // �������� ������ �� CandlePlace, ����� ��������� ��������� ������
        candlePlace = FindObjectOfType<CandlePlace>();

        // ���������� ��������� ���������
        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }
    }

    void Update()
    {
        // ���������, ��������� �� ������� ��� �������������� � ��������
        if (interactionRequirement != null && interactionRequirement.AreRequirementsMet())
        {
            // ���� ������ ���������, ��������� ���������
            if (candlePlace != null && candlePlace.isCandlePlaced)
            {
                if (boxCollider != null && boxCollider.enabled)
                {
                    boxCollider.enabled = false;
                }
                return; // �������, ���� ������ ���������
            }

            // ���� ������� ���������, �������� ��������� ��� ��������������
            if (boxCollider != null && !boxCollider.enabled)
            {
                boxCollider.enabled = true;
            }
        }
    }

    public void PickUp()
    {
        // ������ ��� ������� �������
        gameObject.SetActive(false); // ��������� ������ �� �����

        // ���������� ������ ��������� � ���������� ������
        mainInteractableObject.Interact();
    }
}
