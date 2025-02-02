using UnityEngine;

public class RadioScreamer : MonoBehaviour
{
    public Camera playerCamera;             // Ссылка на камеру игрока, задается в инспекторе
    public AudioSource screamerSource;      // Источник для звука скримера
    public AudioClip screamerSound;         // Аудиоклип для скримера
    private bool hasTriggered = false;      // Флаг, чтобы скример сработал только один раз
    private bool isScreamerPlaying = false; // Статус проигрывания скримера
    public float maxInteractDistance = 5f;  // Максимальное расстояние для взаимодействия
    public float countdownTime = 5f;       // Время обратного отсчета до отправки сигнала

    private InteractionRequirement interactionRequirement; // Компонент для проверки условий
    private MainInteractableObject mainInteractableObject; // Компонент для отправки сигнала менеджеру
    private Collider scremerCollider; // Коллайдер скримера
    private float countdownTimer;    // Таймер для обратного отсчета

    private void Start()
    {
        // Получаем компонент InteractionRequirement
        interactionRequirement = GetComponent<InteractionRequirement>();

        // Получаем компонент MainInteractableObject
        mainInteractableObject = GetComponent<MainInteractableObject>();

        // Получаем коллайдер скримера
        scremerCollider = GetComponent<Collider>();

        // Изначально отключаем коллайдер
        if (scremerCollider != null)
        {
            scremerCollider.enabled = false;
        }
    }

    private void Update()
    {
        // Проверяем, выполнены ли условия для взаимодействия с скримером
        if (interactionRequirement != null && interactionRequirement.AreRequirementsMet())
        {
            // Если условия выполнены, включаем коллайдер для взаимодействия
            if (scremerCollider != null && !scremerCollider.enabled)
            {
                scremerCollider.enabled = true;
            }
        }

        // Проверяем, идет ли обратный отсчет
        if (countdownTimer > 0)
        {
            countdownTimer -= Time.deltaTime;
            if (countdownTimer <= 0)
            {
                // Отправляем сигнал менеджеру о завершении задачи
                mainInteractableObject.Interact();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.GetComponent<PlayerMovement>() != null)
        {
            PlayScreamer();
            hasTriggered = true;
            if (scremerCollider != null)
            {
                scremerCollider.enabled = false; // Выключаем коллайдер, чтобы скример больше не срабатывал
            }

            // Начинаем обратный отсчет
            countdownTimer = countdownTime;
        }
    }

    private void PlayScreamer()
    {
        if (screamerSource != null && screamerSound != null)
        {
            RadioInteraction.SetScreamerActive(true); // Блокируем взаимодействие с радио
            screamerSource.PlayOneShot(screamerSound); // Воспроизведение звука скримера
            isScreamerPlaying = true;
        }
        else
        {
            Debug.LogWarning("Не назначен источник звука или клип скримера!");
        }
    }

    // Проверка, смотрит ли игрок на радио
    private bool IsLookingAtRadio()
    {
        if (playerCamera == null)
        {
            Debug.LogWarning("Камера игрока не задана! Установите камеру в инспекторе.");
            return false;
        }

        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit, maxInteractDistance))
        {
            return hit.collider.GetComponent<RadioInteraction>() != null;
        }
        return false;
    }
}
