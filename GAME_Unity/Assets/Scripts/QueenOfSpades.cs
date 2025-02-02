using UnityEngine;
using UnityEngine.SceneManagement;

public class QueenOfSpades : MonoBehaviour
{
    public float menuTransitionDelay = 5f; // �������� ����� ��������� �� ����� ����
    private bool isTransitioning = false;
    private bool isScreamerPlaying = false; // ���� ��� �������� ��������������� �������
    private DialogueManager dialogueManager; // ��������� ��� ���������� ��������
    public GameObject dialogueTrigger; // ������ �������� �������
    public AudioClip screamerSound; // ���� ��������
    private AudioSource audioSource; // ��������� ��� ��������������� �����

    private Vector3 savedPlayerPosition; // ����������� ������� ������
    private Quaternion savedPlayerRotation; // ����������� ������� ������

    private void Start()
    {
        dialogueManager = GetComponentInChildren<DialogueManager>();
        if (dialogueManager != null)
        {
            dialogueManager.enabled = false;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = screamerSound;
    }

    void Update()
    {
        // ���������, ������� �� ����� �� ������� ����
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10f))
        {
            if (hit.collider.gameObject == gameObject)
            {
                PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
                PlayerLook playerLook = playerMovement.GetComponentInChildren<PlayerLook>();
                playerMovement.SetInteracting(true);
                playerLook.SetInteracting(true);

                // ��������� ������� ������� � ������� ������
                savedPlayerPosition = playerMovement.transform.position;
                savedPlayerRotation = playerMovement.transform.rotation;

                // ������������ ������ � ��������� ������� ��� �������
                if (dialogueTrigger != null)
                {
                    Vector3 targetPosition = dialogueTrigger.transform.position;
                    Vector3 cameraPosition = Camera.main.transform.position;
                    Vector3 direction = (targetPosition - cameraPosition).normalized;
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    Camera.main.transform.rotation = targetRotation;
                }

                // ��������� ������
                if (dialogueManager != null)
                {
                    dialogueManager.enabled = true;
                    dialogueManager.StartDialogue(playerMovement, playerLook);
                }

                // ������������� ���� ��������, ���� �� ��� �� �������������
                if (audioSource != null && screamerSound != null && !isScreamerPlaying)
                {
                    audioSource.Play();
                    isScreamerPlaying = true;
                }

                // ��������� ������ ��� �������� �� ����� ����
                if (!isTransitioning)
                {
                    isTransitioning = true;
                    Invoke("LoadMenuScene", menuTransitionDelay);
                }
            }
        }
    }

    // ����� ��� �������� ����� ����
    private void LoadMenuScene()
    {
        EnableCursor();
        SceneManager.LoadScene("Main_Menu");
    }

    // ����� ��� ��������� �������
    private void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // ����� ��� ��������� ���������� ��������������� �����
    private void OnAudioFinished()
    {
        isScreamerPlaying = false;
    }
}
