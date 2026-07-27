using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button continueGameButton;
    [SerializeField] private Button levelSelectionButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;

    [Header("Panels")]
    [SerializeField] private GameObject settingsPanel;

    private const string GameSceneName = "SampleScene";
    private const string LevelSelectSceneName = "LevelSelect";

    private void Start()
    {
        bool hasSave = SaveManager.Instance != null && SaveManager.Instance.HasSaveFile();
        continueGameButton.interactable = hasSave;

        startGameButton.onClick.AddListener(OnStartGame);
        continueGameButton.onClick.AddListener(OnContinueGame);
        levelSelectionButton.onClick.AddListener(OnLevelSelection);
        settingsButton.onClick.AddListener(OnOpenSettings);
        exitButton.onClick.AddListener(OnExit);
    }

    private void OnStartGame()
    {
        // Fresh save — wipes progress, starts at Level 1
        SaveManager.Instance?.DeleteSave();
        GameManager.Instance?.SetCurrentLevel(0);
        SceneManager.LoadScene(GameSceneName);
    }

    private void OnContinueGame()
    {
        
        SceneManager.LoadScene(GameSceneName);
    }

    private void OnLevelSelection()
    {
        SceneManager.LoadScene(LevelSelectSceneName);
    }

    private void OnOpenSettings()
    {
        settingsPanel?.SetActive(true);
    }

    private void OnExit()
    {
        Debug.Log("[MainMenuManager] Exiting game.");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}