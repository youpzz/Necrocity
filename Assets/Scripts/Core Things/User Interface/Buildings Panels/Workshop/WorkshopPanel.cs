using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorkshopPanel : MonoBehaviour
{
    [Header("Tabs")]
    [SerializeField] private UIPanel workshopPanel;
    [SerializeField] private UIPanel componentsPanel;
    [SerializeField] private UIPanel laboratoryPanel;
    [SerializeField] private ButtonStyler workshopButton;
    [SerializeField] private ButtonStyler componentsButton;
    [SerializeField] private ButtonStyler laboratoryButton;

    [Header("Workshop")]
    [SerializeField] private Transform workshopGrid;

    [Header("Config")]
    [SerializeField] private CraftingConfig craftingConfig;
    [SerializeField] private IconsConfig iconsConfig;

    private Dictionary<UIPanel, ButtonStyler> _panelButtons;

    private void Awake()
    {
        _panelButtons = new Dictionary<UIPanel, ButtonStyler>
        {
            { workshopPanel,    workshopButton    },
            { componentsPanel,  componentsButton  },
            { laboratoryPanel,  laboratoryButton  },
        };

        foreach (var (panel, button) in _panelButtons)
            button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => OpenPanel(panel));

        OpenPanel(workshopPanel);
    }

    public void OpenPanel(UIPanel panel)
    {
        foreach (var (key, button) in _panelButtons)
        {
            bool isActive = key == panel;
            button.SwitchState(isActive);
            if (isActive) key.Show(); else key.Hide();
        }
    }

    public (Sprite, int)[] BuildIngredients(ItemData.Ingredient[] recipe)
    {
        var result = new (Sprite, int)[recipe.Length];
        for (int i = 0; i < recipe.Length; i++)
        {
            var data = craftingConfig.GetComponentData(recipe[i].component);
            result[i] = (data.icon, recipe[i].amount);
        }
        return result;
    }

    public ItemData[] GetItemDatas => craftingConfig.GetAllItems();
    public ItemData[] GetItemsInInventory => craftingConfig.GetAllItems().Where(item => InventoryManager.Instance.GetItem(item.type) > 0).ToArray();
    public ComponentData GetComponentData(ComponentType component) => craftingConfig.GetComponentData(component);
    public ComponentData[] GetAllComponentDatas => craftingConfig.GetAllComponents();

}