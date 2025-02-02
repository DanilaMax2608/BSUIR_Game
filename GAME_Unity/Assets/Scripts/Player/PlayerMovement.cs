using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private float soundThreshold = 0.1f;
    [SerializeField] private float soundDelay = 0.5f;

    private new Rigidbody rigidbody;
    private AudioSource audioSource;
    private Coroutine walkSoundCoroutine;
    private bool isInteracting = false; // Флаг для отключения движения
    private Vector3 savedPosition;
    private Quaternion savedRotation;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (isInteracting)
        {
            // Если идет взаимодействие, не обрабатываем ввод и фиксируем позицию и поворот
            rigidbody.linearVelocity = Vector3.zero;
            transform.position = savedPosition;
            transform.rotation = savedRotation;
            return;
        }

        float targetMovingSpeed = speed;
        Vector2 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);
        rigidbody.linearVelocity = transform.rotation * new Vector3(targetVelocity.x, rigidbody.linearVelocity.y, targetVelocity.y);

        if (targetVelocity.magnitude > soundThreshold)
        {
            if (walkSoundCoroutine == null && audioSource.enabled)
            {
                walkSoundCoroutine = StartCoroutine(PlayWalkSound());
            }
        }
        else
        {
            if (walkSoundCoroutine != null)
            {
                StopCoroutine(walkSoundCoroutine);
                walkSoundCoroutine = null;
                if (audioSource.enabled)
                {
                    audioSource.Stop();
                }
            }
        }
    }

    private IEnumerator PlayWalkSound()
    {
        if (!audioSource.enabled) yield break;

        audioSource.clip = walkSound;
        audioSource.Play();

        while (true)
        {
            yield return new WaitForSeconds(soundDelay);
            if (!audioSource.enabled) yield break;
            if (audioSource.isPlaying)
            {
                continue;
            }
            audioSource.Play();
        }
    }

    // Метод для временного отключения движения
    public void SetInteracting(bool interacting)
    {
        isInteracting = interacting;
        if (interacting)
        {
            // Сохраняем текущую позицию и поворот
            savedPosition = transform.position;
            savedRotation = transform.rotation;
        }
    }
}
