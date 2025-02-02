using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Bed : MonoBehaviour, IInteractable
{
    public AudioClip interactionSound; // ���� ��������������
    public float darknessDuration = 3f; // ������������ �������
    public float soundDelay = 1f; // �������� ����� ������������� �����
    public Image darknessImage; // ����������� ������� �� �������
    public float fadeDuration = 1f; // ������������ �������� ��������� �������
    private AudioSource audioSource;
    private PlayerMovement playerMovement;
    private PlayerLook playerLook;
    private BlockableObject blockableObject;
    private MainInteractableObject mainInteractableObject;
    private AudioSource playerAudioSource;
    private LightController lightController; // ������ �� LightController
    private InteractionRequirement interactionRequirement; // ��������� ��� �������� �������
    private Collider bedCollider; // ��������� �������
    public GameObject radioScreamer; // ������ �����-���������
    public RadioInteraction radio; // ������ �� ������ �����
    public TextMeshProUGUI warningText; // ��������� ���� ��� ������ ��������������
    public GameObject mirrorToEnable; // ������ ������� ��� ���������
    public GameObject mirrorToDisable; // ������ ������� ��� ����������
    public GameObject[] switchesToEnable; // ����������� ��� ���������
    public GameObject[] switchesToDisable; // ����������� ��� ����������

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
            darknessImage.enabled = false; // ���������� ��������� ����������� �������
            darknessImage.color = new Color(darknessImage.color.r, darknessImage.color.g, darknessImage.color.b, 0f); // ������������� ������������ � 0
        }

        // ������ AudioSource �� ������� ������
        playerAudioSource = playerMovement.GetComponent<AudioSource>();

        // ������ LightController �� �����
        lightController = FindObjectOfType<LightController>();

        // �������� ��������� InteractionRequirement
        interactionRequirement = GetComponent<InteractionRequirement>();

        // �������� ��������� �������
        bedCollider = GetComponent<Collider>();

        // ���������� ��������� ���������
        if (bedCollider != null)
        {
            bedCollider.enabled = false;
        }

        // ���������� ��������� �����-��������
        if (radioScreamer != null)
        {
            radioScreamer.SetActive(false);
        }

        // ���������� ��������� ��������������
        if (warningText != null)
        {
            warningText.enabled = false;
        }

        // ���������� ��������� ������� ��� ���������
        if (mirrorToEnable != null)
        {
            mirrorToEnable.SetActive(false);
        }

        // ���������� �������� ������� ��� ����������
        if (mirrorToDisable != null)
        {
            mirrorToDisable.SetActive(true);
        }

        // ���������� ��������� ����������� ��� ���������
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

        // ���������� �������� ����������� ��� ����������
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
        // ���������, ��������� �� ������� ��� �������������� � ��������
        if (interactionRequirement != null && interactionRequirement.AreRequirementsMet())
        {
            // ���� ������� ���������, �������� ��������� ��� ��������������
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
                // ������� ��������� �� �����
                if (warningText != null)
                {
                    warningText.text = "����� ���, ��� ���� �����, ����� ��������� �����.";
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
        // �������� ��������� ��������� ����� ����
        playerLook.SetInteracting(true);

        // ��������� ���������� �������
        playerMovement.SetInteracting(true);
        playerLook.SetInteracting(true);

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

        // ���� ��������� ����� ����� ������������� �����
        yield return new WaitForSeconds(soundDelay);

        // ����������� ���� ��������������
        audioSource.Play();

        // ��������� ����
        if (lightController != null)
        {
            lightController.TurnOffLights();
        }

        // ���� ��������� �����
        yield return new WaitForSeconds(darknessDuration - soundDelay);

        // ����� ��������� �������
        if (darknessImage != null)
        {
            darknessImage.enabled = false;
        }

        // ���������� ���������� �������
        playerMovement.SetInteracting(false);
        playerLook.SetInteracting(false);

        // �������� AudioSource ������
        if (playerAudioSource != null)
        {
            playerAudioSource.enabled = true;
        }

        // �������� �������� ��������� ����� ����
        playerLook.SetInteracting(false);

        // ��������� ������
        blockableObject.Block();

        // �������� �����-��������
        if (radioScreamer != null)
        {
            radioScreamer.SetActive(true);
        }

        // �������� ������� ��� ���������
        if (mirrorToEnable != null)
        {
            mirrorToEnable.SetActive(true);
        }

        // ��������� ������� ��� ����������
        if (mirrorToDisable != null)
        {
            mirrorToDisable.SetActive(false);
        }

        // �������� ����������� ��� ���������
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

        // ��������� ����������� ��� ����������
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

        // ���������� ������ ��������� � ���������� ������
        mainInteractableObject.Interact();

        // ��������� ��������� ����� ��������������
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
        yield return new WaitForSeconds(3f); // ���������� ��������� �� 3 �������
        warningText.enabled = false;
    }
}
