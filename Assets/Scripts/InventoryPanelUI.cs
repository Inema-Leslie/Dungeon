using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class InventoryPanelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inventoryText;

    // Maps internal item IDs to friendly display names.
    private static readonly Dictionary<string, string> DisplayNames = new Dictionary<string, string>
    {
        { "Weapon", "Axe" },
        { "HealPotion", "Healing Potion" },
    };

    private void Update()
    {
        if (PlayerInventory.Instance == null || inventoryText == null) return;

        var sorted = InventorySorter.SortAlphabetically(PlayerInventory.Instance.CollectedItems);

        if (sorted.Count == 0)
        {
            inventoryText.text = "(empty)";
            return;
        }

        List<string> displayList = new List<string>();
        foreach (string id in sorted)
        {
            displayList.Add(DisplayNames.TryGetValue(id, out string friendlyName) ? friendlyName : id);
        }

        inventoryText.text = string.Join("\n", displayList);
    }
}