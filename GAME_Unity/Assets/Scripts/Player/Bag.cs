using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
    private List<GameObject> pickedObjects = new List<GameObject>();

    public void AddObject(GameObject obj)
    {
        pickedObjects.Add(obj);
        obj.transform.SetParent(transform); // Перемещаем объект в мешок
        obj.SetActive(false); // Отключаем объект со сцены
    }

    public void RemoveObject(GameObject obj)
    {
        pickedObjects.Remove(obj);
        obj.transform.SetParent(null); // Возвращаем объект на сцену
        obj.SetActive(true); // Включаем объект на сцене
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
