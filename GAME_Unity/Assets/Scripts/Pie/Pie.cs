using UnityEngine;

public class Pie : MonoBehaviour, IPickable
{
    public static int piesEaten = 0;
    public static int totalPies = 4;

    private MainInteractableObject mainInteractableObject;
    private MeshCollider meshCollider;
    private InteractionRequirement interactionRequirement; // Новый компонент для проверки условий

    void Start()
    {
        mainInteractableObject = GetComponent<MainInteractableObject>();
        meshCollider = GetComponent<MeshCollider>();
        interactionRequirement = GetComponent<InteractionRequirement>();

        // Изначально отключаем коллайдер
        if (meshCollider != null)
        {
            meshCollider.enabled = false;
        }
    }

    void Update()
    {
        // Проверяем, выполнены ли условия для взаимодействия с пирожком
        if (interactionRequirement != null && interactionRequirement.AreRequirementsMet())
        {
            // Если условия выполнены, включаем коллайдер для взаимодействия
            if (meshCollider != null && !meshCollider.enabled)
            {
                meshCollider.enabled = true;
            }
        }
    }

    public void PickUp()
    {
        // Логика для подбора объекта
        gameObject.SetActive(false); // Отключаем объект со сцены

        // Увеличиваем счетчик съеденных пирожков
        piesEaten++;

        // Отправляем сигнал менеджеру о завершении задачи
        mainInteractableObject.Interact();

        // Проверяем, был ли съеден последний пирожок
        if (piesEaten == totalPies)
        {
            Telephone telephone = FindObjectOfType<Telephone>();
            if (telephone != null)
            {
                telephone.StartRinging();
            }
        }
    }
}
