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
    [SerializeField] private TMPro.TMP_InputField nameInputField; 
    

    [Header("Panels")]
    [SerializeField] private GameObject settingsPanel;

    [Header("Audio")]
    [SerializeField] private AudioClip introSound;

    private const string GameSceneName = "SampleScene";
    private const string LevelSelectSceneName = "LevelSelect";

    private void Start()
    {
        AudioManager.Instance?.PlayMusic(introSound); 
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
    public void OnNameSubmitted()
{
    Debug.Log("[MainMenuManager] OnNameSubmitted called.");

    if (nameInputField == null)
    {
        Debug.LogError("[MainMenuManager] nameInputField is not assigned!");
        return;
    }

    Debug.Log($"[MainMenuManager] Input text: '{nameInputField.text}'");

    if (!string.IsNullOrWhiteSpace(nameInputField.text))
    {
        PlayerProfileManager.Instance?.SetPlayerName(nameInputField.text);
    }
    else
    {
        Debug.LogWarning("[MainMenuManager] Input field is empty, not saving.");
    }
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