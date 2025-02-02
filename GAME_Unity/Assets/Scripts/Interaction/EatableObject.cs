using UnityEngine;

public class EatableObject : MonoBehaviour, IEatable
{
    public void Eat()
    {
        // Логика для уничтожения объекта
        Destroy(gameObject);
    }
}
