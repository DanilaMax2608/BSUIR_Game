using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InteractiveObject : MonoBehaviour, IInteractable
{
    public AudioClip interactionSound; // Звук взаимодействия
    public float darknessDuration = 3f; // Длительность темноты
    public Image darknessImage; // Изображение темноты на канвасе
    public float fadeDuration = 1f; // Длительность плавного появления и исчезновения темноты
    private AudioSource audioSource;
    private PlayerMovement playerMovement;
    private PlayerLook playerLook;
    private PlayerInteraction playerInteraction;
    private BlockableObject blockableObject;
    private MainInteractableObject mainInteractableObject;
    private AudioSource playerAudioSource;

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
        playerInteraction = FindObjectOfType<PlayerInteraction>();
        blockableObject = GetComponent<BlockableObject>();
        mainInteractableObject = GetComponent<MainInteractableObject>();

        if (darknessImage != null)
        {
            darknessImage.enabled = false; // Изначально выключаем изображение темноты
            darknessImage.color = new Color(darknessImage.color.r, darknessImage.color.g, darknessImage.color.b, 0f); // Устанавливаем прозрачность в 0
        }

        // Найдем AudioSource на объекте игрока
        playerAudioSource = playerMovement.GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (!blockableObject.IsBlocked)
        {
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
        playerInteraction.enabled = false;

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

        // Проигрываем звук взаимодействия
        audioSource.Play();

        // Ждем указанное время
        yield return new WaitForSeconds(darknessDuration);

        // Выключаем темноту
        if (darknessImage != null)
        {
            yield return StartCoroutine(FadeImage(darknessImage, 0f, fadeDuration)); // Плавное исчезновение темноты
            darknessImage.enabled = false;
        }

        // Возвращаем управление игроком
        playerMovement.SetInteracting(false);
        playerLook.SetInteracting(false);
        playerInteraction.enabled = true;

        // Включаем AudioSource игрока
        if (playerAudioSource != null)
        {
            playerAudioSource.enabled = true;
        }

        // Временно включаем обработку ввода мыши
        playerLook.SetInteracting(false);

        // Блокируем объект
        blockableObject.Block();

        // Отправляем сигнал менеджеру о завершении задачи
        mainInteractableObject.Interact();
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
}
