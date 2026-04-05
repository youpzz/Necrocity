using UnityEngine;

public class Workshop : MonoBehaviour, IClickableBuilding
{
    

    public void OnClick()
    {
        UIManager.Instance.ShowWorkshopPanel();
    }
}
