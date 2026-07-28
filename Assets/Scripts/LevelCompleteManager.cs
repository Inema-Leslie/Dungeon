using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteManager : MonoBehaviour
{
    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private TMPro.TextMeshProUGUI levelCompleteText;
    [SerializeField] private string levelSelectSceneName = "LevelSelect";

    private void OnEnable()
    {
        GameEvents.OnLevelCompleted += HandleLevelCompleted;
    }

    private void OnDisable()
    {
        GameEvents.OnLevelCompleted -= HandleLevelCompleted;
    }

    private void HandleLevelCompleted(int levelIndex)
    {
        Debug.Log($"[LevelCompleteManager] Level {levelIndex + 1} completed.");

        if (levelCompleteText != null)
        {
            levelCompleteText.text = $"Level {levelIndex + 1} Complete!";
        }

        levelCompletePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnContinueClicked()
    {
        Time.timeScale = 1f;
        levelCompletePanel.SetActive(false);
    }

    public void OnLevelSelectClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelSelectSceneName);
    }
}