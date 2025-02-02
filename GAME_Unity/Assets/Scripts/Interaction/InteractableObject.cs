using System.Collections;
using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private Vector3 rotationAxis = Vector3.up; // ���, ������ ������� �������������� ������
    [SerializeField] private float rotationAngle = 90f; // ����, �� ������� �� ������������ ������
    [SerializeField] private float rotationSpeed = 2f; // �������� ��������
    [SerializeField] private AudioClip openSound; // ���� ��� �������� �������
    [SerializeField] private AudioClip closeSound; // ���� ��� �������� �������
    [SerializeField] private AudioSource doorAudioSource; // ��������� ��� ������ �������� � �������� �����

    [Header("Door Handle Settings")]
    [SerializeField] private bool hasHandle = false; // ����, �����������, ���� �� � ������� �����
    [SerializeField] private DoorHandle doorHandle; // ������ �� ����� �����

    [Header("Auto Close Settings")]
    [SerializeField] private bool automatically = false; // ���� ��� ��������������� ��������
    [SerializeField] private float autoCloseDelay = 5f; // �������� ����� �������������� ���������

    public bool isOpen = false;
    private bool isRotating = false; // ���� ��� �������� �������� ��������
    private Quaternion openRotation;
    private Quaternion closedRotation;
    private Collider doorCollider; // ��������� �����

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.AngleAxis(rotationAngle, rotationAxis) * closedRotation;
        doorCollider = GetComponent<Collider>();
    }

    public void Interact()
    {
        if (!isRotating) // ��������, ��� ������ �� � �������� ��������
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
        isRotating = true; // ������������� ����, ��� ������ � �������� ��������
        float t = 0;
        Quaternion startRotation = transform.rotation;

        while (t < 1)
        {
            t += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null;
        }

        isRotating = false; // ���������� ���� ����� ���������� ��������
        if (hasHandle)
        {
            doorHandle.ResetHandle(); // ���������� ����� � �������� ���������
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
