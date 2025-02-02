using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Doorbell : MonoBehaviour, IInteractable
{
    [SerializeField] private AudioClip doorbellSound; // Звук звонка
    [SerializeField] private float cooldownTime = 2f; // Время задержки перед следующей возможной активацией

    private AudioSource audioSource;
    private bool isOnCooldown = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (doorbellSound != null)
        {
            audioSource.clip = doorbellSound;
        }
    }

    public void Interact()
    {
        if (!isOnCooldown)
        {
            PlayDoorbellSound();
            StartCoroutine(CooldownRoutine());
        }
    }

    private void PlayDoorbellSound()
    {
        if (audioSource != null && doorbellSound != null)
        {
            audioSource.PlayOneShot(doorbellSound);
        }
    }

    private IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
    }
}
