using UnityEngine;

public class EatableObject : MonoBehaviour, IEatable
{
    public void Eat()
    {
        // ������ ��� ����������� �������
        Destroy(gameObject);
    }
}
