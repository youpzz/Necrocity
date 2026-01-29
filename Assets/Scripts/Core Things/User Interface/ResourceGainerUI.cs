using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ResourceGainerUI : MonoBehaviour
{
    [SerializeField] private Image slider;
    [SerializeField] private TMP_Text sliderText;
    [SerializeField] private float tweenDuration = 0.5f;
    private ResourceGainer resourceGainer;
    private float currentFill;

    void Awake()
    {
        resourceGainer = GetComponentInParent<ResourceGainer>();
    }

    void LateUpdate()
    {
        HandleUI();
    }

    void HandleUI()
    {
        if (!slider || !resourceGainer || !sliderText) { this.enabled = false; return;}
        sliderText.text = $"{resourceGainer.EarnAmount}/{resourceGainer.EarnLimit}";
        float targetFill = Mathf.Clamp01((float)resourceGainer.EarnAmount / resourceGainer.EarnLimit);

        if (!Mathf.Approximately(currentFill, targetFill))
        {
            DOTween.Kill(slider);
            slider.DOFillAmount(targetFill, tweenDuration).SetEase(Ease.OutCubic);
            currentFill = targetFill;
        }
    }


}
