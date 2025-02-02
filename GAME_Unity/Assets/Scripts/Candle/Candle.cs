using UnityEngine;

public class Candle : MonoBehaviour
{
    [SerializeField] private string lighterName = "���������"; // ��� ������� "���������"
    [SerializeField] private Bag playerBag; // ������ �� ����� ������

    public void CheckForLighter(GameObject candle)
    {
        GameObject flame = candle.transform.Find("Flame").gameObject; // �������� ������ "�����"
        if (flame == null)
        {
            return;
        }

        GameObject lighter = playerBag.GetObject(lighterName);
        if (lighter != null)
        {
            flame.SetActive(true);
        }
        else
        {
            flame.SetActive(false);
        }
    }
}
