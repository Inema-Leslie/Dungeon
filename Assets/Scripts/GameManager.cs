using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Level Progression")]
    [SerializeField] private int totalLevels = 5;
    private int[] levelStatus; 

    public int CurrentLevelIndex { get; private set; } = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeLevelStatus();
    }

    private void InitializeLevelStatus()
    {
        levelStatus = new int[totalLevels];

        SaveData data = SaveManager.Instance != null ? SaveManager.Instance.LoadGame() : null;

        if (data != null && data.levelStatus != null && data.levelStatus.Length == totalLevels)
        {
            levelStatus = data.levelStatus;
            CurrentLevelIndex = data.currentLevel;
        }
        else
        {
            levelStatus[0] = 1; 
            for (int i = 1; i < totalLevels; i++) levelStatus[i] = 0;
        }
    }

    public int GetLevelStatus(int levelIndex) => levelStatus[levelIndex];

    public bool IsLevelUnlocked(int levelIndex) => levelStatus[levelIndex] >= 1;

    public void CompleteLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= totalLevels) return;

        levelStatus[levelIndex] = 2; 
        GameEvents.RaiseLevelCompleted(levelIndex);

        int next = levelIndex + 1;
        if (next < totalLevels && levelStatus[next] == 0)
        {
            levelStatus[next] = 1;
            GameEvents.RaiseLevelUnlocked(next);
        }

        SaveProgress();
    }

    public void SetCurrentLevel(int levelIndex)
    {
        CurrentLevelIndex = levelIndex;
        SaveManager.Instance?.SaveCurrentLevel(levelIndex);
    }

    private void SaveProgress()
    {
        if (SaveManager.Instance == null) return;
        SaveManager.Instance.SaveLevelStatus(levelStatus);
    }
}