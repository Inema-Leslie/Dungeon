using UnityEngine;

public class PlayerSpawnHandler : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private CharacterController playerController;
    [SerializeField] private Health playerHealth;
    [SerializeField] private Transform[] levelSpawnPoints;
    [SerializeField] private ObjectiveMessageManager objectiveMessageManager;
    [SerializeField] private AudioClip backgroundMusic;

   private void Start()
{
    int levelIndex = GameManager.Instance != null ? GameManager.Instance.CurrentLevelIndex : 0;

    if (levelIndex < 0 || levelIndex >= levelSpawnPoints.Length || levelSpawnPoints[levelIndex] == null)
    {
        Debug.LogWarning($"[PlayerSpawnHandler] No spawn point set for level index {levelIndex}.");
    }
    else
    {
        TeleportPlayer(levelSpawnPoints[levelIndex].position, levelSpawnPoints[levelIndex].rotation);
    }

    RestorePlayerState();
    objectiveMessageManager?.ShowObjective(levelIndex);
    AudioManager.Instance?.PlayMusic(backgroundMusic);
}

    private void TeleportPlayer(Vector3 position, Quaternion rotation)
{
    if (playerController != null) playerController.enabled = false;

    player.position = position;
    player.rotation = rotation;

    if (playerController != null) playerController.enabled = true;

    int currentLevel = GameManager.Instance != null ? GameManager.Instance.CurrentLevelIndex : 0;
    Debug.Log($"[PlayerSpawnHandler] Player spawned at level index {currentLevel}.");
}

    private void RestorePlayerState()
    {
        SaveData data = SaveManager.Instance?.LoadGame();
        if (data == null) return;

        if (playerHealth != null)
        {
            playerHealth.SetHealth(data.playerHealth);
        }
    }
}