using UnityEngine;

public class LightController : MonoBehaviour
{
    public Light[] lights; // Массив источников света
    public LightSwitch[] lightSwitches; // Массив выключателей

    public void TurnOffLights()
    {
        foreach (var light in lights)
        {
            if (light != null && light.enabled)
            {
                // Находим выключатель, который управляет этим светом
                foreach (var lightSwitch in lightSwitches)
                {
                    if (lightSwitch != null && lightSwitch.IsControllingLight(light))
                    {
                        lightSwitch.Interact(); // Имитируем нажатие на выключатель
                    }
                }
            }
        }
    }

    public void TurnOnLights()
    {
        foreach (var light in lights)
        {
            if (light != null)
            {
                light.enabled = true;
            }
        }
    }
}
