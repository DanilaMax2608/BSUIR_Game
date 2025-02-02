using UnityEngine;

public class CursorManager : MonoBehaviour
{
    void Start()
    {
        // Включаем курсор и разблокируем его
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
