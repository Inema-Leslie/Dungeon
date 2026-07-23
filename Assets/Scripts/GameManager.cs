using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Level Progression")]
    [SerializeField] private int totalLevels = 5;
    private int[] levelStatus; // 0 = locked, 1 = available, 2 = completed

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
        }
        else
        {
            levelStatus[0] = 1; // Level 1 always available
            for (int i = 1; i < totalLevels; i++) levelStatus[i] = 0;
        }
    }

    public int GetLevelStatus(int levelIndex) => levelStatus[levelIndex];

    public bool IsLevelUnlocked(int levelIndex) => levelStatus[levelIndex] >= 1;

    public void CompleteLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= totalLevels) return;

        levelStatus[levelIndex] = 2; // completed
        GameEvents.RaiseLevelCompleted(levelIndex);

        int next = levelIndex + 1;
        if (next < totalLevels && levelStatus[next] == 0)
        {
            levelStatus[next] = 1;
            GameEvents.RaiseLevelUnlocked(next);
        }

        SaveProgress();
    }

    public void SetCurrentLevel(int levelIndex) => CurrentLevelIndex = levelIndex;

    private void SaveProgress()
    {
        if (SaveManager.Instance == null) return;
        SaveManager.Instance.SaveLevelStatus(levelStatus);
    }
}