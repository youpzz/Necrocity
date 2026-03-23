using System;
using UnityEngine;

public class IconsConfig : MonoBehaviour
{
    [Serializable]
    private struct ResourceEntry { public ResourceType type; public Sprite icon; }

    [SerializeField] private ResourceEntry[] resources;
    private Sprite[] _resourceCache;

    void Awake()
    {
        _resourceCache = new Sprite[Enum.GetValues(typeof(ResourceType)).Length];
        foreach (var e in resources) _resourceCache[(int)e.type] = e.icon;
    }

    public Sprite GetIcon(ResourceType type) => _resourceCache[(int)type];
}