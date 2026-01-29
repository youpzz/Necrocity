using UnityEngine;

[DefaultExecutionOrder(100)]
public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    [Header("references")]
    [SerializeField] private Camera cam;
    [SerializeField] private Transform baseTarget;
    [SerializeField] private Collider worldBounds;

    [Header("orbit dist")]
    [SerializeField] private float distance = 14f;

    [Header("zoom (fov)")]
    [SerializeField] private float fov = 45f;
    [SerializeField] private float minFov = 25f;
    [SerializeField] private float maxFov = 65f;
    [SerializeField] private float fovZoomSpeed = 0.15f;

    [Header("rotationz")]
    [SerializeField] private float yaw;
    [SerializeField] private float pitch = 35f;
    [SerializeField] private float minPitch = 20f;
    [SerializeField] private float maxPitch = 70f;
    [SerializeField] private float rotationSpeed = 0.2f;

    [Header("pan")]
    [SerializeField] private float panSpeed = 0.004f;
    [SerializeField] private float verticalPanSpeed = 0.004f;
    [SerializeField] private float minVerticalOffset = -2f;
    [SerializeField] private float maxVerticalOffset = 6f;

    [Header("cam limits btw")]
    [SerializeField] private bool useManualLimits = false;
    [SerializeField] private Vector2 xLimits = new(-10f, 10f);
    [SerializeField] private Vector2 zLimits = new(-10f, 10f);
    [SerializeField] private Vector2 yLimits = new(0f, 5f);

    [Header("smooth")]
    [SerializeField] private float positionSmooth = 0.08f;

    private Vector3 centerOffset;
    private float verticalOffset;
    private Vector3 velocity;

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
        // ===== Мобилка еба =====
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
        Vector3 right = cam.transform.right;
        Vector3 forward = cam.transform.forward;

        right.y = 0f;
        forward.y = 0f;

        right.Normalize();
        forward.Normalize();

        centerOffset += (-right * delta.x - forward * delta.y) * panSpeed * distance;

        verticalOffset += delta.y * verticalPanSpeed * distance;
        verticalOffset = Mathf.Clamp(verticalOffset, minVerticalOffset, maxVerticalOffset);

        ClampToBounds();
    }

    void UpdateCamera()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, Time.deltaTime * 10f);

        Vector3 center = baseTarget.position + centerOffset + Vector3.up * verticalOffset;
        Quaternion rot = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 desiredPos = center + rot * Vector3.back * distance;

        desiredPos = ClampPosition(desiredPos);

        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, positionSmooth);
        transform.rotation = rot;
    }

    void ClampValues()
    {
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        fov = Mathf.Clamp(fov, minFov, maxFov);
    }

    void ClampToBounds()
    {
        if (useManualLimits)
        {
            Vector3 worldCenter = baseTarget.position + centerOffset;
            worldCenter.x = Mathf.Clamp(worldCenter.x, xLimits.x, xLimits.y);
            worldCenter.z = Mathf.Clamp(worldCenter.z, zLimits.x, zLimits.y);

            Vector3 local = worldCenter - baseTarget.position;
            centerOffset = new Vector3(local.x, 0f, local.z);
        }
        else if (worldBounds != null)
        {
            Bounds b = worldBounds.bounds;
            Vector3 worldCenter = baseTarget.position + centerOffset;
            worldCenter.x = Mathf.Clamp(worldCenter.x, b.min.x, b.max.x);
            worldCenter.z = Mathf.Clamp(worldCenter.z, b.min.z, b.max.z);
            Vector3 local = worldCenter - baseTarget.position;
            centerOffset = new Vector3(local.x, 0f, local.z);
        }
    }

    Vector3 ClampPosition(Vector3 pos)
    {
        if (useManualLimits)
        {
            pos.x = Mathf.Clamp(pos.x, xLimits.x, xLimits.y);
            pos.y = Mathf.Clamp(pos.y, yLimits.x, yLimits.y);
            pos.z = Mathf.Clamp(pos.z, zLimits.x, zLimits.y);
        }
        else if (worldBounds != null)
        {
            Bounds b = worldBounds.bounds;
            pos.x = Mathf.Clamp(pos.x, b.min.x, b.max.x);
            pos.y = Mathf.Clamp(pos.y, b.min.y, b.max.y);
            pos.z = Mathf.Clamp(pos.z, b.min.z, b.max.z);
        }

        return pos;
    }
}