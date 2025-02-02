using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
    private List<GameObject> pickedObjects = new List<GameObject>();

    public void AddObject(GameObject obj)
    {
        pickedObjects.Add(obj);
        obj.transform.SetParent(transform); // ���������� ������ � �����
        obj.SetActive(false); // ��������� ������ �� �����
    }

    public void RemoveObject(GameObject obj)
    {
        pickedObjects.Remove(obj);
        obj.transform.SetParent(null); // ���������� ������ �� �����
        obj.SetActive(true); // �������� ������ �� �����
    }

    public bool ContainsObject(GameObject obj)
    {
        return pickedObjects.Contains(obj);
    }

    public GameObject GetObject(string objectName)
    {
        return pickedObjects.Find(obj => obj.name == objectName);
    }
}
