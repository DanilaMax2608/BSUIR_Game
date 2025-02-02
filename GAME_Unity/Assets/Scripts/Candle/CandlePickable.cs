using UnityEngine;

public class CandlePickable : MonoBehaviour, IPickable
{
    public void PickUp()
    {
        // Логика для подбора свечки
        gameObject.SetActive(false); // Отключаем объект со сцены
    }

    public void Place()
    {
        // Логика для размещения свечки
        gameObject.SetActive(true); // Включаем объект на сцене
    }
}
