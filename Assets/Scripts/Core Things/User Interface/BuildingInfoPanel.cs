using TMPro;
using UnityEngine;

public class BuildingInfoPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [Space(5)] [Header("Stats Grid")]
    [SerializeField] private Transform statsGrid;
    [SerializeField] private GameObject statsPrefab;
    [Space(10)] [Header("References")]
    [SerializeField] private UIPanel uiPanel;

    void Start()
    {
        if (!uiPanel) uiPanel = GetComponent<UIPanel>(); // подстраховка еба
    }

    public void Show(BuildingData data)
    {
        Debug.Assert(data != null);

        foreach (Transform obj in statsGrid) Destroy(obj.gameObject);

        titleText.text = data.buildingName;
        foreach (var res in data.gainResources)
        {
            StatsHolder statsHolder = Instantiate(statsPrefab, statsGrid).GetComponent<StatsHolder>();
            int producePerSec = res.gainLimit / res.gainTime;
            statsHolder.Init(UIManager.Instance.GetIcon(res.resourceType), producePerSec);
        }

        uiPanel.Show();
    }

    public void Hide() => uiPanel.Hide();



}
