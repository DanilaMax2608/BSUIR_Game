using UnityEngine;

public class Mirror : MonoBehaviour, IInteractable
{
    [SerializeField] private string pomadeName = "Pomade"; // ��� ������� ������
    [SerializeField] private GameObject drawing; // ������, ������� ����� ������������ �� �������
    [SerializeField] private AudioClip newBackgroundMusic; // ����� ������� ������

    private bool isDrawingShown = false;
    private MainInteractableObject mainInteractableObject;
    private InteractionRequirement interactionRequirement;
    private Bag playerBag;
    private Collider mirrorCollider; // ��������� �������

    void Start()
    {
        mainInteractableObject = GetComponent<MainInteractableObject>();
        interactionRequirement = GetComponent<InteractionRequirement>();
        playerBag = FindObjectOfType<Bag>();
        mirrorCollider = GetComponent<Collider>(); // �������� ��������� �������

        // ���������� ��������� ���������
        if (mirrorCollider != null)
        {
            mirrorCollider.enabled = false;
        }
    }

    void Update()
    {
        // ��������� ���������� ������� ����� ���������������
        if (interactionRequirement != null && interactionRequirement.AreRequirementsMet())
        {
            // �������� ���������, ���� ������� ���������
            if (mirrorCollider != null && !mirrorCollider.enabled)
            {
                mirrorCollider.enabled = true;
            }
        }
    }

    public void Interact()
    {
        if (isDrawingShown) return;

        // �������� �� ������� ������ � ���������� ����������� �������
        GameObject pomade = playerBag.GetObject(pomadeName);
        if (pomade != null && (interactionRequirement == null || interactionRequirement.AreRequirementsMet()))
        {
            ShowDrawing();
        }
    }

    private void ShowDrawing()
    {
        drawing.SetActive(true);
        isDrawingShown = true;

        // ��������� Collider � �������
        if (mirrorCollider != null)
        {
            mirrorCollider.enabled = false;
        }

        // ���������� ������ ��������� � ���������� ������
        mainInteractableObject?.Interact();

        // �������� ������� ������ � ����������� ���������
        BackgroundMusic backgroundMusic = FindObjectOfType<BackgroundMusic>();
        if (backgroundMusic != null && newBackgroundMusic != null)
        {
            backgroundMusic.ChangeBackgroundMusic(newBackgroundMusic, 0.25f);
        }
    }
}
