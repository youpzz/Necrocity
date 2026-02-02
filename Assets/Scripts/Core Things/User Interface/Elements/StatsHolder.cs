using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsHolder : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text value;

    public void Init(Sprite sprite, float amount, int limit)
    {
        icon.sprite = sprite;
        string amount_ = amount.ToString("0.##");
        value.text = $"{amount_}/sec | {limit}"; // потом норм сделать прикрутку текста
    }

}
