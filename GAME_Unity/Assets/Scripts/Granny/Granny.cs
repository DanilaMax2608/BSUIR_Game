using UnityEngine;

public class Granny : MonoBehaviour, IInteractable
{
    public AudioClip scareSound; // ����, ������� ������������� ��� ��������� �������
    public float moveDistance = 2f; // ����������, �� ������� ������� ��������� �� ��� Z
    public float moveSpeed = 1f; // �������� �������� �������
    public InteractableObject door; // ������ �� ������ InteractableObject (����������� � ����������)

    private DialogueManager dialogueManager;
    private bool hasInteracted = false;
    private AudioSource audioSource;
    private Vector3 targetPosition;
    private Vector3 initialPosition;
    private bool hasAppeared = false;
    private bool isReturning = false;
    private MainInteractableObject mainInteractableObject; // ��������� ��� �������� ������� ���������

    void Start()
    {
        // ����������� ������������
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = scareSound;
        audioSource.playOnAwake = false; // ����� ���� �� ������������ �������������

        // ���������� �������� �������
        targetPosition = transform.position + Vector3.forward * moveDistance;

        // ��������� �������� �������
        initialPosition = transform.position;

        // �������� ��������� MainInteractableObject
        mainInteractableObject = GetComponent<MainInteractableObject>();

        dialogueManager = GetComponentInChildren<DialogueManager>();
        if (dialogueManager != null)
        {
            dialogueManager.enabled = false;
            dialogueManager.OnDialogueEnd += OnDialogueEnd; // ������������� �� ������� ���������� �������
        }
    }

    void OnEnable()
    {
        // ����������� ���� ��� ��������� �������
        if (audioSource != null && scareSound != null)
        {
            audioSource.Play();
        }

        // ������������� ���� ���������
        hasAppeared = true;

        // ��������� ��������� �����
        if (door != null)
        {
            door.DisableCollider();
        }
    }

    void Update()
    {
        if (hasAppeared)
        {
            // ������� ������� � ������� ������� �� ��� Z
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // ������������� ��������, ���� �������� ������� �������
            if (transform.position == targetPosition)
            {
                hasAppeared = false;
            }
        }

        if (isReturning)
        {
            // ������� ������� ������� � �������� �������
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);

            // ������������� ��������, ���� �������� �������� �������
            if (transform.position == initialPosition)
            {
                Destroy(gameObject); // ���������� �������

                // �������� ��������� �����
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
        // ��������� ����������� ������� �� �������� �������
        isReturning = true;

        MainInteractableObject mainInteractableObject = GetComponent<MainInteractableObject>();
        if (mainInteractableObject != null)
        {
            mainInteractableObject.Interact();
        }
    }
}
