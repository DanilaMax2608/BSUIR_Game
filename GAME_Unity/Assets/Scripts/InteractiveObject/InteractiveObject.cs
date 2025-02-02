using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InteractiveObject : MonoBehaviour, IInteractable
{
    public AudioClip interactionSound; // ���� ��������������
    public float darknessDuration = 3f; // ������������ �������
    public Image darknessImage; // ����������� ������� �� �������
    public float fadeDuration = 1f; // ������������ �������� ��������� � ������������ �������
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
            darknessImage.enabled = false; // ���������� ��������� ����������� �������
            darknessImage.color = new Color(darknessImage.color.r, darknessImage.color.g, darknessImage.color.b, 0f); // ������������� ������������ � 0
        }

        // ������ AudioSource �� ������� ������
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
        // �������� ��������� ��������� ����� ����
        playerLook.SetInteracting(true);

        // ��������� ���������� �������
        playerMovement.SetInteracting(true);
        playerLook.SetInteracting(true);
        playerInteraction.enabled = false;

        // ��������� AudioSource ������
        if (playerAudioSource != null)
        {
            playerAudioSource.enabled = false;
        }

        // �������� �������
        if (darknessImage != null)
        {
            darknessImage.enabled = true;
            yield return StartCoroutine(FadeImage(darknessImage, 1f, fadeDuration)); // ������� ��������� �������
        }

        // ����������� ���� ��������������
        audioSource.Play();

        // ���� ��������� �����
        yield return new WaitForSeconds(darknessDuration);

        // ��������� �������
        if (darknessImage != null)
        {
            yield return StartCoroutine(FadeImage(darknessImage, 0f, fadeDuration)); // ������� ������������ �������
            darknessImage.enabled = false;
        }

        // ���������� ���������� �������
        playerMovement.SetInteracting(false);
        playerLook.SetInteracting(false);
        playerInteraction.enabled = true;

        // �������� AudioSource ������
        if (playerAudioSource != null)
        {
            playerAudioSource.enabled = true;
        }

        // �������� �������� ��������� ����� ����
        playerLook.SetInteracting(false);

        // ��������� ������
        blockableObject.Block();

        // ���������� ������ ��������� � ���������� ������
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
