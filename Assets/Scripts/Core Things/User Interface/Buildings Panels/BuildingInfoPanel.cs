using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInfoPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text upgradeTitleText;
    [Space(5)]
    [Header("Stats Grid")]
    [SerializeField] private Transform statsGrid;
    [SerializeField] private Transform upgradeGrid;
    [SerializeField] private GameObject statsPrefab;

    [Space(10)]
    [Header("References")]
    [SerializeField] private UIPanel uiPanel;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TMP_Text upgradeButtonText;

    private BuildingData currentData;
    private ResourceGainer currentGainer;
    private int level;

    void Start()
    {
        if (!uiPanel) uiPanel = GetComponent<UIPanel>();
        upgradeButton.onClick.AddListener(UpgradeBuilding);
    }

    public void Show(ResourceGainer gainer)
    {
        Debug.Assert(gainer != null);

        currentData = gainer.Data;
        currentGainer = gainer;
        level = gainer.Level;

        foreach (Transform obj in statsGrid) Pooling.Destroy(obj.gameObject);
        foreach (Transform obj in upgradeGrid) Pooling.Destroy(obj.gameObject);

        titleText.text = currentData.buildingName + $" | Level {level}";
        upgradeTitleText.text = "";
        upgradeButtonText.text = "Max Level";

        GainResources targetSlot = currentData.gainResources[level - 1];

        StatsHolder statsHolder = Pooling.Instantiate(statsPrefab, statsGrid).GetComponent<StatsHolder>();
        float producePerSec = (float)targetSlot.gainLimit / targetSlot.gainTime;
        statsHolder.Init(UIManager.Instance.GetIcon(targetSlot.resourceType), producePerSec, targetSlot.gainLimit);

        upgradeButton.interactable = level < currentData.gainResources.Length;
        if (level < currentData.gainResources.Length)
        {
            targetSlot = currentData.gainResources[level];

            upgradeButton.interactable = ResourceManager.Instance.CanSpendResource(targetSlot.upgradePrice.ResourceType, targetSlot.upgradePrice.Amount);
            upgradeButtonText.text = $"Upgrade\n {targetSlot.upgradePrice.Amount} {targetSlot.upgradePrice.ResourceType}";
            upgradeTitleText.text = $"Level {level + 1}";

            StatsHolder updateHolder = Pooling.Instantiate(statsPrefab, upgradeGrid).GetComponent<StatsHolder>();
            producePerSec = (float)targetSlot.gainLimit / targetSlot.gainTime;
            updateHolder.Init(UIManager.Instance.GetIcon(targetSlot.resourceType), producePerSec, targetSlot.gainLimit);
        }

        uiPanel.Show();
    }

    public void Hide()
    {
        currentData = null;
        uiPanel.Hide();
    }

    void UpgradeBuilding()
    {
        GainResources target = currentData.gainResources[level];
        if (!ResourceManager.Instance.CanSpendResource(target.upgradePrice.ResourceType, target.upgradePrice.Amount)) return;

        ResourceManager.Instance.SpendResource(target.upgradePrice.ResourceType, target.upgradePrice.Amount);

        currentGainer.AddLevel();
        Show(currentGainer);
    }
}