using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private void OnEnable()
    {
        GameEvents.OnGuardianDefeated += HandleGuardianDefeated;
    }

    private void OnDisable()
    {
        GameEvents.OnGuardianDefeated -= HandleGuardianDefeated;
    }

    private void HandleGuardianDefeated()
    {
        Debug.Log("[VictoryManager] Guardian defeated — showing Victory screen.");
        victoryPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnMainMenuClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}