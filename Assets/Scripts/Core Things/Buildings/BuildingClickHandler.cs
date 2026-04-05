using UnityEngine;

public class BuildingClickHandler : MonoBehaviour
{
    [SerializeField] private ResourceGainer resourceGainer;

    private void OnMouseDown()
    {
        UIManager.Instance.ShowBuildingInfo(resourceGainer);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (resourceGainer == null) resourceGainer = GetComponentInParent<ResourceGainer>();
    }
#endif
}