using UnityEngine;
using UnityEngine.UI;

public class AddResourceButton : MonoBehaviour
{
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private int amount;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Add);
    }

    public void Add()
    {
        ResourceManager.Instance.AddResource(resourceType, amount);
    }
}
