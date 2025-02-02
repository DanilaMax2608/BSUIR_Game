using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Bed : MonoBehaviour, IInteractable
{
    public AudioClip interactionSound; // Звук взаимодействия
    public float darknessDuration = 3f; // Длительность темноты
    public float soundDelay = 1f; // Задержка перед проигрыванием звука
    public Image darknessImage; // Изображение темноты на канвасе
    public float fadeDuration = 1f; // Длительность плавного появления темноты
    private AudioSource audioSource;
    private PlayerMovement playerMovement;
    private PlayerLook playerLook;
    private BlockableObject blockableObject;
    private MainInteractableObject mainInteractableObject;
    private AudioSource playerAudioSource;
    private LightController lightController; // Ссылка на LightController
    private InteractionRequirement interactionRequirement; // Компонент для проверки условий
    private Collider bedCollider; // Коллайдер кровати
    public GameObject radioScreamer; // Объект радио-скриммера
    public RadioInteraction radio; // Ссылка на объект радио
    public TextMeshProUGUI warningText; // Текстовое поле для вывода предупреждения
    public GameObject mirrorToEnable; // Объект зеркала для включения
    public GameObject mirrorToDisable; // Объект зеркала для выключения
    public GameObject[] switchesToEnable; // Выключатели для включения
    public GameObject[] switchesToDisable; // Выключатели для выключения

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = interactionSound;
        audioSource.loop = false;

        playerMovement = FindObjectOfType<PlayerMovement>();
        playerLook = FindObjectOfType<PlayerLook>();
        blockableObject = GetComponent<BlockableObject>();
        mainInteractableObject = GetComponent<MainInteractableObject>();

        if (darknessImage != null)
        {
            darknessImage.enabled = false; // Изначально выключаем изображение темноты
            darknessImage.color = new Color(darknessImage.color.r, darknessImage.color.g, darknessImage.color.b, 0f); // Устанавливаем прозрачность в 0
        }

        // Найдем AudioSource на объекте игрока
        playerAudioSource = playerMovement.GetComponent<AudioSource>();

        // Найдем LightController на сцене
        lightController = FindObjectOfType<LightController>();

        // Получаем компонент InteractionRequirement
        interactionRequirement = GetComponent<InteractionRequirement>();

        // Получаем коллайдер кровати
        bedCollider = GetComponent<Collider>();

        // Изначально отключаем коллайдер
        if (bedCollider != null)
        {
            bedCollider.enabled = false;
        }

        // Изначально отключаем радио-скриммер
        if (radioScreamer != null)
        {
            radioScreamer.SetActive(false);
        }

        // Изначально отключаем предупреждение
        if (warningText != null)
        {
            warningText.enabled = false;
        }

        // Изначально отключаем зеркало для включения
        if (mirrorToEnable != null)
        {
            mirrorToEnable.SetActive(false);
        }

        // Изначально включаем зеркало для выключения
        if (mirrorToDisable != null)
        {
            mirrorToDisable.SetActive(true);
        }

        // Изначально отключаем выключатели для включения
        if (switchesToEnable != null)
        {
            foreach (var switchObj in switchesToEnable)
            {
                if (switchObj != null)
                {
                    switchObj.SetActive(false);
                }
            }
        }

        // Изначально включаем выключатели для выключения
        if (switchesToDisable != null)
        {
            foreach (var switchObj in switchesToDisable)
            {
                if (switchObj != null)
                {
                    switchObj.SetActive(true);
                }
            }
        }
    }

    void Update()
    {
        // Проверяем, выполнены ли условия для взаимодействия с кроватью
        if (interactionRequirement != null && interactionRequirement.AreRequirementsMet())
        {
            // Если условия выполнены, включаем коллайдер для взаимодействия
            if (bedCollider != null && !bedCollider.enabled)
            {
                bedCollider.enabled = true;
            }
        }
    }

    public void Interact()
    {
        if (!blockableObject.IsBlocked)
        {
            if (radio != null && radio.isPlaying)
            {
                // Выводим сообщение на экран
                if (warningText != null)
                {
                    warningText.text = "Перед тем, как лечь спать, нужно выключить радио.";
                    warningText.enabled = true;
                    StartCoroutine(HideWarningText());
                }
                return;
            }

            StartCoroutine(ObjectInteraction());
        }
    }

    private IEnumerator ObjectInteraction()
    {
        // Временно отключаем обработку ввода мыши
        playerLook.SetInteracting(true);

        // Отключаем управление игроком
        playerMovement.SetInteracting(true);
        playerLook.SetInteracting(true);

        // Отключаем AudioSource игрока
        if (playerAudioSource != null)
        {
            playerAudioSource.enabled = false;
        }

        // Включаем темноту
        if (darknessImage != null)
        {
            darknessImage.enabled = true;
            yield return StartCoroutine(FadeImage(darknessImage, 1f, fadeDuration)); // Плавное появление темноты
        }

        // Ждем указанное время перед проигрыванием звука
        yield return new WaitForSeconds(soundDelay);

        // Проигрываем звук взаимодействия
        audioSource.Play();

        // Выключаем свет
        if (lightController != null)
        {
            lightController.TurnOffLights();
        }

        // Ждем указанное время
        yield return new WaitForSeconds(darknessDuration - soundDelay);

        // Резко выключаем темноту
        if (darknessImage != null)
        {
            darknessImage.enabled = false;
        }

        // Возвращаем управление игроком
        playerMovement.SetInteracting(false);
        playerLook.SetInteracting(false);

        // Включаем AudioSource игрока
        if (playerAudioSource != null)
        {
            playerAudioSource.enabled = true;
        }

        // Временно включаем обработку ввода мыши
        playerLook.SetInteracting(false);

        // Блокируем объект
        blockableObject.Block();

        // Включаем радио-скриммер
        if (radioScreamer != null)
        {
            radioScreamer.SetActive(true);
        }

        // Включаем зеркало для включения
        if (mirrorToEnable != null)
        {
            mirrorToEnable.SetActive(true);
        }

        // Выключаем зеркало для выключения
        if (mirrorToDisable != null)
        {
            mirrorToDisable.SetActive(false);
        }

        // Включаем выключатели для включения
        if (switchesToEnable != null)
        {
            foreach (var switchObj in switchesToEnable)
            {
                if (switchObj != null)
                {
                    switchObj.SetActive(true);
                }
            }
        }

        // Выключаем выключатели для выключения
        if (switchesToDisable != null)
        {
            foreach (var switchObj in switchesToDisable)
            {
                if (switchObj != null)
                {
                    switchObj.SetActive(false);
                }
            }
        }

        // Отправляем сигнал менеджеру о завершении задачи
        mainInteractableObject.Interact();

        // Отключаем коллайдер после взаимодействия
        if (bedCollider != null)
        {
            bedCollider.enabled = false;
        }
    }

    private IEnumerator FadeImage(Image image, float targetAlpha, float duration)
    {
        float startAlpha = image.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            image.color = new Color(image.color.r, image.color.g, image.color.b, newAlpha);
            yield return null;
        }

        image.color = new Color(image.color.r, image.color.g, image.color.b, targetAlpha);
    }

    private IEnumerator HideWarningText()
    {
        yield return new WaitForSeconds(3f); // Показываем сообщение на 3 секунды
        warningText.enabled = false;
    }
}
