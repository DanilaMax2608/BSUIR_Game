using UnityEngine;

public class RadioScreamer : MonoBehaviour
{
    public Camera playerCamera;             // ������ �� ������ ������, �������� � ����������
    public AudioSource screamerSource;      // �������� ��� ����� ��������
    public AudioClip screamerSound;         // ��������� ��� ��������
    private bool hasTriggered = false;      // ����, ����� ������� �������� ������ ���� ���
    private bool isScreamerPlaying = false; // ������ ������������ ��������
    public float maxInteractDistance = 5f;  // ������������ ���������� ��� ��������������
    public float countdownTime = 5f;       // ����� ��������� ������� �� �������� �������

    private InteractionRequirement interactionRequirement; // ��������� ��� �������� �������
    private MainInteractableObject mainInteractableObject; // ��������� ��� �������� ������� ���������
    private Collider scremerCollider; // ��������� ��������
    private float countdownTimer;    // ������ ��� ��������� �������

    private void Start()
    {
        // �������� ��������� InteractionRequirement
        interactionRequirement = GetComponent<InteractionRequirement>();

        // �������� ��������� MainInteractableObject
        mainInteractableObject = GetComponent<MainInteractableObject>();

        // �������� ��������� ��������
        scremerCollider = GetComponent<Collider>();

        // ���������� ��������� ���������
        if (scremerCollider != null)
        {
            scremerCollider.enabled = false;
        }
    }

    private void Update()
    {
        // ���������, ��������� �� ������� ��� �������������� � ���������
        if (interactionRequirement != null && interactionRequirement.AreRequirementsMet())
        {
            // ���� ������� ���������, �������� ��������� ��� ��������������
            if (scremerCollider != null && !scremerCollider.enabled)
            {
                scremerCollider.enabled = true;
            }
        }

        // ���������, ���� �� �������� ������
        if (countdownTimer > 0)
        {
            countdownTimer -= Time.deltaTime;
            if (countdownTimer <= 0)
            {
                // ���������� ������ ��������� � ���������� ������
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
                scremerCollider.enabled = false; // ��������� ���������, ����� ������� ������ �� ����������
            }

            // �������� �������� ������
            countdownTimer = countdownTime;
        }
    }

    private void PlayScreamer()
    {
        if (screamerSource != null && screamerSound != null)
        {
            RadioInteraction.SetScreamerActive(true); // ��������� �������������� � �����
            screamerSource.PlayOneShot(screamerSound); // ��������������� ����� ��������
            isScreamerPlaying = true;
        }
        else
        {
            Debug.LogWarning("�� �������� �������� ����� ��� ���� ��������!");
        }
    }

    // ��������, ������� �� ����� �� �����
    private bool IsLookingAtRadio()
    {
        if (playerCamera == null)
        {
            Debug.LogWarning("������ ������ �� ������! ���������� ������ � ����������.");
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
