using System;
using UnityEngine;

public class BuildingVisual : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ResourceGainer resourceGainer;

    [Space(15)]
    [SerializeField] private LevelObjects[] levelChanges;

    void Start()
    {
        resourceGainer.onLevelChanged += UpdateVisual;
    }

    void UpdateVisual()
    {
        foreach (var change in levelChanges) if (change.level == resourceGainer.Level) {foreach (var obj in change.buildingParts) {obj.SetActive(true); }};
        // пока так через делаем
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (resourceGainer == null) resourceGainer = GetComponent<ResourceGainer>();
    }
#endif

    [Serializable]
    public class LevelObjects
    {
        public GameObject[] buildingParts;
        public int level;
    }

}
