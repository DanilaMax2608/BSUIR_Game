using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip backgroundMusic; // ��������� � ������� �������
    private AudioSource audioSource; // ��������� ��� ��������������� �����

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = backgroundMusic;
        audioSource.loop = true; // ������������� ���������� ������
        audioSource.Play(); // �������� ��������������� ������
    }

    // ����� ��� ��������� ������� ������
    public void ChangeBackgroundMusic(AudioClip newMusic, float volume = 0.1f)
    {
        if (audioSource != null && newMusic != null)
        {
            audioSource.clip = newMusic;
            audioSource.volume = volume; // ������������� ���������
            audioSource.Play();
        }
    }
}
