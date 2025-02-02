using System.Collections;
using UnityEngine;

public class DoorHandle : MonoBehaviour
{
    [SerializeField] private Vector3 rotationAxis = Vector3.right; // ���, ������ ������� �������������� �����
    [SerializeField] private float handleRotationAngle = 30f; // ���� �������� �����
    [SerializeField] private float handleRotationSpeed = 2f; // �������� �������� �����

    private Quaternion openRotation;
    private Quaternion closedRotation;
    private bool isRotating = false;

    void Start()
    {
        closedRotation = transform.localRotation;
        openRotation = Quaternion.AngleAxis(handleRotationAngle, rotationAxis) * closedRotation;
    }

    public void ToggleHandle(bool isOpen)
    {
        if (!isRotating)
        {
            StartCoroutine(RotateHandle(isOpen ? openRotation : closedRotation));
        }
    }

    public void ResetHandle()
    {
        if (!isRotating)
        {
            StartCoroutine(RotateHandle(closedRotation));
        }
    }

    private IEnumerator RotateHandle(Quaternion targetRotation)
    {
        isRotating = true;
        float t = 0;
        Quaternion startRotation = transform.localRotation;

        while (t < 1)
        {
            t += Time.deltaTime * handleRotationSpeed;
            transform.localRotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null;
        }

        isRotating = false;
    }
}
