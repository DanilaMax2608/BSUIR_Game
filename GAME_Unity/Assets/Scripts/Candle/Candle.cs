using UnityEngine;

public class Candle : MonoBehaviour
{
    [SerializeField] private string lighterName = "Зажигалка"; // Имя объекта "Зажигалка"
    [SerializeField] private Bag playerBag; // Ссылка на мешок игрока

    public void CheckForLighter(GameObject candle)
    {
        GameObject flame = candle.transform.Find("Flame").gameObject; // Дочерний объект "Пламя"
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
