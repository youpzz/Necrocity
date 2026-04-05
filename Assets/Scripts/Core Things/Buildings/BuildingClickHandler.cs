using UnityEngine;

public class BuildingClickHandler : MonoBehaviour
{
    private IClickableBuilding building;

    private void Awake()
    {
        building = GetComponentInParent<IClickableBuilding>();
    }

    private void OnMouseDown()
    {
        building?.OnClick();
    }
}