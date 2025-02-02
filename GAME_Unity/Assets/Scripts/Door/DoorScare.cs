using UnityEngine;
using System.Collections;

public class DoorScare : MonoBehaviour
{
    public AudioClip scareSound; // ���� ��� ������������
    public GameObject grannyObject; // ������ �������
    public float grannyDelay = 1.5f; // �������� ����� ���������� ������� (� ��������)
    public GameObject playerObject; // ������ ������
    [SerializeField] private AudioSource scareAudioSource; // ��������� ��� ����� ������ � �����

    private bool isScareActive = false;
    private InteractableObject door;
    private MainInteractableObject mainInteractableObject;
    private bool hasBeenOpened = false; // ���� ��� ������������, ���� �� ����� �������
    private bool hasSentSignal = false; // ���� ��� ������������, ��� �� ��������� ������ ���������
    private InteractionRequirement interactionRequirement; // ����� ��������� ��� �������� �������

    private PlayerMovement playerMovement;
    private PlayerLook playerLook;
    private PlayerInteraction playerInteraction;

    void Start()
    {
        scareAudioSource.clip = scareSound;
        scareAudioSource.loop = true;
        scareAudioSource.enabled = false; // ���������� ��������� ����

        door = GetComponent<InteractableObject>();
        mainInteractableObject = GetComponent<MainInteractableObject>();
        interactionRequirement = GetComponent<InteractionRequirement>();

        if (grannyObject != null)
        {
            grannyObject.SetActive(false); // ���������� ������� ���������
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
            mainInteractableObject.Interact(); // ���������� ������ ��������� � ���������� ������
            hasBeenOpened = true; // ������������� ����, ��� ����� ���� �������
            hasSentSignal = true; // ������������� ����, ��� ������ ��� ���������

            // ���������� ������� ����� �������� � �������� ��������� ���������� ������
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
            grannyObject.SetActive(true); // ����� �������� �������
        }

        // ��������� ��������� AudioSource
        if (scareAudioSource != null)
        {
            scareAudioSource.enabled = false;
        }
    }
}
