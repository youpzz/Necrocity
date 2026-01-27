using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }
    public Action onResourcesChanged; // делаем заебатое событие, чтоб не ковырять каждый раз этот скрипт, а то будет какашко
    [SerializeField] private List<ResourceSlot> resourceSlots = new List<ResourceSlot>();
    

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public int GetResourceAmount(ResourceType resourceType)
    {
        ResourceSlot slot = resourceSlots.Find(rs => rs.ResourceType == resourceType);
        return slot != null ? slot.Amount : 0;
    }

    public void AddResource(ResourceType resourceType, int amount)
    {
        ResourceSlot slot = resourceSlots.Find(rs => rs.ResourceType == resourceType);
        if (slot != null)
        {
            slot.AddAmount(amount);
        }
        else
        {
            resourceSlots.Add(new ResourceSlot(resourceType, amount));
        }

        onResourcesChanged?.Invoke();
    }    

    public bool SpendResource(ResourceType resourceType, int amount)
    {
        ResourceSlot slot = resourceSlots.Find(rs => rs.ResourceType == resourceType);
        if (slot != null && slot.TrySpend(amount))
        {
            onResourcesChanged?.Invoke();
            return true;
        }

        return false;
    }



}