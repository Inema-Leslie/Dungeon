using UnityEngine;

public class PlayerSpawnHandler : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private CharacterController playerController; 
    [SerializeField] private Health playerHealth;
    [SerializeField] private Transform[] levelSpawnPoints; 
    [SerializeField] private ObjectiveMessageManager objectiveMessageManager;

    private void Start()
{
    int levelIndex = GameManager.Instance != null ? GameManager.Instance.CurrentLevelIndex : 0;

    if (levelIndex < 0 || levelIndex >= levelSpawnPoints.Length || levelSpawnPoints[levelIndex] == null)
    {
        Debug.LogWarning($"[PlayerSpawnHandler] No spawn point set for level index {levelIndex}.");
        return;
    }

    TeleportPlayer(levelSpawnPoints[levelIndex].position, levelSpawnPoints[levelIndex].rotation);
    RestorePlayerState();

    objectiveMessageManager?.ShowObjective(levelIndex); // NEW
}

    private void TeleportPlayer(Vector3 position, Quaternion rotation)
    {
        if (playerController != null) playerController.enabled = false; 

        player.position = position;
        player.rotation = rotation;

        if (playerController != null) playerController.enabled = true;

        Debug.Log($"[PlayerSpawnHandler] Player spawned at level index {GameManager.Instance.CurrentLevelIndex}.");
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