using UnityEngine;

public class Mirror : MonoBehaviour, IInteractable
{
    [SerializeField] private string pomadeName = "Pomade"; // Имя объекта помады
    [SerializeField] private GameObject drawing; // Объект, который будет отображаться на зеркале
    [SerializeField] private AudioClip newBackgroundMusic; // Новая фоновая музыка

    private bool isDrawingShown = false;
    private MainInteractableObject mainInteractableObject;
    private InteractionRequirement interactionRequirement;
    private Bag playerBag;
    private Collider mirrorCollider; // Коллайдер зеркала

    void Start()
    {
        mainInteractableObject = GetComponent<MainInteractableObject>();
        interactionRequirement = GetComponent<InteractionRequirement>();
        playerBag = FindObjectOfType<Bag>();
        mirrorCollider = GetComponent<Collider>(); // Получаем коллайдер зеркала

        // Изначально отключаем коллайдер
        if (mirrorCollider != null)
        {
            mirrorCollider.enabled = false;
        }
    }

    void Update()
    {
        // Проверяем выполнение условий перед взаимодействием
        if (interactionRequirement != null && interactionRequirement.AreRequirementsMet())
        {
            // Включаем коллайдер, если условия выполнены
            if (mirrorCollider != null && !mirrorCollider.enabled)
            {
                mirrorCollider.enabled = true;
            }
        }
    }

    public void Interact()
    {
        if (isDrawingShown) return;

        // Проверка на наличие помады и выполнение необходимых событий
        GameObject pomade = playerBag.GetObject(pomadeName);
        if (pomade != null && (interactionRequirement == null || interactionRequirement.AreRequirementsMet()))
        {
            ShowDrawing();
        }
    }

    private void ShowDrawing()
    {
        drawing.SetActive(true);
        isDrawingShown = true;

        // Отключаем Collider у зеркала
        if (mirrorCollider != null)
        {
            mirrorCollider.enabled = false;
        }

        // Отправляем сигнал менеджеру о завершении задачи
        mainInteractableObject?.Interact();

        // Изменяем фоновую музыку и увеличиваем громкость
        BackgroundMusic backgroundMusic = FindObjectOfType<BackgroundMusic>();
        if (backgroundMusic != null && newBackgroundMusic != null)
        {
            backgroundMusic.ChangeBackgroundMusic(newBackgroundMusic, 0.25f);
        }
    }
}
