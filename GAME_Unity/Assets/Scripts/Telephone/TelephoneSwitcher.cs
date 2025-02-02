using UnityEngine;

public class TelephoneSwitcher : MonoBehaviour
{
    public GameObject secondTelephone; // ������ �������, ������� ����� ��������

    private void Start()
    {
        if (secondTelephone != null)
        {
            secondTelephone.SetActive(false); // ���������� ������ ������� ��������
        }
    }

    public void SwitchTelephones()
    {
        if (secondTelephone != null)
        {
            secondTelephone.SetActive(true); // �������� ������ �������
        }

        gameObject.SetActive(false); // ��������� ������ �������
    }
}
