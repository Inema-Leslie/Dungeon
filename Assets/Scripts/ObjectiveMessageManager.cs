using UnityEngine;
using TMPro;
using System.Collections;

public class ObjectiveMessageManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private float displayDuration = 4f;
    [SerializeField] private float fadeDuration = 1f;

    private readonly string[] objectives = new string[]
    {
        "Break out of your chains and find a weapon.",
        "Defeat the Black Paladin.",
        "Save the child from the monster.",
        "Get past the archers.",
        "Defeat the Guardian and escape the prison."
    };

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = objectiveText.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = objectiveText.gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0f;
    }

    public void ShowObjective(int levelIndex)
{
    Debug.Log($"[ObjectiveMessageManager] ShowObjective called with levelIndex: {levelIndex}"); 

    if (levelIndex < 0 || levelIndex >= objectives.Length)
    {
        Debug.LogWarning($"[ObjectiveMessageManager] No objective text for level index {levelIndex}.");
        return;
    }

    objectiveText.text = objectives[levelIndex];
    StopAllCoroutines();
    StartCoroutine(ShowThenFade());
}

    private IEnumerator ShowThenFade()
{
    Debug.Log($"[ObjectiveMessageManager] Coroutine started, setting alpha to 1."); 
    canvasGroup.alpha = 1f;
    yield return new WaitForSeconds(displayDuration);

    float timer = 0f;
    while (timer < fadeDuration)
    {
        timer += Time.deltaTime;
        canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
        yield return null;
    }
    canvasGroup.alpha = 0f;
    Debug.Log("[ObjectiveMessageManager] Fade complete, alpha back to 0."); // NEW
}
}