using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip backgroundMusic; // Аудиоклип с фоновой музыкой
    private AudioSource audioSource; // Компонент для воспроизведения звука

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = backgroundMusic;
        audioSource.loop = true; // Устанавливаем повторение музыки
        audioSource.Play(); // Начинаем воспроизведение музыки
    }

    // Метод для изменения фоновой музыки
    public void ChangeBackgroundMusic(AudioClip newMusic, float volume = 0.1f)
    {
        if (audioSource != null && newMusic != null)
        {
            audioSource.clip = newMusic;
            audioSource.volume = volume; // Устанавливаем громкость
            audioSource.Play();
        }
    }
}
