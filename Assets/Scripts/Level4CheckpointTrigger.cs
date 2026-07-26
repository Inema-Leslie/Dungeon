using UnityEngine;

public class Level4CheckpointTrigger : MonoBehaviour
{
    [SerializeField] private ArcherBehaviour[] archers;
    [SerializeField] private string playerTag = "Player";

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag(playerTag)) return;

        triggered = true;
        Debug.Log("[Level4CheckpointTrigger] Player reached checkpoint — stopping all Archers.");

        foreach (var archer in archers)
        {
            if (archer != null)
                archer.ForceStop();
        }
    }
}