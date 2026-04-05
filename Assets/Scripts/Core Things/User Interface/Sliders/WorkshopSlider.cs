using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WorkshopSlider : MonoBehaviour
{
    [SerializeField] private Image slider;
    [SerializeField] private float tweenDuration = 0.5f;
    [Space(5)]
    [SerializeField] private Image componentIcon;
    [SerializeField] private GameObject readyIcon;
    [Tooltip("Если пусто — ищется в сцене при старте (префаб не в иерархии панели).")]
    [SerializeField] private WorkshopPanel workshopPanel;
    [SerializeField] private Button openWorkshopButton;

    private float currentFill;

    private void Awake()
    {
        if (workshopPanel == null) workshopPanel = FindFirstObjectByType<WorkshopPanel>(FindObjectsInactive.Include);
    }

    private void Start()
    {
        if (workshopPanel == null) workshopPanel = FindFirstObjectByType<WorkshopPanel>(FindObjectsInactive.Include);

        CraftingQueue.Instance.RestoreCurrentData(workshopPanel);

        CraftingQueue.Instance.OnCraftStarted += OnCraftStarted;
        CraftingQueue.Instance.OnCraftCompleted += OnCraftCompleted;
        CraftingQueue.Instance.OnCraftCancelled += OnCraftCancelled;
        if (openWorkshopButton) openWorkshopButton.onClick.AddListener(OnOpenWorkshopClicked);

        UpdateUI();
    }

    private void OnDestroy()
    {
        if (openWorkshopButton) openWorkshopButton.onClick.RemoveListener(OnOpenWorkshopClicked);

        if (CraftingQueue.Instance == null) return;
        CraftingQueue.Instance.OnCraftStarted -= OnCraftStarted;
        CraftingQueue.Instance.OnCraftCompleted -= OnCraftCompleted;
        CraftingQueue.Instance.OnCraftCancelled -= OnCraftCancelled;
    }

    private static void OnOpenWorkshopClicked()
    {
        if (UIManager.Instance != null) UIManager.Instance.ShowWorkshopPanel();

    }

    private void Update()
    {
        if (!CraftingQueue.Instance.IsActive) return;
        UpdateFill(CraftingQueue.Instance.Progress);
    }

    private void OnCraftStarted() => UpdateUI();

    private void OnCraftCompleted(ComponentType _) => UpdateUI();

    private void OnCraftCancelled() => UpdateUI();

    private void UpdateUI()
    {
        var q = CraftingQueue.Instance;
        bool show = q.IsActive || q.IsReadyToCollect;
        if (!show)
        {
            if (slider) DOTween.Kill(slider);
            currentFill = 0f;
            gameObject.SetActive(false);
            return;
        }

        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        bool ready = q.IsReadyToCollect;
        if (readyIcon) readyIcon.SetActive(ready);

        Sprite icon = q.CurrentData?.icon ?? GetIcon(q.CurrentType);
        SetIcon(icon);

        float fillTarget = ready ? 1f : q.Progress;
        UpdateFill(fillTarget);
    }

    private void SetIcon(Sprite sprite)
    {
        if (!componentIcon) return;
        componentIcon.sprite = sprite;
        componentIcon.enabled = sprite != null;
    }

    private Sprite GetIcon(ComponentType type)
    {
        if (workshopPanel == null) return null;
        return workshopPanel.GetComponentData(type)?.icon;
    }

    private void UpdateFill(float target)
    {
        if (!slider || Mathf.Approximately(currentFill, target)) return;

        DOTween.Kill(slider);
        slider.DOFillAmount(target, tweenDuration).SetEase(Ease.OutCubic);
        currentFill = target;
    }
}