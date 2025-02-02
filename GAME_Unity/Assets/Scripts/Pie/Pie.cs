using UnityEngine;

public class Pie : MonoBehaviour, IPickable
{
    public static int piesEaten = 0;
    public static int totalPies = 4;

    private MainInteractableObject mainInteractableObject;
    private MeshCollider meshCollider;
    private InteractionRequirement interactionRequirement; // ����� ��������� ��� �������� �������

    void Start()
    {
        mainInteractableObject = GetComponent<MainInteractableObject>();
        meshCollider = GetComponent<MeshCollider>();
        interactionRequirement = GetComponent<InteractionRequirement>();

        // ���������� ��������� ���������
        if (meshCollider != null)
        {
            meshCollider.enabled = false;
        }
    }

    void Update()
    {
        // ���������, ��������� �� ������� ��� �������������� � ��������
        if (interactionRequirement != null && interactionRequirement.AreRequirementsMet())
        {
            // ���� ������� ���������, �������� ��������� ��� ��������������
            if (meshCollider != null && !meshCollider.enabled)
            {
                meshCollider.enabled = true;
            }
        }
    }

    public void PickUp()
    {
        // ������ ��� ������� �������
        gameObject.SetActive(false); // ��������� ������ �� �����

        // ����������� ������� ��������� ��������
        piesEaten++;

        // ���������� ������ ��������� � ���������� ������
        mainInteractableObject.Interact();

        // ���������, ��� �� ������ ��������� �������
        if (piesEaten == totalPies)
        {
            Telephone telephone = FindObjectOfType<Telephone>();
            if (telephone != null)
            {
                telephone.StartRinging();
            }
        }
    }
}
