using UnityEngine;

public class MirrorScreamer : MonoBehaviour, IInteractable
{
    private DialogueManager dialogueManager;
    private bool hasInteracted = false;
    private InteractionRequirement interactionRequirement; // Компонент для проверки условий
    private MainInteractableObject mainInteractableObject; // Компонент для отправки сигнала менеджеру
    private Collider mirrorCollider; // Коллайдер зеркала
    public GameObject queenOfSpades; // Объект Пиковой Дамы

    void Start()
    {
        // Получаем компонент InteractionRequirement
        interactionRequirement = GetComponent<InteractionRequirement>();

        // Получаем компонент MainInteractableObject
        mainInteractableObject = GetComponent<MainInteractableObject>();

        dialogueManager = GetComponentInChildren<DialogueManager>();
        if (dialogueManager != null)
        {
            dialogueManager.enabled = false;
            dialogueManager.OnDialogueEnd += OnDialogueEnd; // Подписываемся на событие завершения диалога
        }

        mirrorCollider = GetComponent<Collider>(); // Получаем коллайдер зеркала

        // Изначально отключаем коллайдер
        if (mirrorCollider != null)
        {
            mirrorCollider.enabled = false;
        }

        // Изначально отключаем Пиковую Даму
        if (queenOfSpades != null)
        {
            queenOfSpades.SetActive(false);
        }
    }

    void Update()
    {
        // Проверяем, выполнены ли условия для взаимодействия с зеркалом
        if (interactionRequirement != null && interactionRequirement.AreRequirementsMet())
        {
            // Если условия выполнены, включаем коллайдер для взаимодействия
            if (mirrorCollider != null && !mirrorCollider.enabled)
            {
                mirrorCollider.enabled = true;
            }
        }
    }

    public void Interact()
    {
        if (hasInteracted) return;

        // Проверяем выполнение условий перед взаимодействием
        if (interactionRequirement != null && !interactionRequirement.AreRequirementsMet())
        {
            return;
        }

        hasInteracted = true;

        if (dialogueManager != null)
        {
            PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
            PlayerLook playerLook = playerMovement.GetComponentInChildren<PlayerLook>();
            playerMovement.SetInteracting(true);
            playerLook.SetInteracting(true);
            dialogueManager.enabled = true;
            dialogueManager.StartDialogue(playerMovement, playerLook);
        }
        // Сохраняем и блокируем текущие координаты игрока
        PlayerMovement playerMovementComponent = FindObjectOfType<PlayerMovement>();
        if (playerMovementComponent != null)
        {
            Vector3 playerPosition = playerMovementComponent.transform.position;
            Quaternion playerRotation = playerMovementComponent.transform.rotation;
        }

        // Включаем Пиковую Даму
        if (queenOfSpades != null)
        {
            queenOfSpades.SetActive(true);
            PlayerLook playerLook = FindObjectOfType<PlayerLook>();
            if (playerLook != null)
            {
                playerLook.SetQueenOfSpades(queenOfSpades.transform);
            }
        }

        // Отключаем коллайдер после взаимодействия
        if (mirrorCollider != null)
        {
            mirrorCollider.enabled = false;
        }
    }

    private void OnDialogueEnd()
    {
        // Отправляем сигнал менеджеру о завершении задачи
        if (mainInteractableObject != null)
        {
            mainInteractableObject.Interact();
        }
    }
}
