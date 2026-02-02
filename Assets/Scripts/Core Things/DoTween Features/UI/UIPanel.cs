using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class UIPanel : MonoBehaviour
{
    public enum PanelAnimationType
    {
        FadeScale,
        SlideFromBottom,
        SlideFromTop,
        SlideFromLeft,
        SlideFromRight,
        PopBounce,
        FadeOnly
    }

    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _content;
    [SerializeField] private float _duration = 0.4f;
    [SerializeField] private PanelAnimationType _animationType = PanelAnimationType.FadeScale;

    private Vector2 _originalAnchoredPos;

    private const string TweenId = "UIPanelTween";

    void Awake()
    {
        _originalAnchoredPos = _content.anchoredPosition;
    }

    public void Show()
    {
        DOTween.Kill(TweenId);

        gameObject.SetActive(true);

        _canvasGroup.alpha = 0;
        _content.localScale = Vector3.one;
        _content.anchoredPosition = _originalAnchoredPos;

        switch (_animationType)
        {
            case PanelAnimationType.FadeScale:
                PlayFadeScaleShow();
                break;

            case PanelAnimationType.SlideFromBottom:
                PlaySlideShow(new Vector2(0, -Screen.height));
                break;

            case PanelAnimationType.SlideFromTop:
                PlaySlideShow(new Vector2(0, Screen.height));
                break;

            case PanelAnimationType.SlideFromLeft:
                PlaySlideShow(new Vector2(-Screen.width, 0));
                break;

            case PanelAnimationType.SlideFromRight:
                PlaySlideShow(new Vector2(Screen.width, 0));
                break;

            case PanelAnimationType.PopBounce:
                PlayPopBounceShow();
                break;

            case PanelAnimationType.FadeOnly:
                PlayFadeOnlyShow();
                break;
        }
    }

    public void Hide()
    {
        DOTween.Kill(TweenId);

        switch (_animationType)
        {
            case PanelAnimationType.FadeScale:
                PlayFadeScaleHide();
                break;

            case PanelAnimationType.SlideFromBottom:
                PlaySlideHide(new Vector2(0, -Screen.height));
                break;

            case PanelAnimationType.SlideFromTop:
                PlaySlideHide(new Vector2(0, Screen.height));
                break;

            case PanelAnimationType.SlideFromLeft:
                PlaySlideHide(new Vector2(-Screen.width, 0));
                break;

            case PanelAnimationType.SlideFromRight:
                PlaySlideHide(new Vector2(Screen.width, 0));
                break;

            case PanelAnimationType.PopBounce:
                PlayPopBounceHide();
                break;

            case PanelAnimationType.FadeOnly:
                PlayFadeOnlyHide();
                break;
        }
    }

    // ========== Анимки ебать ==========

    void PlayFadeScaleShow()
    {
        _canvasGroup.DOFade(1f, _duration).SetUpdate(true).SetId(TweenId);
        _content.localScale = Vector3.one * 0.8f;
        _content.DOScale(1f, _duration).SetEase(Ease.OutBack).SetUpdate(true).SetId(TweenId);
    }

    void PlayFadeScaleHide()
    {
        _canvasGroup.DOFade(0f, _duration).SetUpdate(true).SetId(TweenId);
        _content.DOScale(0.8f, _duration).SetEase(Ease.InBack).SetUpdate(true).SetId(TweenId)
            .OnComplete(() => gameObject.SetActive(false));
    }

    void PlaySlideShow(Vector2 offset)
    {
        _canvasGroup.DOFade(1f, _duration).SetUpdate(true).SetId(TweenId);

        _content.anchoredPosition = _originalAnchoredPos + offset;
        _content.DOAnchorPos(_originalAnchoredPos, _duration)
            .SetEase(Ease.OutCubic)
            .SetUpdate(true)
            .SetId(TweenId);
    }

    void PlaySlideHide(Vector2 offset)
    {
        _canvasGroup.DOFade(0f, _duration).SetUpdate(true).SetId(TweenId);

        _content.DOAnchorPos(_originalAnchoredPos + offset, _duration)
            .SetEase(Ease.InCubic)
            .SetUpdate(true)
            .SetId(TweenId)
            .OnComplete(() => gameObject.SetActive(false));
    }

    void PlayPopBounceShow()
    {
        _canvasGroup.DOFade(1f, _duration * 0.7f).SetUpdate(true).SetId(TweenId);

        _content.localScale = Vector3.one * 0.5f;
        _content.DOScale(1f, _duration)
            .SetEase(Ease.OutElastic, 1f, 0.3f)
            .SetUpdate(true)
            .SetId(TweenId);
    }

    void PlayPopBounceHide()
    {
        _canvasGroup.DOFade(0f, _duration * 0.6f).SetUpdate(true).SetId(TweenId);

        _content.DOScale(0.5f, _duration * 0.6f)
            .SetEase(Ease.InBack)
            .SetUpdate(true)
            .SetId(TweenId)
            .OnComplete(() => gameObject.SetActive(false));
    }

    void PlayFadeOnlyShow()
    {
        _canvasGroup.DOFade(1f, _duration).SetUpdate(true).SetId(TweenId);
    }

    void PlayFadeOnlyHide()
    {
        _canvasGroup.DOFade(0f, _duration).SetUpdate(true).SetId(TweenId)
            .OnComplete(() => gameObject.SetActive(false));
    }

}
