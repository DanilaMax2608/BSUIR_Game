using UnityEngine;

public class LightController : MonoBehaviour
{
    public Light[] lights; // ������ ���������� �����
    public LightSwitch[] lightSwitches; // ������ ������������

    public void TurnOffLights()
    {
        foreach (var light in lights)
        {
            if (light != null && light.enabled)
            {
                // ������� �����������, ������� ��������� ���� ������
                foreach (var lightSwitch in lightSwitches)
                {
                    if (lightSwitch != null && lightSwitch.IsControllingLight(light))
                    {
                        lightSwitch.Interact(); // ��������� ������� �� �����������
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
