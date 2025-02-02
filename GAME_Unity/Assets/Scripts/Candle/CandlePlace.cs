using UnityEngine;

public class CandlePlace : MonoBehaviour, IInteractable
{
    [SerializeField] private string candleName = "Свечка"; // Имя объекта свечки
    [SerializeField] private Transform candlePlaceTransform; // Трансформ для размещения свечки

    public bool isCandlePlaced = false;
    private MainInteractableObject mainInteractableObject;
    private InteractionRequirement interactionRequirement; // Новый компонент для проверки условий
    private Collider placeCollider; // Коллайдер места для свечи

    void Start()
    {
        mainInteractableObject = GetComponent<MainInteractableObject>();
        interactionRequirement = GetComponent<InteractionRequirement>();
        placeCollider = GetComponent<Collider>(); // Получаем коллайдер места для свечи

        // Изначально отключаем коллайдер
        if (placeCollider != null)
        {
            placeCollider.enabled = false;
        }
    }

    void Update()
    {
        // Проверяем выполнение условий перед взаимодействием
        if (interactionRequirement != null && interactionRequirement.AreRequirementsMet())
        {
            // Включаем коллайдер, если условия выполнены и свечка не размещена
            if (placeCollider != null && !placeCollider.enabled && !isCandlePlaced)
            {
                placeCollider.enabled = true;
            }
        }
    }

    public void Interact()
    {
        // Проверяем выполнение условий перед взаимодействием
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

                // Отключаем все BoxCollider у места и у свечки
                DisableColliders();

                isCandlePlaced = true;

                // Активируем скрипт Candle
                Candle candleScript = GetComponent<Candle>();
                if (candleScript != null)
                {
                    candleScript.CheckForLighter(candle);
                }

                // Отправляем сигнал менеджеру о завершении задачи
                mainInteractableObject.Interact();
            }
        }
        else
        {
            // Если свечка уже размещена, проверяем наличие зажигалки
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
        // Отключаем коллайдеры у места
        if (placeCollider != null)
        {
            placeCollider.enabled = false;
        }

        // Отключаем все коллайдеры у свечки
        GameObject placedCandle = candlePlaceTransform.GetChild(0).gameObject; // Получаем размещённую свечку
        DisableAllCollidersInChildren(placedCandle);
    }

    private void DisableAllCollidersInChildren(GameObject obj)
    {
        Collider[] colliders = obj.GetComponentsInChildren<Collider>(true); // Получаем все коллайдеры в дочерних объектах

        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
    }
}
