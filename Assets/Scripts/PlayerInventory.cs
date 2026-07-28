using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    public List<string> CollectedItems { get; private set; } = new List<string>();
    public Dictionary<string, int> ItemCharges { get; private set; } = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadFromSave();
    }

    private void OnEnable() => GameEvents.OnItemCollected += HandleItemCollected;
    private void OnDisable() => GameEvents.OnItemCollected -= HandleItemCollected;

    private void HandleItemCollected(string itemId)
    {
        if (!CollectedItems.Contains(itemId))
        {
            CollectedItems.Add(itemId);
        }
        SaveManager.Instance?.SaveInventory(CollectedItems);
    }

    public bool HasItem(string itemId) => CollectedItems.Contains(itemId);

    // --- Charge-based items (e.g. Heal Potions) ---

    public bool HasCharge(string itemId) => ItemCharges.ContainsKey(itemId) && ItemCharges[itemId] > 0;

    public void AddCharge(string itemId, int amount = 1)
    {
        if (!ItemCharges.ContainsKey(itemId))
            ItemCharges[itemId] = 0;

        ItemCharges[itemId] += amount;
    }

    public void ConsumeCharge(string itemId)
    {
        if (ItemCharges.ContainsKey(itemId) && ItemCharges[itemId] > 0)
        {
            ItemCharges[itemId]--;
        }
    }

    private void LoadFromSave()
    {
        SaveData data = SaveManager.Instance?.LoadGame();
        if (data != null && data.inventory != null)
        {
            CollectedItems = new List<string>(data.inventory);
        }
    }
}