using System;
using UnityEngine;

public class ResourceIconsConfig : MonoBehaviour
{
    [Serializable]
    private struct Entry
    {
        public ResourceType type;
        public Sprite icon;
    }

    [SerializeField] private Entry[] entries;

    private Sprite[] _cache;

    void Awake()
    {
        _cache = new Sprite[Enum.GetValues(typeof(ResourceType)).Length];
        foreach (var obj in entries) _cache[(int)obj.type] = obj.icon;
    }

    public Sprite GetIcon(ResourceType type) => _cache[(int)type];
}
