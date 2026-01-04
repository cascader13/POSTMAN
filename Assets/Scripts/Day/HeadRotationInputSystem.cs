using UnityEngine;
using UnityEngine.InputSystem;

public class HeadRotationInputSystem : MonoBehaviour
{
    [Header("Настройки вращения")]
    [SerializeField] private float rotationSpeed = 0.5f;
    [SerializeField] private float maxRotationAngle = 15f;
    [SerializeField] private float smoothFactor = 10f;
    [SerializeField] private bool invertX = false;
    [SerializeField] private bool invertY = false;

    [Header("Ограничения")]
    [SerializeField] private float minYRotation = -30f;
    [SerializeField] private float maxYRotation = 30f;
    [SerializeField] private float minXRotation = -25f;
    [SerializeField] private float maxXRotation = 25f;

    [Header("Возврат в центр")]
    [SerializeField] private bool enableReturnToCenter = true;
    [SerializeField] private float returnToCenterDelay = 3f;
    [SerializeField] private float returnSpeed = 5f;
    [SerializeField] private float mouseDeadzone = 0.1f; // Минимальное движение мыши для сброса таймера

    private Vector2 mouseDelta;
    private Vector3 targetRotation;
    private Vector3 currentRotation;
    private Vector3 centerRotation;

    // Таймеры для возврата в центр
    private float inactivityTimer = 0f;
    private bool isReturningToCenter = false;

    private void Start()
    {
        centerRotation = transform.localEulerAngles;
        targetRotation = centerRotation;
        currentRotation = targetRotation;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Vector2 mouseInput = Mouse.current.delta.ReadValue();
        UpdateRotation(mouseInput);
        UpdateInactivityTimer(mouseInput);
        CheckReturnToCenter();
        ApplySmoothRotation();
    }

    private void UpdateRotation(Vector2 mouseInput)
    {
        if (isReturningToCenter && enableReturnToCenter)
            return;

        float xMultiplier = invertX ? -1f : 1f;
        float yMultiplier = invertY ? -1f : 1f;

        mouseInput = Vector2.ClampMagnitude(mouseInput, maxRotationAngle * 2);

        targetRotation.y += mouseInput.x * rotationSpeed * xMultiplier * Time.deltaTime;
        targetRotation.x -= mouseInput.y * rotationSpeed * yMultiplier * Time.deltaTime;

        targetRotation.x = Mathf.Clamp(targetRotation.x, minXRotation, maxXRotation);
        targetRotation.y = Mathf.Clamp(targetRotation.y, minYRotation, maxYRotation);
        targetRotation.z = 0;
    }

    private void UpdateInactivityTimer(Vector2 mouseInput)
    {
        if (Mathf.Abs(mouseInput.x) > mouseDeadzone ||
            Mathf.Abs(mouseInput.y) > mouseDeadzone)
        {
            inactivityTimer = 0f;
            isReturningToCenter = false;
        }
        else if (enableReturnToCenter && !isReturningToCenter)
        {
            inactivityTimer += Time.deltaTime;
        }
    }

    private void CheckReturnToCenter()
    {
        if (!enableReturnToCenter || isReturningToCenter)
            return;
        if (inactivityTimer >= returnToCenterDelay)
        {
            StartReturnToCenter();
        }
    }

    private void StartReturnToCenter()
    {
        isReturningToCenter = true;
        inactivityTimer = 0f;
    }

    private void ApplySmoothRotation()
    {
        Vector3 finalTargetRotation = targetRotation;

        if (isReturningToCenter && enableReturnToCenter)
        {
            finalTargetRotation = Vector3.Lerp(
                targetRotation,
                centerRotation,
                returnSpeed * Time.deltaTime
            );

            targetRotation = finalTargetRotation;

            if (Vector3.Distance(targetRotation, centerRotation) < 0.1f)
            {
                targetRotation = centerRotation;
                isReturningToCenter = false;
            }
        }

        currentRotation = Vector3.Lerp(
            currentRotation,
            finalTargetRotation,
            smoothFactor * Time.deltaTime
        );

        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void ResetRotation()
    {
        targetRotation = centerRotation;
        isReturningToCenter = false;
        inactivityTimer = 0f;
    }

    public void SetRotationSpeed(float speed)
    {
        rotationSpeed = Mathf.Max(0.1f, speed);
    }

    public void SetReturnToCenterEnabled(bool enabled)
    {
        enableReturnToCenter = enabled;
        if (!enabled)
        {
            isReturningToCenter = false;
            inactivityTimer = 0f;
        }
    }

    public void SetReturnDelay(float delay)
    {
        returnToCenterDelay = Mathf.Max(0.1f, delay);
    }

    public void ForceReturnToCenter()
    {
        StartReturnToCenter();
    }

    public float GetInactivityTime()
    {
        return inactivityTimer;
    }

    public bool IsReturningToCenter()
    {
        return isReturningToCenter;
    }
}