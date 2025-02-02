using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startButton; // ������ "������ ����"
    public Button exitButton; // ������ "�����"
    public string gameSceneName = "Game"; // �������� �����, �� ������� ����� ������� ��� ������� �� ������ "������ ����"

    void Start()
    {
        // ������������� �� ������� ������� ������
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClick);
        }

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitButtonClick);
        }
    }

    // ����� ��� ��������� ������� ������ "������ ����"
    private void OnStartButtonClick()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // ����� ��� ��������� ������� ������ "�����"
    private void OnExitButtonClick()
    {
        Application.Quit();
    }
}
