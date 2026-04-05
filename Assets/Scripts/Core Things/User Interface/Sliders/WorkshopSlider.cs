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

    private float currentFill;

    private void Start()
    {
        CraftingQueue.Instance.OnCraftStarted += OnCraftStarted;
        CraftingQueue.Instance.OnCraftCompleted += OnCraftCompleted;
        CraftingQueue.Instance.OnCraftCancelled += OnCraftCancelled;
        UpdateUI();
    }

    private void Update()
    {
        if (!CraftingQueue.Instance.IsActive) return;
        UpdateFill(CraftingQueue.Instance.Progress);
    }

    private void OnCraftStarted()
    {
        readyIcon.SetActive(false);
        var data = CraftingQueue.Instance.CurrentData;
        if (componentIcon && data != null) componentIcon.sprite = data.icon;
        UpdateFill(0f);
    }

    private void OnCraftCompleted(ComponentType _)
    {
        readyIcon.SetActive(true);
        UpdateFill(1f);
    }

    private void OnCraftCancelled()
    {
        readyIcon.SetActive(false);
        if (componentIcon) componentIcon.sprite = null;
        UpdateFill(0f);
    }

    private void UpdateUI()
    {
        bool completed = !CraftingQueue.Instance.IsActive && readyIcon.activeSelf;
        readyIcon.SetActive(completed);

        var data = CraftingQueue.Instance.CurrentData;
        if (componentIcon) componentIcon.sprite = data != null ? data.icon : null;

        UpdateFill(CraftingQueue.Instance.Progress);
    }

    private void UpdateFill(float target)
    {
        if (Mathf.Approximately(currentFill, target)) return;

        DOTween.Kill(slider);
        slider.DOFillAmount(target, tweenDuration).SetEase(Ease.OutCubic);
        currentFill = target;
    }
}