using UnityEngine;
using System.Collections;

public class DoorScare : MonoBehaviour
{
    public AudioClip scareSound; // Звук для проигрывания
    public GameObject grannyObject; // Объект бабушки
    public float grannyDelay = 1.5f; // Задержка перед появлением бабушки (в секундах)
    public GameObject playerObject; // Объект игрока
    [SerializeField] private AudioSource scareAudioSource; // Компонент для звука звонка в дверь

    private bool isScareActive = false;
    private InteractableObject door;
    private MainInteractableObject mainInteractableObject;
    private bool hasBeenOpened = false; // Флаг для отслеживания, была ли дверь открыта
    private bool hasSentSignal = false; // Флаг для отслеживания, был ли отправлен сигнал менеджеру
    private InteractionRequirement interactionRequirement; // Новый компонент для проверки условий

    private PlayerMovement playerMovement;
    private PlayerLook playerLook;
    private PlayerInteraction playerInteraction;

    void Start()
    {
        scareAudioSource.clip = scareSound;
        scareAudioSource.loop = true;
        scareAudioSource.enabled = false; // Изначально выключаем звук

        door = GetComponent<InteractableObject>();
        mainInteractableObject = GetComponent<MainInteractableObject>();
        interactionRequirement = GetComponent<InteractionRequirement>();

        if (grannyObject != null)
        {
            grannyObject.SetActive(false); // Изначально бабушка неактивна
        }

        if (playerObject != null)
        {
            playerMovement = playerObject.GetComponent<PlayerMovement>();
            playerLook = playerObject.GetComponent<PlayerLook>();
            playerInteraction = playerObject.GetComponent<PlayerInteraction>();
        }
    }

    void Update()
    {
        if (!isScareActive && interactionRequirement != null && interactionRequirement.AreRequirementsMet())
        {
            StartScare();
        }

        if (isScareActive && door != null && door.isOpen && !hasSentSignal)
        {
            StopScare();
            mainInteractableObject.Interact(); // Отправляем сигнал менеджеру о завершении задачи
            hasBeenOpened = true; // Устанавливаем флаг, что дверь была открыта
            hasSentSignal = true; // Устанавливаем флаг, что сигнал был отправлен

            // Активируем бабушку после задержки и временно отключаем управление игрока
            StartCoroutine(ActivateGrannyWithDelay());
        }
    }

    public void StartScare()
    {
        if (!isScareActive)
        {
            isScareActive = true;
            scareAudioSource.enabled = true;
            scareAudioSource.Play();
        }
    }

    public void StopScare()
    {
        if (isScareActive)
        {
            isScareActive = false;
            scareAudioSource.Stop();
            scareAudioSource.enabled = false;
        }
    }

    private IEnumerator ActivateGrannyWithDelay()
    {
        yield return new WaitForSeconds(grannyDelay);

        if (grannyObject != null)
        {
            grannyObject.SetActive(true); // Резко появляем бабушку
        }

        // Отключаем компонент AudioSource
        if (scareAudioSource != null)
        {
            scareAudioSource.enabled = false;
        }
    }
}
