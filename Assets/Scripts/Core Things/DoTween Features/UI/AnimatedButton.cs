using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class AnimatedButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float _scaleFactor = 0.8f;
    [SerializeField] private float _duration = 0.25f;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (button == null || !button.interactable) return;

        transform.DOKill();
        transform.DOScale(_scaleFactor, _duration).SetEase(Ease.OutQuad);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (button == null || !button.interactable) return;

        transform.DOKill();
        transform.DOScale(1f, _duration).SetEase(Ease.OutBack);
    }

    public void PlayClickAnimation()
    {
        // if (button != null && !button.interactable) return;

        transform.DOKill();
        transform.DOScale(_scaleFactor, _duration / 2).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            transform.DOScale(1f, _duration / 2).SetEase(Ease.OutBack);
        });
    }
}