using UnityEngine;
using UnityEngine.SceneManagement;

public class QueenOfSpades : MonoBehaviour
{
    public float menuTransitionDelay = 5f; // Задержка перед переходом на сцену меню
    private bool isTransitioning = false;
    private bool isScreamerPlaying = false; // Флаг для проверки воспроизведения мелодии
    private DialogueManager dialogueManager; // Компонент для управления диалогом
    public GameObject dialogueTrigger; // Объект триггера диалога
    public AudioClip screamerSound; // Звук скримера
    private AudioSource audioSource; // Компонент для воспроизведения звука

    private Vector3 savedPlayerPosition; // Сохраненная позиция игрока
    private Quaternion savedPlayerRotation; // Сохраненный поворот игрока

    private void Start()
    {
        dialogueManager = GetComponentInChildren<DialogueManager>();
        if (dialogueManager != null)
        {
            dialogueManager.enabled = false;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = screamerSound;
    }

    void Update()
    {
        // Проверяем, смотрит ли игрок на Пиковую Даму
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10f))
        {
            if (hit.collider.gameObject == gameObject)
            {
                PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
                PlayerLook playerLook = playerMovement.GetComponentInChildren<PlayerLook>();
                playerMovement.SetInteracting(true);
                playerLook.SetInteracting(true);

                // Сохраняем текущую позицию и поворот игрока
                savedPlayerPosition = playerMovement.transform.position;
                savedPlayerRotation = playerMovement.transform.rotation;

                // Поворачиваем камеру к дочернему объекту для диалога
                if (dialogueTrigger != null)
                {
                    Vector3 targetPosition = dialogueTrigger.transform.position;
                    Vector3 cameraPosition = Camera.main.transform.position;
                    Vector3 direction = (targetPosition - cameraPosition).normalized;
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    Camera.main.transform.rotation = targetRotation;
                }

                // Запускаем диалог
                if (dialogueManager != null)
                {
                    dialogueManager.enabled = true;
                    dialogueManager.StartDialogue(playerMovement, playerLook);
                }

                // Воспроизводим звук скримера, если он еще не проигрывается
                if (audioSource != null && screamerSound != null && !isScreamerPlaying)
                {
                    audioSource.Play();
                    isScreamerPlaying = true;
                }

                // Запускаем таймер для перехода на сцену меню
                if (!isTransitioning)
                {
                    isTransitioning = true;
                    Invoke("LoadMenuScene", menuTransitionDelay);
                }
            }
        }
    }

    // Метод для загрузки сцены меню
    private void LoadMenuScene()
    {
        EnableCursor();
        SceneManager.LoadScene("Main_Menu");
    }

    // Метод для включения курсора
    private void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Метод для обработки завершения воспроизведения звука
    private void OnAudioFinished()
    {
        isScreamerPlaying = false;
    }
}
