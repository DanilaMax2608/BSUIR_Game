using UnityEngine;

public class CandlePlace : MonoBehaviour, IInteractable
{
    [SerializeField] private string candleName = "������"; // ��� ������� ������
    [SerializeField] private Transform candlePlaceTransform; // ��������� ��� ���������� ������

    public bool isCandlePlaced = false;
    private MainInteractableObject mainInteractableObject;
    private InteractionRequirement interactionRequirement; // ����� ��������� ��� �������� �������
    private Collider placeCollider; // ��������� ����� ��� �����

    void Start()
    {
        mainInteractableObject = GetComponent<MainInteractableObject>();
        interactionRequirement = GetComponent<InteractionRequirement>();
        placeCollider = GetComponent<Collider>(); // �������� ��������� ����� ��� �����

        // ���������� ��������� ���������
        if (placeCollider != null)
        {
            placeCollider.enabled = false;
        }
    }

    void Update()
    {
        // ��������� ���������� ������� ����� ���������������
        if (interactionRequirement != null && interactionRequirement.AreRequirementsMet())
        {
            // �������� ���������, ���� ������� ��������� � ������ �� ���������
            if (placeCollider != null && !placeCollider.enabled && !isCandlePlaced)
            {
                placeCollider.enabled = true;
            }
        }
    }

    public void Interact()
    {
        // ��������� ���������� ������� ����� ���������������
        if (interactionRequirement != null && !interactionRequirement.AreRequirementsMet())
        {
            return;
        }

        Bag playerBag = FindObjectOfType<Bag>();
        GameObject candle = playerBag.GetObject(candleName);

        if (!isCandlePlaced)
        {
            if (candle != null)
            {
                playerBag.RemoveObject(candle);
                candle.transform.position = candlePlaceTransform.position;
                candle.transform.rotation = candlePlaceTransform.rotation;
                candle.transform.SetParent(candlePlaceTransform);
                candle.SetActive(true);

                // ��������� ��� BoxCollider � ����� � � ������
                DisableColliders();

                isCandlePlaced = true;

                // ���������� ������ Candle
                Candle candleScript = GetComponent<Candle>();
                if (candleScript != null)
                {
                    candleScript.CheckForLighter(candle);
                }

                // ���������� ������ ��������� � ���������� ������
                mainInteractableObject.Interact();
            }
        }
        else
        {
            // ���� ������ ��� ���������, ��������� ������� ���������
            Candle candleScript = GetComponent<Candle>();
            if (candleScript != null)
            {
                GameObject placedCandle = candlePlaceTransform.GetChild(0).gameObject;
                candleScript.CheckForLighter(placedCandle);
            }
        }
    }

    private void DisableColliders()
    {
        // ��������� ���������� � �����
        if (placeCollider != null)
        {
            placeCollider.enabled = false;
        }

        // ��������� ��� ���������� � ������
        GameObject placedCandle = candlePlaceTransform.GetChild(0).gameObject; // �������� ����������� ������
        DisableAllCollidersInChildren(placedCandle);
    }

    private void DisableAllCollidersInChildren(GameObject obj)
    {
        Collider[] colliders = obj.GetComponentsInChildren<Collider>(true); // �������� ��� ���������� � �������� ��������

        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
    }
}
