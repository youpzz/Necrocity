using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsHolder : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text value;

    public void Init(Sprite sprite, int amount)
    {
        icon.sprite = sprite;
        value.text = $"{amount}/sec"; // потом норм сделать прикрутку текста
    }

}
