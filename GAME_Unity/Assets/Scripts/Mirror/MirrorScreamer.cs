using UnityEngine;

public class MirrorScreamer : MonoBehaviour, IInteractable
{
    private DialogueManager dialogueManager;
    private bool hasInteracted = false;
    private InteractionRequirement interactionRequirement; // ��������� ��� �������� �������
    private MainInteractableObject mainInteractableObject; // ��������� ��� �������� ������� ���������
    private Collider mirrorCollider; // ��������� �������
    public GameObject queenOfSpades; // ������ ������� ����

    void Start()
    {
        // �������� ��������� InteractionRequirement
        interactionRequirement = GetComponent<InteractionRequirement>();

        // �������� ��������� MainInteractableObject
        mainInteractableObject = GetComponent<MainInteractableObject>();

        dialogueManager = GetComponentInChildren<DialogueManager>();
        if (dialogueManager != null)
        {
            dialogueManager.enabled = false;
            dialogueManager.OnDialogueEnd += OnDialogueEnd; // ������������� �� ������� ���������� �������
        }

        mirrorCollider = GetComponent<Collider>(); // �������� ��������� �������

        // ���������� ��������� ���������
        if (mirrorCollider != null)
        {
            mirrorCollider.enabled = false;
        }

        // ���������� ��������� ������� ����
        if (queenOfSpades != null)
        {
            queenOfSpades.SetActive(false);
        }
    }

    void Update()
    {
        // ���������, ��������� �� ������� ��� �������������� � ��������
        if (interactionRequirement != null && interactionRequirement.AreRequirementsMet())
        {
            // ���� ������� ���������, �������� ��������� ��� ��������������
            if (mirrorCollider != null && !mirrorCollider.enabled)
            {
                mirrorCollider.enabled = true;
            }
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

        if (dialogueManager != null)
        {
            PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
            PlayerLook playerLook = playerMovement.GetComponentInChildren<PlayerLook>();
            playerMovement.SetInteracting(true);
            playerLook.SetInteracting(true);
            dialogueManager.enabled = true;
            dialogueManager.StartDialogue(playerMovement, playerLook);
        }
        // ��������� � ��������� ������� ���������� ������
        PlayerMovement playerMovementComponent = FindObjectOfType<PlayerMovement>();
        if (playerMovementComponent != null)
        {
            Vector3 playerPosition = playerMovementComponent.transform.position;
            Quaternion playerRotation = playerMovementComponent.transform.rotation;
        }

        // �������� ������� ����
        if (queenOfSpades != null)
        {
            queenOfSpades.SetActive(true);
            PlayerLook playerLook = FindObjectOfType<PlayerLook>();
            if (playerLook != null)
            {
                playerLook.SetQueenOfSpades(queenOfSpades.transform);
            }
        }

        // ��������� ��������� ����� ��������������
        if (mirrorCollider != null)
        {
            mirrorCollider.enabled = false;
        }
    }

    private void OnDialogueEnd()
    {
        // ���������� ������ ��������� � ���������� ������
        if (mainInteractableObject != null)
        {
            mainInteractableObject.Interact();
        }
    }
}
