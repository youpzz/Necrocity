using UnityEngine;

[DefaultExecutionOrder(100)]
public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Camera cam;
    [SerializeField] private Transform baseTarget;
    [SerializeField] private Collider worldBounds;

    [Header("Orbit Settings")]
    [SerializeField] private float distance = 14f;

    [Header("Zoom (FOV)")]
    [SerializeField] private float fov = 45f;
    [SerializeField] private float minFov = 25f;
    [SerializeField] private float maxFov = 65f;
    [SerializeField] private float fovZoomSpeed = 0.15f;

    [Header("Rotation")]
    [SerializeField] private float yaw;
    [SerializeField] private float pitch = 35f;

    [Header("Yaw Limits")]
    [SerializeField] private bool useYawLimits = false;
    [SerializeField] private float minYaw = -60f;
    [SerializeField] private float maxYaw = 60f;
    [Header("Pitch Limits")]
    [SerializeField] private float minPitch = 20f;
    [SerializeField] private float maxPitch = 70f;
    [SerializeField] private float rotationSpeed = 0.2f;

    [Header("Pan")]
    [SerializeField] private float panSpeed = 0.004f;
    [SerializeField] private float verticalPanSpeed = 0.004f;
    [SerializeField] private float minVerticalOffset = -2f;
    [SerializeField] private float maxVerticalOffset = 6f;

    [Header("Limits")]
    [SerializeField] private bool useManualLimits = false;
    [SerializeField] private Vector2 xLimits = new(-10f, 10f);
    [SerializeField] private Vector2 zLimits = new(-10f, 10f);
    [SerializeField] private Vector2 yLimits = new(0f, 5f);

    [Header("Smooth")]
    [SerializeField] private float positionSmooth = 0.08f;

    private Vector3 centerOffset;
    private float verticalOffset;
    private Vector3 velocity;
    private Vector3 initialOffset;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (cam == null) cam = Camera.main;
        cam.fieldOfView = fov;

        initialOffset = transform.position - baseTarget.position;
        distance = initialOffset.magnitude;
    }

    void Update()
    {
        HandleInput();
        ClampValues();
    }

    void LateUpdate()
    {
        UpdateCamera();
    }

    private void HandleInput()
    {
        if (UIManager.Instance.AreModalWindowOpened()) return; // чтоб игрок не выёбывался особо, нехуй двигаться пока открыты панели
        
        // Мобилка еба
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Moved)
            {
                yaw += t.deltaPosition.x * rotationSpeed;
                pitch -= t.deltaPosition.y * rotationSpeed;
            }
        }
        else if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            float prevDist = Vector2.Distance(t0.position - t0.deltaPosition, t1.position - t1.deltaPosition);
            float currDist = Vector2.Distance(t0.position, t1.position);

            fov += (prevDist - currDist) * fovZoomSpeed;

            Vector2 avgDelta = (t0.deltaPosition + t1.deltaPosition) * 0.5f;
            ApplyPan(avgDelta);
        }

#if UNITY_EDITOR || UNITY_STANDALONE
        // ПК мышка
        if (Input.GetMouseButton(0))
        {
            yaw += Input.GetAxis("Mouse X") * rotationSpeed * 50f;
            pitch -= Input.GetAxis("Mouse Y") * rotationSpeed * 50f;
        }

        if (Input.GetMouseButton(2))
        {
            Vector2 delta = new(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            ApplyPan(delta * 10f);
        }

        float wheel = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(wheel) > 0.0001f) fov -= wheel * 20f;
#endif
    }

    void ApplyPan(Vector2 delta)
    {
        Vector3 localRight = cam.transform.right;
        localRight.y = 0f;
        localRight.Normalize();
        centerOffset += localRight * delta.x * panSpeed * distance;

        verticalOffset += delta.y * verticalPanSpeed * distance;
        verticalOffset = Mathf.Clamp(verticalOffset, minVerticalOffset, maxVerticalOffset);

        ClampToBounds();
    }

    void UpdateCamera()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, Time.deltaTime * 10f);

        Vector3 center = baseTarget.position + centerOffset + Vector3.up * verticalOffset;

        Quaternion rot = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 desiredPos = center + rot * -Vector3.forward * distance;

        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, positionSmooth);
        transform.rotation = rot;
    }

    void ClampValues()
    {
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        fov = Mathf.Clamp(fov, minFov, maxFov);

        if (useYawLimits)
        {
            yaw = Mathf.Clamp(yaw, minYaw, maxYaw);
        }
    }

    void ClampToBounds()
    {
        Vector3 worldPos = baseTarget.position + centerOffset + Vector3.up * verticalOffset;

        if (useManualLimits)
        {
            worldPos.x = Mathf.Clamp(worldPos.x, xLimits.x, xLimits.y);
            worldPos.y = Mathf.Clamp(worldPos.y, yLimits.x, yLimits.y);
            worldPos.z = baseTarget.position.z; // фикс Z
        }
        else if (worldBounds != null)
        {
            Bounds b = worldBounds.bounds;
            worldPos.x = Mathf.Clamp(worldPos.x, b.min.x, b.max.x);
            worldPos.y = Mathf.Clamp(worldPos.y, b.min.y, b.max.y);
            worldPos.z = baseTarget.position.z;
        }

        centerOffset = new Vector3(worldPos.x - baseTarget.position.x, 0f, 0f);
        verticalOffset = worldPos.y - baseTarget.position.y;
    }
}