using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    public List<string> CollectedItems { get; private set; } = new List<string>();

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

    private void LoadFromSave()
    {
        SaveData data = SaveManager.Instance?.LoadGame();
        if (data != null && data.inventory != null)
        {
            CollectedItems = new List<string>(data.inventory);
        }
    }
}