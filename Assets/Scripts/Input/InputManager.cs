using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [SerializeField] private InputActionAsset actions;

    // ── Events ───────────────────────────────────────────────
    public event Action<Vector2> OnLook;
    public event Action<Vector2> OnPan;
    public event Action<float> OnScroll;
    public event Action<float> OnPinch;
    public event Action<bool> OnRotateChanged;
    public event Action<bool> OnMiddleMouseChanged;

    // ── Read-only state ──────────────────────────────────────
    public Vector2 MouseDelta { get; private set; }
    public bool RotateHeld { get; private set; }
    public bool MiddleMouseHeld { get; private set; }

    // ── Private ──────────────────────────────────────────────
    private InputActionMap _map;

    private InputAction _lookAction;
    private InputAction _panAction;
    private InputAction _scrollAction;
    private InputAction _pinchAction;
    private InputAction _rotateAction;
    private InputAction _middleMouseAction;

    // ────────────────────────────────────────────────────────

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        _map = actions.FindActionMap("Camera", throwIfNotFound: true);

        _lookAction = _map.FindAction("Look", throwIfNotFound: true);
        _panAction = _map.FindAction("Pan", throwIfNotFound: true);
        _scrollAction = _map.FindAction("Scroll", throwIfNotFound: true);
        _pinchAction = _map.FindAction("Pinch", throwIfNotFound: true);
        _rotateAction = _map.FindAction("Rotate", throwIfNotFound: true);
        _middleMouseAction = _map.FindAction("MiddleMouse", throwIfNotFound: true);
    }

    void OnEnable()
    {
        _lookAction.performed += OnLookPerformed;
        _panAction.performed += OnPanPerformed;
        _scrollAction.performed += OnScrollPerformed;
        _pinchAction.performed += OnPinchPerformed;
        _rotateAction.started += OnRotateStarted;
        _rotateAction.canceled += OnRotateCanceled;
        _middleMouseAction.started += OnMiddleStarted;
        _middleMouseAction.canceled += OnMiddleCanceled;

        _map.Enable();
    }

    void OnDisable()
    {
        _lookAction.performed -= OnLookPerformed;
        _panAction.performed -= OnPanPerformed;
        _scrollAction.performed -= OnScrollPerformed;
        _pinchAction.performed -= OnPinchPerformed;
        _rotateAction.started -= OnRotateStarted;
        _rotateAction.canceled -= OnRotateCanceled;
        _middleMouseAction.started -= OnMiddleStarted;
        _middleMouseAction.canceled -= OnMiddleCanceled;

        _map.Disable();
    }

    // ── Callbacks ────────────────────────────────────────────

    private void OnLookPerformed(InputAction.CallbackContext ctx)
    {
        MouseDelta = ctx.ReadValue<Vector2>();
        OnLook?.Invoke(MouseDelta);
    }

    private void OnPanPerformed(InputAction.CallbackContext ctx)
    {
        OnPan?.Invoke(ctx.ReadValue<Vector2>());
    }

    private void OnScrollPerformed(InputAction.CallbackContext ctx)
    {
        float v = ctx.ReadValue<float>();
        if (v != 0f) OnScroll?.Invoke(v);
    }

    private void OnPinchPerformed(InputAction.CallbackContext ctx)
    {
        OnPinch?.Invoke(ctx.ReadValue<float>());
    }

    private void OnRotateStarted(InputAction.CallbackContext ctx)
    {
        RotateHeld = true;
        OnRotateChanged?.Invoke(true);
    }

    private void OnRotateCanceled(InputAction.CallbackContext ctx)
    {
        RotateHeld = false;
        OnRotateChanged?.Invoke(false);
    }

    private void OnMiddleStarted(InputAction.CallbackContext ctx)
    {
        MiddleMouseHeld = true;
        OnMiddleMouseChanged?.Invoke(true);
    }

    private void OnMiddleCanceled(InputAction.CallbackContext ctx)
    {
        MiddleMouseHeld = false;
        OnMiddleMouseChanged?.Invoke(false);
    }
}
