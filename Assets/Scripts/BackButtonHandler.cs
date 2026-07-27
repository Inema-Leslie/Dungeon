using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonHandler : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private string levelSelectSceneName = "LevelSelect";

    public void TriggerBack()
{
    if (playerHealth != null && SaveManager.Instance != null)
    {
        var inventory = PlayerInventory.Instance != null
            ? PlayerInventory.Instance.CollectedItems
            : new System.Collections.Generic.List<string>();

        SaveManager.Instance.SavePlayerState(playerHealth.CurrentHealth, false, inventory);
    }

    SceneManager.LoadScene(levelSelectSceneName);
}
}