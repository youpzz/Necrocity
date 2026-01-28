using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text dublonsText;
    [SerializeField] private TMP_Text loveText;

    void Start()
    {
        ResourceManager.Instance.onResourcesChanged += UpdateResourcesText;
    }

    void UpdateResourcesText()
    {
        dublonsText.text = $"{ResourceManager.Instance.GetResourceAmount(ResourceType.Dublons)}";
        loveText.text = $"{ResourceManager.Instance.GetResourceAmount(ResourceType.Love)}";
    }


}
