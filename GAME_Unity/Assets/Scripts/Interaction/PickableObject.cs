using UnityEngine;

public class PickableObject : MonoBehaviour, IPickable
{
    private MainInteractableObject mainInteractableObject;
    private BoxCollider boxCollider;
    private InteractionRequirement interactionRequirement; // Новый компонент для проверки условий
    private CandlePlace candlePlace; // Ссылка на CandlePlace для проверки состояния свечки

    void Start()
    {
        mainInteractableObject = GetComponent<MainInteractableObject>();
        boxCollider = GetComponent<BoxCollider>();
        interactionRequirement = GetComponent<InteractionRequirement>();

        // Получаем ссылку на CandlePlace, чтобы проверить состояние свечки
        candlePlace = FindObjectOfType<CandlePlace>();

        // Изначально отключаем коллайдер
        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }
    }

    void Update()
    {
        // Проверяем, выполнены ли условия для взаимодействия с объектом
        if (interactionRequirement != null && interactionRequirement.AreRequirementsMet())
        {
            // Если свечка размещена, отключаем коллайдер
            if (candlePlace != null && candlePlace.isCandlePlaced)
            {
                if (boxCollider != null && boxCollider.enabled)
                {
                    boxCollider.enabled = false;
                }
                return; // Выходим, если свечка размещена
            }

            // Если условия выполнены, включаем коллайдер для взаимодействия
            if (boxCollider != null && !boxCollider.enabled)
            {
                boxCollider.enabled = true;
            }
        }
    }

    public void PickUp()
    {
        // Логика для подбора объекта
        gameObject.SetActive(false); // Отключаем объект со сцены

        // Отправляем сигнал менеджеру о завершении задачи
        mainInteractableObject.Interact();
    }
}
