using System;

[Serializable]
public class ResourceSlot
{
    private ResourceType resourceType;
    private int amount;

    public ResourceType ResourceType => resourceType;
    public int Amount => amount;

    public ResourceSlot(ResourceType resourceType, int amount)
    {
        this.resourceType = resourceType;
        this.amount = amount;
    }

    public void AddAmount(int amount_)
    {
        amount += amount_;
    }

    public void SetAmount(int amount_) => amount = amount_;

    public bool TrySpend(int amount_)
    {
        if (amount >= amount_) {amount -= amount_; return true;}
        return false;
    }

    
}
