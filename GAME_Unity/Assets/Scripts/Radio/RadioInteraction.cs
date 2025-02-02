using UnityEngine;

public class RadioInteraction : MonoBehaviour, IInteractable
{
    public AudioSource musicSource;        // Источник для музыки
    public AudioSource buttonSource;       // Источник для звуков кнопок
    public AudioClip buttonOnSound;        // Звук включения
    public AudioClip buttonOffSound;       // Звук выключения
    public AudioClip[] songs;              // Массив песен
    private int currentSongIndex = 0;      // Индекс текущей песни
    public bool isPlaying = false;        // Статус проигрывания
    public static bool isScreamerActive = false; // Статус скримера для блокировки

    void Start()
    {
        songs = Resources.LoadAll<AudioClip>("Audio/tsoi"); // Загружаем все песни из папки
    }

    public void Interact()
    {
        if (isScreamerActive)
        {
            Debug.Log("Радио временно недоступно, пока активен скример.");
            return;
        }

        if (isPlaying)
        {
            StopMusic(); // Остановить музыку, если она играет
        }
        else
        {
            PlayNextSong(); // Проиграть следующую песню, если музыка не играет
        }
    }

    void PlayNextSong()
    {
        if (songs.Length == 0)
        {
            Debug.LogWarning("Нет песен в папке!");
            return;
        }

        Debug.Log("Проигрываю песню: " + songs[currentSongIndex].name);

        buttonSource.PlayOneShot(buttonOnSound); // Воспроизведение звука кнопки включения

        musicSource.clip = songs[currentSongIndex];
        musicSource.Play(); // Проигрывание музыки
        isPlaying = true;

        currentSongIndex = (currentSongIndex + 1) % songs.Length; // Переход к следующей песне
    }

    void StopMusic()
    {
        Debug.Log("Останавливаю музыку");

        buttonSource.PlayOneShot(buttonOffSound); // Воспроизведение звука кнопки выключения
        Invoke(nameof(StopMusicAfterButtonSound), buttonOffSound.length); // Остановка музыки после завершения звука кнопки
    }

    void StopMusicAfterButtonSound()
    {
        musicSource.Stop(); // Остановка музыки
        isPlaying = false;
    }

    public static void SetScreamerActive(bool active)
    {
        isScreamerActive = active;
    }
}
