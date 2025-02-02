using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class LightSwitch : MonoBehaviour, IInteractable
{
    [SerializeField] private Light[] roomLights; // Массив источников света в комнате
    [SerializeField] private GameObject bulbObject; // Объект лампочки для отключения визуального эффекта после "взрыва"
    [SerializeField] private AudioClip popSound; // Звук лопания лампочки
    [SerializeField] private AudioClip switchSound; // Звук переключения выключателя
    [SerializeField] private int maxPressCount = 10; // Порог для лопания лампочки
    [SerializeField] private float rotationAngle = 90f; // Угол поворота кнопки
    [SerializeField] private Vector3 rotationAxis = Vector3.up; // Ось поворота кнопки
    [SerializeField] private float rotationDuration = 0.5f; // Время плавного поворота
    [SerializeField] private float flashDuration = 0.2f; // Длительность вспышки света

    private AudioSource audioSource;
    private bool isLightOn = false; // Флаг для отслеживания состояния света
    private bool isRotated = false; // Флаг для отслеживания состояния поворота кнопки
    private bool hasBeenTurnedOn = false; // Флаг для отслеживания, был ли свет уже включен хотя бы один раз
    private bool isBulbPopped = false; // Флаг для блокировки включения света после лопания
    private int pressCounter = 0; // Счетчик нажатий

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        pressCounter++;

        // Если лампочка лопнула, выполняем только вращение кнопки, не включая свет
        if (isBulbPopped)
        {
            StartCoroutine(RotateSwitch());
            return;
        }

        // Если достигли порога нажатий, лампочка лопается
        if (pressCounter >= maxPressCount)
        {
            PopLight();
            return;
        }

        // Включение или выключение света в нормальном режиме
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

        // Включение или выключение эмиссивного свечения
        SetEmission(isLightOn);

        StartCoroutine(RotateSwitch());

        // Воспроизводим звук переключения
        if (audioSource != null && switchSound != null)
        {
            audioSource.PlayOneShot(switchSound);
        }
    }

    private void PopLight()
    {
        isBulbPopped = true; // Блокируем включение света
        StartCoroutine(FlashAndPop());

        if (bulbObject != null)
        {
            bulbObject.SetActive(false); // Отключаем объект лампочки
        }

        // Воспроизводим звук лопания
        if (audioSource != null && popSound != null)
        {
            audioSource.PlayOneShot(popSound);
        }

        // Отключаем эмиссивное свечение
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
