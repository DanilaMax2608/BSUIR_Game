using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InitialDarkness : MonoBehaviour
{
    [SerializeField] private float darknessDuration = 5f;
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private Image darknessImage;

    [SerializeField] private AudioClip liftSound;
    [SerializeField] private AudioClip doorSound;
    [SerializeField] private float liftSoundDelay = 2f;
    [SerializeField] private float doorSoundDelay = 2f;

    [SerializeField] private TextMeshProUGUI textDisplay;
    [SerializeField] private string[] texts;
    [SerializeField] private float textSpeed = 0.05f;
    [SerializeField] private float textDisplayTime = 2f;

    [SerializeField] private GameObject taskUIObject; // Ссылка на объект TaskUI
    [SerializeField] private GameObject backgroundMusicObject; // Ссылка на объект BackgroundMusic

    private PlayerMovement playerMovement;
    private PlayerLook playerLook;
    private PlayerInteraction playerInteraction;
    private AudioSource audioSource;
    private TaskUI taskUI; // Ссылка на скрипт TaskUI

    private void Start()
    {
        darknessImage.gameObject.SetActive(true);
        darknessImage.color = new Color(0, 0, 0, 1);

        playerMovement = FindObjectOfType<PlayerMovement>();
        playerLook = FindObjectOfType<PlayerLook>();
        playerInteraction = FindObjectOfType<PlayerInteraction>();

        if (playerMovement != null) playerMovement.enabled = false;
        if (playerLook != null) playerLook.enabled = false;
        if (playerInteraction != null) playerInteraction.enabled = false;

        audioSource = gameObject.AddComponent<AudioSource>();

        // Получаем ссылку на скрипт TaskUI
        taskUI = taskUIObject.GetComponent<TaskUI>();

        // Останавливаем все корутины на объекте TaskUI в начале
        if (taskUI != null)
        {
            taskUI.StopAllCoroutines();
        }

        StartCoroutine(ManageDarknessAndSounds());
    }

    private IEnumerator ManageDarknessAndSounds()
    {
        yield return new WaitForSeconds(liftSoundDelay);

        audioSource.clip = liftSound;
        audioSource.Play();

        foreach (var text in texts)
        {
            yield return StartCoroutine(TypeText(text));
            yield return new WaitForSeconds(textDisplayTime);
            textDisplay.text = "";
        }

        yield return new WaitForSeconds(liftSound.length - audioSource.time);

        yield return new WaitForSeconds(doorSoundDelay);

        audioSource.clip = doorSound;
        audioSource.Play();
        yield return new WaitForSeconds(doorSound.length);

        float elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeTime);
            darknessImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        darknessImage.gameObject.SetActive(false);

        if (playerMovement != null) playerMovement.enabled = true;
        if (playerLook != null) playerLook.enabled = true;
        if (playerInteraction != null) playerInteraction.enabled = true;

        // Включаем объект TaskUI и запускаем его корутины перед уничтожением объекта
        if (taskUIObject != null)
        {
            taskUIObject.SetActive(true);
            taskUI.enabled = true;
        }

        // Включаем объект BackgroundMusic
        if (backgroundMusicObject != null)
        {
            backgroundMusicObject.SetActive(true);
        }

        Destroy(gameObject);
    }

    private IEnumerator TypeText(string text)
    {
        textDisplay.text = "";
        foreach (char letter in text.ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }
}
