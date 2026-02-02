using UnityEngine;

public class ResourceGainer : MonoBehaviour
{
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private int earnSpeed = 5; // скорость дохода, то, за сколько будет произведён максимум ресурса. (В секундах)
    [SerializeField] private int earnLimit = 100; // лимит по доходу, как в тз

    private int earnAmount = 0; // количество текущее
    private bool isProducing = true;
    private float produceTimer = 0;

    public int EarnLimit => earnLimit;
    public int EarnAmount => earnAmount;

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
        earnAmount = 0;
        isProducing = true;
    }

    void AddProgress()
    {
        int producePerSec = earnLimit / earnSpeed;
        earnAmount += producePerSec;

        if (earnAmount >= earnLimit)
        {
            earnAmount = earnLimit;
            isProducing = false;
        }
    }

    public void Redeem()
    {
        if (earnAmount <= 0) return;

        ResourceManager.Instance.AddResource(resourceType, earnAmount);
        Reset();
    }

    void OnMouseDown()
    {
        // тут панель должна открыться с улучшениями
    }

}
