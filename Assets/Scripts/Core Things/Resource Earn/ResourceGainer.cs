using System;
using UnityEngine;

public class ResourceGainer : MonoBehaviour
{
    public Action onResourcesChanged;

    [SerializeField] private BuildingData buildingData;

    private int gainLimit;
    private int gainTime;
    private int gainAmount = 0; // количество текущее
    private bool isProducing = true;
    private float produceTimer = 0;

    // ссылки
    public int GainLimit => buildingData.gainResources[0].gainLimit;
    public int GainAmount => gainAmount;

    void Awake()
    {
        gainLimit = buildingData.gainResources[0].gainLimit;
        gainTime = buildingData.gainResources[0].gainTime;
    }

    void Update()
    {
        HandleProducing();
    }

    void HandleProducing()
    {
        if (isProducing)
        {
            produceTimer += Time.deltaTime;
            if (produceTimer >= 1f)
            {
                produceTimer = 0;
                AddProgress();
            }
        }
    }

    void Reset()
    {
        produceTimer = 0f;
        gainAmount = 0;
        isProducing = true;
        onResourcesChanged?.Invoke();
    }

    void AddProgress()
    {
        int producePerSec = gainLimit / gainTime;
        gainAmount += producePerSec;
        onResourcesChanged?.Invoke();

        if (gainAmount >= gainLimit)
        {
            gainAmount = gainLimit;
            isProducing = false;
        }
    }

    public void Redeem()
    {
        if (gainAmount <= 0) return;

        ResourceManager.Instance.AddResource(buildingData.gainResources[0].resourceType, gainAmount);
        Reset();
    }

    void OnMouseDown()
    {
        UIManager.Instance.ShowBuildingInfo(buildingData); // тут панель должна открыться с улучшениями
    }

}
