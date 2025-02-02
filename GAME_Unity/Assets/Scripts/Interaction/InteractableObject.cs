using System.Collections;
using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private Vector3 rotationAxis = Vector3.up; // Ось, вокруг которой поворачивается объект
    [SerializeField] private float rotationAngle = 90f; // Угол, на который мы поворачиваем объект
    [SerializeField] private float rotationSpeed = 2f; // Скорость поворота
    [SerializeField] private AudioClip openSound; // Звук для открытия объекта
    [SerializeField] private AudioClip closeSound; // Звук для закрытия объекта
    [SerializeField] private AudioSource doorAudioSource; // Компонент для звуков открытия и закрытия двери

    [Header("Door Handle Settings")]
    [SerializeField] private bool hasHandle = false; // Флаг, указывающий, есть ли у объекта ручка
    [SerializeField] private DoorHandle doorHandle; // Ссылка на ручку двери

    [Header("Auto Close Settings")]
    [SerializeField] private bool automatically = false; // Флаг для автоматического закрытия
    [SerializeField] private float autoCloseDelay = 5f; // Задержка перед автоматическим закрытием

    public bool isOpen = false;
    private bool isRotating = false; // Флаг для проверки процесса поворота
    private Quaternion openRotation;
    private Quaternion closedRotation;
    private Collider doorCollider; // Коллайдер двери

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.AngleAxis(rotationAngle, rotationAxis) * closedRotation;
        doorCollider = GetComponent<Collider>();
    }

    public void Interact()
    {
        if (!isRotating) // Проверка, что объект не в процессе поворота
        {
            ToggleObject();
        }
    }

    private void ToggleObject()
    {
        isOpen = !isOpen;
        if (isOpen)
        {
            PlaySound(openSound);
            StartCoroutine(RotateObject(openRotation));
            if (hasHandle)
            {
                doorHandle.ToggleHandle(true);
            }
            if (automatically)
            {
                StartCoroutine(AutoClose());
            }
        }
        else
        {
            PlaySound(closeSound);
            StartCoroutine(RotateObject(closedRotation));
            if (hasHandle)
            {
                doorHandle.ToggleHandle(false);
            }
        }
    }

    private IEnumerator RotateObject(Quaternion targetRotation)
    {
        isRotating = true; // Устанавливаем флаг, что объект в процессе поворота
        float t = 0;
        Quaternion startRotation = transform.rotation;

        while (t < 1)
        {
            t += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null;
        }

        isRotating = false; // Сбрасываем флаг после завершения поворота
        if (hasHandle)
        {
            doorHandle.ResetHandle(); // Возвращаем ручку в исходное состояние
        }
    }

    private IEnumerator AutoClose()
    {
        yield return new WaitForSeconds(autoCloseDelay);
        if (isOpen)
        {
            ToggleObject();
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (doorAudioSource != null && clip != null)
        {
            doorAudioSource.PlayOneShot(clip);
        }
    }

    public void CloseDoor()
    {
        if (isOpen)
        {
            ToggleObject();
        }
    }

    public void DisableCollider()
    {
        if (doorCollider != null)
        {
            doorCollider.enabled = false;
        }
    }

    public void EnableCollider()
    {
        if (doorCollider != null)
        {
            doorCollider.enabled = true;
        }
    }
}
