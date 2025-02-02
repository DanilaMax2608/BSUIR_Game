using UnityEngine;

public class CandlePickable : MonoBehaviour, IPickable
{
    public void PickUp()
    {
        // ������ ��� ������� ������
        gameObject.SetActive(false); // ��������� ������ �� �����
    }

    public void Place()
    {
        // ������ ��� ���������� ������
        gameObject.SetActive(true); // �������� ������ �� �����
    }
}
