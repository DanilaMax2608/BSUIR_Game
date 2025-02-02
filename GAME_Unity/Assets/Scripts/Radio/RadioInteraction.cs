using UnityEngine;

public class RadioInteraction : MonoBehaviour, IInteractable
{
    public AudioSource musicSource;        // �������� ��� ������
    public AudioSource buttonSource;       // �������� ��� ������ ������
    public AudioClip buttonOnSound;        // ���� ���������
    public AudioClip buttonOffSound;       // ���� ����������
    public AudioClip[] songs;              // ������ �����
    private int currentSongIndex = 0;      // ������ ������� �����
    public bool isPlaying = false;        // ������ ������������
    public static bool isScreamerActive = false; // ������ �������� ��� ����������

    void Start()
    {
        songs = Resources.LoadAll<AudioClip>("Audio/tsoi"); // ��������� ��� ����� �� �����
    }

    public void Interact()
    {
        if (isScreamerActive)
        {
            Debug.Log("����� �������� ����������, ���� ������� �������.");
            return;
        }

        if (isPlaying)
        {
            StopMusic(); // ���������� ������, ���� ��� ������
        }
        else
        {
            PlayNextSong(); // ��������� ��������� �����, ���� ������ �� ������
        }
    }

    void PlayNextSong()
    {
        if (songs.Length == 0)
        {
            Debug.LogWarning("��� ����� � �����!");
            return;
        }

        Debug.Log("���������� �����: " + songs[currentSongIndex].name);

        buttonSource.PlayOneShot(buttonOnSound); // ��������������� ����� ������ ���������

        musicSource.clip = songs[currentSongIndex];
        musicSource.Play(); // ������������ ������
        isPlaying = true;

        currentSongIndex = (currentSongIndex + 1) % songs.Length; // ������� � ��������� �����
    }

    void StopMusic()
    {
        Debug.Log("������������ ������");

        buttonSource.PlayOneShot(buttonOffSound); // ��������������� ����� ������ ����������
        Invoke(nameof(StopMusicAfterButtonSound), buttonOffSound.length); // ��������� ������ ����� ���������� ����� ������
    }

    void StopMusicAfterButtonSound()
    {
        musicSource.Stop(); // ��������� ������
        isPlaying = false;
    }

    public static void SetScreamerActive(bool active)
    {
        isScreamerActive = active;
    }
}
