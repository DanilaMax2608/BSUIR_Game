using UnityEngine;

public class Telephone : MonoBehaviour, IInteractable
{
    public AudioClip ringtone;
    private AudioSource audioSource;
    private bool isRinging = false;
    private bool hasInteracted = false;
    private DialogueManager dialogueManager;
    private InteractionRequirement interactionRequirement; // Новый компонент для проверки условий
    private Collider telephoneCollider; // Коллайдер телефона

    private Coroutine typingCoroutine;
    private Coroutine displayCoroutine;

    private TelephoneSwitcher telephoneSwitcher; // Ссылка на скрипт TelephoneSwitcher

    void Start()
    {
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        audioSource.clip = ringtone;
        audioSource.loop = true;
        audioSource.enabled = false;

        dialogueManager = GetComponentInChildren<DialogueManager>();
        if (dialogueManager != null)
        {
            dialogueManager.enabled = false;
            dialogueManager.OnDialogueEnd += OnDialogueEnd; // Подписываемся на событие завершения диалога
        }

        interactionRequirement = GetComponent<InteractionRequirement>();
        telephoneCollider = GetComponent<Collider>(); // Получаем коллайдер телефона

        // Изначально отключаем коллайдер
        if (telephoneCollider != null)
        {
            telephoneCollider.enabled = false;
        }

        // Получаем ссылку на скрипт TelephoneSwitcher, если он есть
        telephoneSwitcher = GetComponent<TelephoneSwitcher>();
    }

    void Update()
    {
        // Проверяем, выполнены ли условия для взаимодействия с телефоном
        if (interactionRequirement != null && interactionRequirement.AreRequirementsMet())
        {
            // Если условия выполнены, включаем коллайдер для взаимодействия
            if (telephoneCollider != null && !telephoneCollider.enabled)
            {
                telephoneCollider.enabled = true;
            }
        }
    }

    public void StartRinging()
    {
        if (!isRinging)
        {
            isRinging = true;
            audioSource.enabled = true;
            audioSource.Play();
        }
    }

    public void Interact()
    {
        if (hasInteracted) return;

        // Проверяем выполнение условий перед взаимодействием
        if (interactionRequirement != null && !interactionRequirement.AreRequirementsMet())
        {
            return;
        }

        hasInteracted = true;

        if (isRinging)
        {
            audioSource.Stop();
            audioSource.enabled = false;
            isRinging = false;
        }

        if (dialogueManager != null)
        {
            PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
            PlayerLook playerLook = playerMovement.GetComponentInChildren<PlayerLook>();
            playerMovement.SetInteracting(true);
            playerLook.SetInteracting(true);
            dialogueManager.enabled = true;
            dialogueManager.StartDialogue(playerMovement, playerLook);
        }
        // Отключаем коллайдер после взаимодействия
        if (telephoneCollider != null)
        {
            telephoneCollider.enabled = false;
        }
    }

    private void OnDialogueEnd()
    {
        // Переключаем телефоны, если есть скрипт TelephoneSwitcher
        if (telephoneSwitcher != null)
        {
            telephoneSwitcher.SwitchTelephones();
        }

        // Отключаем коллайдер после завершения диалога
        if (telephoneCollider != null)
        {
            telephoneCollider.enabled = false;
        }

        MainInteractableObject mainInteractableObject = GetComponent<MainInteractableObject>();
        if (mainInteractableObject != null)
        {
            mainInteractableObject.Interact();
        }
    }
}
