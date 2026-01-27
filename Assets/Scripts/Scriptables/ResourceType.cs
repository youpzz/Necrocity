using System.Security;
using UnityEngine;

[CreateAssetMenu(fileName = "New Resource Type", menuName = "youpzdev/Resource Type")]
public class ResourceType : ScriptableObject
{
    [SerializeField] private string id;
    [SerializeField] private Sprite icon;

    public string Id => id;
    public Sprite Icon => icon;
}
