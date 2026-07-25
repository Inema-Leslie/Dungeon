
using UnityEngine;

public class Level3Manager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject childGameObject;

    [Header("Level Progression")]
    [SerializeField] private int levelIndex = 2; 

    private bool outcomeResolved = false;

    private void OnEnable()
    {
        GameEvents.OnChildSaved += HandleChildSaved;
        GameEvents.OnChildDied += HandleChildDied;
        GameEvents.OnPlayerDied += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        GameEvents.OnChildSaved -= HandleChildSaved;
        GameEvents.OnChildDied -= HandleChildDied;
        GameEvents.OnPlayerDied -= HandlePlayerDeath;
    }

    private void HandlePlayerDeath()
    {
        if (outcomeResolved) return;
        outcomeResolved = true;

        GameEvents.RaiseChildDied();
    }

    private void HandleChildSaved()
    {
        if (outcomeResolved) return;
        outcomeResolved = true;

        Debug.Log("Child saved! Level 3 complete.");

        if (childGameObject != null)
            childGameObject.SetActive(true);

        GameManager.Instance?.CompleteLevel(levelIndex);
    }

    private void HandleChildDied()
    {
        Debug.Log("Child died. Level 3 failed. (Reload level or show lose screen once UI exists.)");

        if (childGameObject != null)
            childGameObject.SetActive(false);

        
    }
}