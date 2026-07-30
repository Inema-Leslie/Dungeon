using UnityEngine;

public class MonsterRoomEntryTrigger : MonoBehaviour
{
    [SerializeField] private WarriorBehaviour warrior;
    [SerializeField] private string playerTag = "Player";

    private bool triggered = false;

   private void OnTriggerEnter(Collider other)
{
    Debug.Log($"[MonsterRoomEntryTrigger] ANYTHING entered: {other.gameObject.name}, tag: '{other.tag}'");

    if (triggered) return;
    if (!other.CompareTag(playerTag)) return;

    triggered = true;
    Debug.Log("[MonsterRoomEntryTrigger] Player entered Monster room — stopping Warrior.");

    if (warrior != null)
    {
        warrior.ForceStop();
    }
}
}