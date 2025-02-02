using UnityEngine;

public class Telephone : MonoBehaviour, IInteractable
{
    public AudioClip ringtone;
    private AudioSource audioSource;
    private bool isRinging = false;
    private bool hasInteracted = false;
    private DialogueManager dialogueManager;
    private InteractionRequirement interactionRequirement; // ����� ��������� ��� �������� �������
    private Collider telephoneCollider; // ��������� ��������

    private Coroutine typingCoroutine;
    private Coroutine displayCoroutine;

    private TelephoneSwitcher telephoneSwitcher; // ������ �� ������ TelephoneSwitcher

    void Start()
    {
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        audioSource.clip = ringtone;
        audioSource.loop = true;
        audioSource.enabled = false;

        dialogueManager = GetComponentInChildren<DialogueManager>();
        if (dialogueManager != null)
        {
            dialogueManager.enabled = false;
            dialogueManager.OnDialogueEnd += OnDialogueEnd; // ������������� �� ������� ���������� �������
        }

        interactionRequirement = GetComponent<InteractionRequirement>();
        telephoneCollider = GetComponent<Collider>(); // �������� ��������� ��������

        // ���������� ��������� ���������
        if (telephoneCollider != null)
        {
            telephoneCollider.enabled = false;
        }

        // �������� ������ �� ������ TelephoneSwitcher, ���� �� ����
        telephoneSwitcher = GetComponent<TelephoneSwitcher>();
    }

    void Update()
    {
        // ���������, ��������� �� ������� ��� �������������� � ���������
        if (interactionRequirement != null && interactionRequirement.AreRequirementsMet())
        {
            // ���� ������� ���������, �������� ��������� ��� ��������������
            if (telephoneCollider != null && !telephoneCollider.enabled)
            {
                telephoneCollider.enabled = true;
            }
        }
    }

    public void StartRinging()
    {
        if (!isRinging)
        {
            isRinging = true;
            audioSource.enabled = true;
            audioSource.Play();
        }
    }

    public void Interact()
    {
        if (hasInteracted) return;

        // ��������� ���������� ������� ����� ���������������
        if (interactionRequirement != null && !interactionRequirement.AreRequirementsMet())
        {
            return;
        }

        hasInteracted = true;

        if (isRinging)
        {
            audioSource.Stop();
            audioSource.enabled = false;
            isRinging = false;
        }

        if (dialogueManager != null)
        {
            PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
            PlayerLook playerLook = playerMovement.GetComponentInChildren<PlayerLook>();
            playerMovement.SetInteracting(true);
            playerLook.SetInteracting(true);
            dialogueManager.enabled = true;
            dialogueManager.StartDialogue(playerMovement, playerLook);
        }
        // ��������� ��������� ����� ��������������
        if (telephoneCollider != null)
        {
            telephoneCollider.enabled = false;
        }
    }

    private void OnDialogueEnd()
    {
        // ����������� ��������, ���� ���� ������ TelephoneSwitcher
        if (telephoneSwitcher != null)
        {
            telephoneSwitcher.SwitchTelephones();
        }

        // ��������� ��������� ����� ���������� �������
        if (telephoneCollider != null)
        {
            telephoneCollider.enabled = false;
        }

        MainInteractableObject mainInteractableObject = GetComponent<MainInteractableObject>();
        if (mainInteractableObject != null)
        {
            mainInteractableObject.Interact();
        }
    }
}
