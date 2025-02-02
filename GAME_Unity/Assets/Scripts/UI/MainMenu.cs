using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startButton; // Кнопка "Начать игру"
    public Button exitButton; // Кнопка "Выход"
    public string gameSceneName = "Game"; // Название сцены, на которую нужно перейти при нажатии на кнопку "Начать игру"

    void Start()
    {
        // Подписываемся на события нажатия кнопок
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClick);
        }

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitButtonClick);
        }
    }

    // Метод для обработки нажатия кнопки "Начать игру"
    private void OnStartButtonClick()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Метод для обработки нажатия кнопки "Выход"
    private void OnExitButtonClick()
    {
        Application.Quit();
    }
}
