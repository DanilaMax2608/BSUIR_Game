using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class LightSwitch : MonoBehaviour, IInteractable
{
    [SerializeField] private Light[] roomLights; // ������ ���������� ����� � �������
    [SerializeField] private GameObject bulbObject; // ������ �������� ��� ���������� ����������� ������� ����� "������"
    [SerializeField] private AudioClip popSound; // ���� ������� ��������
    [SerializeField] private AudioClip switchSound; // ���� ������������ �����������
    [SerializeField] private int maxPressCount = 10; // ����� ��� ������� ��������
    [SerializeField] private float rotationAngle = 90f; // ���� �������� ������
    [SerializeField] private Vector3 rotationAxis = Vector3.up; // ��� �������� ������
    [SerializeField] private float rotationDuration = 0.5f; // ����� �������� ��������
    [SerializeField] private float flashDuration = 0.2f; // ������������ ������� �����

    private AudioSource audioSource;
    private bool isLightOn = false; // ���� ��� ������������ ��������� �����
    private bool isRotated = false; // ���� ��� ������������ ��������� �������� ������
    private bool hasBeenTurnedOn = false; // ���� ��� ������������, ��� �� ���� ��� ������� ���� �� ���� ���
    private bool isBulbPopped = false; // ���� ��� ���������� ��������� ����� ����� �������
    private int pressCounter = 0; // ������� �������

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        pressCounter++;

        // ���� �������� �������, ��������� ������ �������� ������, �� ������� ����
        if (isBulbPopped)
        {
            StartCoroutine(RotateSwitch());
            return;
        }

        // ���� �������� ������ �������, �������� ��������
        if (pressCounter >= maxPressCount)
        {
            PopLight();
            return;
        }

        // ��������� ��� ���������� ����� � ���������� ������
        if (!hasBeenTurnedOn)
        {
            foreach (var light in roomLights)
            {
                light.enabled = true;
            }
            hasBeenTurnedOn = true;
            isLightOn = true;
        }
        else
        {
            isLightOn = !isLightOn;
            foreach (var light in roomLights)
            {
                light.enabled = isLightOn;
            }
        }

        // ��������� ��� ���������� ����������� ��������
        SetEmission(isLightOn);

        StartCoroutine(RotateSwitch());

        // ������������� ���� ������������
        if (audioSource != null && switchSound != null)
        {
            audioSource.PlayOneShot(switchSound);
        }
    }

    private void PopLight()
    {
        isBulbPopped = true; // ��������� ��������� �����
        StartCoroutine(FlashAndPop());

        if (bulbObject != null)
        {
            bulbObject.SetActive(false); // ��������� ������ ��������
        }

        // ������������� ���� �������
        if (audioSource != null && popSound != null)
        {
            audioSource.PlayOneShot(popSound);
        }

        // ��������� ���������� ��������
        SetEmission(false);
    }

    private IEnumerator FlashAndPop()
    {
        foreach (var light in roomLights)
        {
            light.enabled = true;
        }

        yield return new WaitForSeconds(flashDuration);

        foreach (var light in roomLights)
        {
            light.enabled = false;
        }
    }

    private void SetEmission(bool enabled)
    {
        if (bulbObject != null)
        {
            Renderer renderer = bulbObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                MaterialPropertyBlock block = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(block);
                float emissionIntensity = enabled ? 10f : 0f;
                block.SetFloat("_EmissiveIntensity", emissionIntensity);
                block.SetColor("_EmissiveColor", Color.white * emissionIntensity);
                renderer.SetPropertyBlock(block);
                DynamicGI.SetEmissive(renderer, Color.white * emissionIntensity);
                Debug.Log("Emission intensity set to: " + emissionIntensity);
            }
            else
            {
                Debug.LogError("Renderer component not found on bulbObject.");
            }
        }
        else
        {
            Debug.LogError("bulbObject is not assigned.");
        }
    }

    private IEnumerator RotateSwitch()
    {
        float elapsedTime = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = isRotated
            ? Quaternion.AngleAxis(-rotationAngle, rotationAxis) * startRotation
            : Quaternion.AngleAxis(rotationAngle, rotationAxis) * startRotation;

        while (elapsedTime < rotationDuration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
        isRotated = !isRotated;
    }

    public bool IsControllingLight(Light light)
    {
        foreach (var roomLight in roomLights)
        {
            if (roomLight == light)
            {
                return true;
            }
        }
        return false;
    }
}
