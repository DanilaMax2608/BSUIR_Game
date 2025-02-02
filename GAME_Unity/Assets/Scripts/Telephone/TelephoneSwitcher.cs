using UnityEngine;

public class TelephoneSwitcher : MonoBehaviour
{
    public GameObject secondTelephone; // Второй телефон, который нужно включить

    private void Start()
    {
        if (secondTelephone != null)
        {
            secondTelephone.SetActive(false); // Изначально второй телефон выключен
        }
    }

    public void SwitchTelephones()
    {
        if (secondTelephone != null)
        {
            secondTelephone.SetActive(true); // Включаем второй телефон
        }

        gameObject.SetActive(false); // Выключаем первый телефон
    }
}
