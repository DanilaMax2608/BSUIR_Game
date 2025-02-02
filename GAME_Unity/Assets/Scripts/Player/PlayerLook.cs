using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float sensitivity = 2;
    [SerializeField] private float smoothing = 1.5f;

    [SerializeField] private float swayAmount = 0.02f;
    [SerializeField] private float swaySpeed = 1f;

    [SerializeField] private float headBobAngle = 2f;
    [SerializeField] private float headBobSmoothing = 0.1f;

    [SerializeField] private float walkSwayAngleZ = 3f;
    [SerializeField] private float walkSwaySpeed = 8f;
    [SerializeField] private float walkShiftAmount = 0.02f;

    [SerializeField] private float idleSwayAmount = 0.01f; // Амплитуда покачивания при стоянии
    [SerializeField] private float idleSwaySpeed = 1f; // Скорость покачивания при стоянии
    [SerializeField] private float swayInterpolationSpeed = 5f; // Скорость интерполяции для плавного перехода

    private Vector2 velocity;
    private Vector2 frameVelocity;
    private Vector3 originalPosition;
    private float currentHeadBobAngle = 0f;
    private float walkSwayTimer = 0f;
    private float idleSwayTimer = 0f; // Таймер для покачивания при стоянии

    private bool isInteracting = false;
    private float movementThreshold = 0.1f;
    private bool isPlayerMoving = false;
    private float currentSwayZ = 0f;
    private float currentIdleSwayX = 0f;
    private float currentIdleSwayY = 0f;

    private Transform queenOfSpades; // ������ ������� ����
    private bool isLocked = false; // ���� ��� ���������� �������� ������

    void Reset()
    {
        player = GetComponentInParent<PlayerMovement>().transform;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (isInteracting || isLocked) return;

        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90, 90);

        // ���������� ������� ������ �� ����������� ����
        transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        player.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);

        float targetHeadBobAngle = Mathf.Clamp(-mouseDelta.x * headBobAngle, -headBobAngle, headBobAngle);
        currentHeadBobAngle = Mathf.Lerp(currentHeadBobAngle, targetHeadBobAngle, headBobSmoothing);
        transform.localRotation *= Quaternion.Euler(0, 0, currentHeadBobAngle);

        // ���������, �������� �� �����, � ��������� ����������� ������ ��� ��������
        isPlayerMoving = player.GetComponent<Rigidbody>().linearVelocity.magnitude > movementThreshold;
        if (isPlayerMoving)
        {
            walkSwayTimer += Time.deltaTime * walkSwaySpeed;

            currentSwayZ = Mathf.Sin(walkSwayTimer) * walkSwayAngleZ;
            float shiftY = Mathf.Cos(walkSwayTimer * 2) * walkShiftAmount;

            transform.localPosition = originalPosition + new Vector3(0, shiftY, 0);
            transform.localRotation *= Quaternion.Euler(0, 0, currentSwayZ);

            // Сброс покачивания при стоянии
            currentIdleSwayX = Mathf.Lerp(currentIdleSwayX, 0, Time.deltaTime * swayInterpolationSpeed);
            currentIdleSwayY = Mathf.Lerp(currentIdleSwayY, 0, Time.deltaTime * swayInterpolationSpeed);
        }
        else
        {
            // ������� ������� � ������������ ���������, ���� ����� �����������
            currentSwayZ = Mathf.Lerp(currentSwayZ, 0, Time.deltaTime * swaySpeed);
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * swaySpeed);
            transform.localRotation *= Quaternion.Euler(0, 0, currentSwayZ);

            // ���������� ���������� ��������� ���������
            idleSwayTimer += Time.deltaTime * idleSwaySpeed;
            float idleSwayX = Mathf.Sin(idleSwayTimer) * idleSwayAmount;
            float idleSwayY = Mathf.Cos(idleSwayTimer) * idleSwayAmount;

            currentIdleSwayX = Mathf.Lerp(currentIdleSwayX, idleSwayX, Time.deltaTime * swayInterpolationSpeed);
            currentIdleSwayY = Mathf.Lerp(currentIdleSwayY, idleSwayY, Time.deltaTime * swayInterpolationSpeed);

            transform.localPosition = originalPosition + new Vector3(currentIdleSwayX, currentIdleSwayY, 0);
        }

        // ���������, ������� �� ����� �� ������� ����
        if (queenOfSpades != null)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10f))
            {
                if (hit.collider.GetComponent<QueenOfSpades>() != null)
                {
                    transform.LookAt(queenOfSpades);
                }
            }
        }
    }

    // ����� ��� ���������� ���������� ��������� ����� ����
    public void SetInteracting(bool interacting)
    {
        isInteracting = interacting;
    }

    // ����� ��� ��������� ������� ������� ����
    public void SetQueenOfSpades(Transform queenTransform)
    {
        queenOfSpades = queenTransform;
    }
}
