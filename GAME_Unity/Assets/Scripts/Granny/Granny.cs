using UnityEngine;

public class Granny : MonoBehaviour, IInteractable
{
    public AudioClip scareSound; // Звук, который проигрывается при появлении бабушки
    public float moveDistance = 2f; // Расстояние, на которое бабушка сдвинется по оси Z
    public float moveSpeed = 1f; // Скорость движения бабушки
    public InteractableObject door; // Ссылка на скрипт InteractableObject (указывается в инспекторе)

    private DialogueManager dialogueManager;
    private bool hasInteracted = false;
    private AudioSource audioSource;
    private Vector3 targetPosition;
    private Vector3 initialPosition;
    private bool hasAppeared = false;
    private bool isReturning = false;
    private MainInteractableObject mainInteractableObject; // Компонент для отправки сигнала менеджеру

    void Start()
    {
        // Настраиваем аудиосистему
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = scareSound;
        audioSource.playOnAwake = false; // Чтобы звук не проигрывался автоматически

        // Определяем конечную позицию
        targetPosition = transform.position + Vector3.forward * moveDistance;

        // Сохраняем исходную позицию
        initialPosition = transform.position;

        // Получаем компонент MainInteractableObject
        mainInteractableObject = GetComponent<MainInteractableObject>();

        dialogueManager = GetComponentInChildren<DialogueManager>();
        if (dialogueManager != null)
        {
            dialogueManager.enabled = false;
            dialogueManager.OnDialogueEnd += OnDialogueEnd; // Подписываемся на событие завершения диалога
        }
    }

    void OnEnable()
    {
        // Проигрываем звук при активации объекта
        if (audioSource != null && scareSound != null)
        {
            audioSource.Play();
        }

        // Устанавливаем флаг появления
        hasAppeared = true;

        // Отключаем коллайдер двери
        if (door != null)
        {
            door.DisableCollider();
        }
    }

    void Update()
    {
        if (hasAppeared)
        {
            // Двигаем бабушку к целевой позиции по оси Z
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Останавливаем движение, если достигли целевой позиции
            if (transform.position == targetPosition)
            {
                hasAppeared = false;
            }
        }

        if (isReturning)
        {
            // Двигаем бабушку обратно к исходной позиции
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);

            // Останавливаем движение, если достигли исходной позиции
            if (transform.position == initialPosition)
            {
                Destroy(gameObject); // Уничтожаем бабушку

                // Включаем коллайдер двери
                if (door != null)
                {
                    door.EnableCollider();
                }
            }
        }
    }

    public void Interact()
    {
        if (hasInteracted) return;

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
    }

    private void OnDialogueEnd()
    {
        // Запускаем возвращение бабушки на исходную позицию
        isReturning = true;

        MainInteractableObject mainInteractableObject = GetComponent<MainInteractableObject>();
        if (mainInteractableObject != null)
        {
            mainInteractableObject.Interact();
        }
    }
}
