using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    [System.Serializable]
    public class LevelButtonEntry
    {
        public Button button;
        public Image lockIcon;
    }

    [SerializeField] private LevelButtonEntry[] levelButtons;
    [SerializeField] private Sprite openLockSprite;
    [SerializeField] private Sprite closedLockSprite;
    [SerializeField] private string gameSceneName = "SampleScene";

    private void Start()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i; 
            bool unlocked = GameManager.Instance != null && GameManager.Instance.IsLevelUnlocked(levelIndex);

            levelButtons[i].button.interactable = unlocked;
            levelButtons[i].lockIcon.sprite = unlocked ? openLockSprite : closedLockSprite;

            levelButtons[i].button.onClick.RemoveAllListeners();
            levelButtons[i].button.onClick.AddListener(() => SelectLevel(levelIndex));
        }
    }

    private void SelectLevel(int levelIndex)
    {
        GameManager.Instance?.SetCurrentLevel(levelIndex);
        SceneManager.LoadScene(gameSceneName);
    }
}