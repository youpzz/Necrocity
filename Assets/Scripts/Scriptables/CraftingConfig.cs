using System;
using UnityEngine;

[CreateAssetMenu(menuName = "youpzdev/crafting/cfg")]
public class CraftingConfig : ScriptableObject
{
    [SerializeField] private ComponentData[] components;
    [SerializeField] private ItemData[] items;

    private ComponentData[] _componentCache;
    private ItemData[] _itemCache;

    void OnEnable()
    {
        _componentCache = new ComponentData[Enum.GetValues(typeof(ComponentType)).Length];
        _itemCache = new ItemData[Enum.GetValues(typeof(ItemType)).Length];
        foreach (var c in components) _componentCache[(int)c.type] = c;
        foreach (var i in items) _itemCache[(int)i.type] = i;
    }

    public ComponentData GetComponent(ComponentType type) => _componentCache[(int)type];
    public ItemData GetItem(ItemType type) => _itemCache[(int)type];
}