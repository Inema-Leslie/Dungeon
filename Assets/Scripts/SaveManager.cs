using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

#if UNITY_WEBGL && !UNITY_EDITOR
 
    [DllImport("__Internal")]
    private static extern void SyncFiles();
#endif

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
        Debug.Log($"Game saved to {SavePath}");

#if UNITY_WEBGL && !UNITY_EDITOR
        SyncFiles(); 
#endif
    }

    public SaveData LoadGame()
    {
        if (!File.Exists(SavePath)) return null;

        string json = File.ReadAllText(SavePath);
        return JsonUtility.FromJson<SaveData>(json);
    }

    public void SaveLevelStatus(int[] levelStatus)
    {
        SaveData data = LoadGame() ?? new SaveData();
        data.levelStatus = levelStatus;
        SaveGame(data);
    }

    public void SaveCurrentLevel(int levelIndex)
    {
        SaveData data = LoadGame() ?? new SaveData();
        data.currentLevel = levelIndex;
        SaveGame(data);
    }

    public void SavePlayerState(float health, System.Collections.Generic.List<string> inventory)
    {
        SaveData data = LoadGame() ?? new SaveData();
        data.playerHealth = health;
        data.inventory = inventory;
        SaveGame(data);
    }

    public bool HasSaveFile() => File.Exists(SavePath);

    public void DeleteSave()
    {
        if (File.Exists(SavePath)) File.Delete(SavePath);

#if UNITY_WEBGL && !UNITY_EDITOR
        SyncFiles();
#endif
    }

    public void SaveInventory(System.Collections.Generic.List<string> inventory)
    {
        SaveData data = LoadGame() ?? new SaveData();
        data.inventory = inventory;
        SaveGame(data);
    }
}